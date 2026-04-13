/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase encargada de administrar la conexión TCP del cliente AutoMarket, enviar solicitudes protocolarias al servidor y recibir respuestas de forma segura.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace AutoMarket.Cliente.Comunicacion
{
    public sealed class ClienteTcp : IDisposable
    {
        private readonly string _direccionServidor;
        private readonly int _puertoServidor;
        private readonly object _bloqueoEstado;
        private readonly object _bloqueoComunicacion;

        private TcpClient? _clienteTcp;
        private NetworkStream? _flujoRed;
        private StreamReader? _lector;
        private StreamWriter? _escritor;
        private bool _estaConectado;
        private bool _disposed;

        public event Action<string>? EventoCliente;

        public string DireccionServidor
        {
            get => _direccionServidor;
        }

        public int PuertoServidor
        {
            get => _puertoServidor;
        }

        public int TiempoEsperaEnvioMs { get; set; }

        public int TiempoEsperaRecepcionMs { get; set; }

        public bool EstaConectado
        {
            get
            {
                lock (_bloqueoEstado)
                {
                    return _estaConectado;
                }
            }
        }

        public ClienteTcp()
            : this("127.0.0.1", 15500)
        {
        }

        public ClienteTcp(string direccionServidor, int puertoServidor)
        {
            if (string.IsNullOrWhiteSpace(direccionServidor))
            {
                throw new ArgumentException("La dirección del servidor es obligatoria.", nameof(direccionServidor));
            }

            if (puertoServidor <= 0 || puertoServidor > 65535)
            {
                throw new ArgumentException("El puerto del servidor no es válido.", nameof(puertoServidor));
            }

            _direccionServidor = direccionServidor.Trim();
            _puertoServidor = puertoServidor;
            _bloqueoEstado = new object();
            _bloqueoComunicacion = new object();
            TiempoEsperaEnvioMs = 15000;
            TiempoEsperaRecepcionMs = 15000;
        }

        public void Conectar()
        {
            VerificarNoDesechado();

            lock (_bloqueoEstado)
            {
                if (_estaConectado)
                {
                    throw new InvalidOperationException("El cliente TCP ya se encuentra conectado al servidor.");
                }
            }

            NotificarEvento("Intentando conectar con el servidor " + _direccionServidor + ":" + _puertoServidor + ".");

            TcpClient clienteTemporal = new TcpClient();

            try
            {
                clienteTemporal.SendTimeout = TiempoEsperaEnvioMs;
                clienteTemporal.ReceiveTimeout = TiempoEsperaRecepcionMs;
                clienteTemporal.Connect(_direccionServidor, _puertoServidor);

                NetworkStream flujoTemporal = clienteTemporal.GetStream();
                StreamReader lectorTemporal = new StreamReader(flujoTemporal, Encoding.UTF8, false, 1024, true);
                StreamWriter escritorTemporal = new StreamWriter(flujoTemporal, Encoding.UTF8, 1024, true)
                {
                    AutoFlush = true
                };

                string respuestaConexion = LeerRespuestaInterna(lectorTemporal);

                ValidarRespuestaConexionInicial(respuestaConexion);

                lock (_bloqueoEstado)
                {
                    _clienteTcp = clienteTemporal;
                    _flujoRed = flujoTemporal;
                    _lector = lectorTemporal;
                    _escritor = escritorTemporal;
                    _estaConectado = true;
                }

                NotificarEvento("Conexión TCP establecida correctamente con el servidor.");
            }
            catch (SocketException ex)
            {
                try
                {
                    clienteTemporal.Close();
                }
                catch
                {
                }

                throw new InvalidOperationException("No fue posible establecer la conexión con el servidor TCP.", ex);
            }
            catch (IOException ex)
            {
                try
                {
                    clienteTemporal.Close();
                }
                catch
                {
                }

                throw new InvalidOperationException("Ocurrió un error de entrada/salida durante la conexión inicial con el servidor.", ex);
            }
            catch (Exception)
            {
                try
                {
                    clienteTemporal.Close();
                }
                catch
                {
                }

                throw;
            }
        }

        public string Desconectar()
        {
            VerificarNoDesechado();

            lock (_bloqueoEstado)
            {
                if (!_estaConectado)
                {
                    return "OK|DESCONEXION|ACEPTADA";
                }
            }

            string respuestaDesconexion = "OK|DESCONEXION|ACEPTADA";

            try
            {
                lock (_bloqueoComunicacion)
                {
                    VerificarConectado();

                    if (_escritor == null || _lector == null)
                    {
                        throw new InvalidOperationException("La conexión TCP no se encuentra correctamente inicializada.");
                    }

                    _escritor.WriteLine("LOGOUT");
                    _escritor.Flush();

                    respuestaDesconexion = LeerRespuestaInterna(_lector);
                }
            }
            catch (IOException)
            {
                respuestaDesconexion = "OK|DESCONEXION|ACEPTADA";
            }
            catch (SocketException)
            {
                respuestaDesconexion = "OK|DESCONEXION|ACEPTADA";
            }
            catch (ObjectDisposedException)
            {
                respuestaDesconexion = "OK|DESCONEXION|ACEPTADA";
            }
            finally
            {
                LiberarRecursosConexion();
                NotificarEvento("Conexión TCP finalizada.");
            }

            return respuestaDesconexion;
        }

        public string EnviarSolicitud(string solicitud)
        {
            VerificarNoDesechado();

            string solicitudNormalizada = solicitud?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(solicitudNormalizada))
            {
                throw new ArgumentException("La solicitud a enviar es obligatoria.", nameof(solicitud));
            }

            lock (_bloqueoComunicacion)
            {
                VerificarConectado();

                if (_escritor == null || _lector == null)
                {
                    throw new InvalidOperationException("La conexión TCP no se encuentra correctamente inicializada.");
                }

                try
                {
                    NotificarEvento("Solicitud enviada: " + solicitudNormalizada);

                    _escritor.WriteLine(solicitudNormalizada);
                    _escritor.Flush();

                    string respuesta = LeerRespuestaInterna(_lector);

                    NotificarEvento("Respuesta recibida: " + respuesta);

                    return respuesta;
                }
                catch (IOException ex)
                {
                    LiberarRecursosConexion();
                    throw new InvalidOperationException("Se produjo un error de entrada/salida durante la comunicación con el servidor.", ex);
                }
                catch (SocketException ex)
                {
                    LiberarRecursosConexion();
                    throw new InvalidOperationException("Se produjo un error de socket durante la comunicación con el servidor.", ex);
                }
                catch (ObjectDisposedException ex)
                {
                    LiberarRecursosConexion();
                    throw new InvalidOperationException("La conexión con el servidor fue cerrada mientras se procesaba la solicitud.", ex);
                }
            }
        }

        public string Ping()
        {
            return EnviarSolicitud("PING");
        }

        public string Login(int idCliente)
        {
            return EnviarSolicitud("LOGIN|" + ConvertirEntero(idCliente, nameof(idCliente)));
        }

        public string ObtenerSucursalesActivas()
        {
            return EnviarSolicitud("SUCURSALES_ACTIVAS");
        }

        public string ObtenerVehiculosPorSucursal(int idSucursal)
        {
            return EnviarSolicitud("VEHICULOS_POR_SUCURSAL|" + ConvertirEntero(idSucursal, nameof(idSucursal)));
        }

        public string RegistrarVenta(int idCliente, int idSucursal, int idVehiculo)
        {
            return EnviarSolicitud(
                "VENTA|"
                + ConvertirEntero(idCliente, nameof(idCliente))
                + "|"
                + ConvertirEntero(idSucursal, nameof(idSucursal))
                + "|"
                + ConvertirEntero(idVehiculo, nameof(idVehiculo)));
        }

        public string ObtenerVentasPorCliente(int idCliente)
        {
            return EnviarSolicitud("VENTAS_POR_CLIENTE|" + ConvertirEntero(idCliente, nameof(idCliente)));
        }

        public string ObtenerClientePorId(int idCliente)
        {
            return EnviarSolicitud("CLIENTE_POR_ID|" + ConvertirEntero(idCliente, nameof(idCliente)));
        }

        public string ObtenerVehiculoPorId(int idVehiculo)
        {
            return EnviarSolicitud("VEHICULO_POR_ID|" + ConvertirEntero(idVehiculo, nameof(idVehiculo)));
        }

        public bool ProbarConexion()
        {
            string respuesta = Ping();
            return string.Equals(
                respuesta,
                "OK|PING|PONG",
                StringComparison.OrdinalIgnoreCase);
        }

        private void ValidarRespuestaConexionInicial(string respuestaConexion)
        {
            string respuestaNormalizada = respuestaConexion?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(respuestaNormalizada))
            {
                throw new InvalidOperationException("El servidor no devolvió una respuesta válida al establecer la conexión.");
            }

            if (!string.Equals(
                respuestaNormalizada,
                "OK|CONEXION|ESTABLECIDA",
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("El servidor devolvió una respuesta inicial no reconocida: " + respuestaNormalizada);
            }
        }

        private string LeerRespuestaInterna(StreamReader lector)
        {
            if (lector == null)
            {
                throw new ArgumentNullException(nameof(lector), "El lector TCP es obligatorio.");
            }

            string? respuesta = lector.ReadLine();

            if (respuesta == null)
            {
                throw new IOException("El servidor cerró la conexión antes de enviar una respuesta.");
            }

            string respuestaNormalizada = respuesta.Trim();

            if (string.IsNullOrWhiteSpace(respuestaNormalizada))
            {
                throw new IOException("El servidor devolvió una respuesta vacía.");
            }

            return respuestaNormalizada;
        }

        private string ConvertirEntero(int valor, string nombreParametro)
        {
            if (valor <= 0)
            {
                throw new ArgumentException("El valor indicado para " + nombreParametro + " debe ser mayor que cero.", nombreParametro);
            }

            return valor.ToString(CultureInfo.InvariantCulture);
        }

        private void VerificarConectado()
        {
            lock (_bloqueoEstado)
            {
                if (!_estaConectado || _clienteTcp == null || _flujoRed == null || _lector == null || _escritor == null)
                {
                    throw new InvalidOperationException("No existe una conexión TCP activa con el servidor.");
                }
            }
        }

        private void VerificarNoDesechado()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ClienteTcp), "La instancia de ClienteTcp ya fue liberada.");
            }
        }

        private void LiberarRecursosConexion()
        {
            lock (_bloqueoEstado)
            {
                _estaConectado = false;

                try
                {
                    _escritor?.Dispose();
                }
                catch
                {
                }

                try
                {
                    _lector?.Dispose();
                }
                catch
                {
                }

                try
                {
                    _flujoRed?.Dispose();
                }
                catch
                {
                }

                try
                {
                    _clienteTcp?.Close();
                }
                catch
                {
                }

                _escritor = null;
                _lector = null;
                _flujoRed = null;
                _clienteTcp = null;
            }
        }

        private void NotificarEvento(string mensaje)
        {
            string mensajeNormalizado = mensaje?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(mensajeNormalizado))
            {
                return;
            }

            string mensajeConFecha = "[" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture) + "] " + mensajeNormalizado;
            EventoCliente?.Invoke(mensajeConFecha);
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                Desconectar();
            }
            catch
            {
                LiberarRecursosConexion();
            }

            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}