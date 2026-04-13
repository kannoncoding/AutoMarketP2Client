/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica encargada de administrar la autenticación del cliente, validar la conexión TCP con el servidor y sincronizar el estado de la sesión actual en AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using AutoMarket.Cliente.Comunicacion;

namespace AutoMarket.Cliente.Logica
{
    public sealed class AutenticacionClienteLogica
    {
        private readonly ClienteTcp _clienteTcp;
        private readonly SesionCliente _sesionCliente;

        public ClienteTcp ClienteTcp
        {
            get => _clienteTcp;
        }

        public SesionCliente SesionCliente
        {
            get => _sesionCliente;
        }

        public AutenticacionClienteLogica(ClienteTcp clienteTcp, SesionCliente sesionCliente)
        {
            _clienteTcp = clienteTcp ?? throw new ArgumentNullException(nameof(clienteTcp), "La instancia de ClienteTcp es obligatoria.");
            _sesionCliente = sesionCliente ?? throw new ArgumentNullException(nameof(sesionCliente), "La instancia de SesionCliente es obligatoria.");
        }

        public void ConectarServidor()
        {
            if (_clienteTcp.EstaConectado)
            {
                throw new InvalidOperationException("Ya existe una conexión activa con el servidor.");
            }

            _clienteTcp.Conectar();
            _sesionCliente.RegistrarConexion(_clienteTcp.DireccionServidor, _clienteTcp.PuertoServidor);
        }

        public void DesconectarServidor()
        {
            try
            {
                if (_clienteTcp.EstaConectado)
                {
                    string respuesta = _clienteTcp.Desconectar();

                    if (!InterpretadorRespuestas.EsDesconexionAceptada(respuesta))
                    {
                        throw new InvalidOperationException("El servidor no confirmó correctamente la desconexión solicitada.");
                    }
                }
            }
            finally
            {
                _sesionCliente.CerrarSesion();
            }
        }

        public bool ProbarConexionServidor()
        {
            _sesionCliente.ValidarConexionActiva();

            if (!_clienteTcp.EstaConectado)
            {
                _sesionCliente.CerrarSesion();
                throw new InvalidOperationException("La sesión indica conexión activa, pero el canal TCP ya no se encuentra conectado.");
            }

            string respuesta = _clienteTcp.Ping();
            return InterpretadorRespuestas.EsPingExitoso(respuesta);
        }

        public ResultadoAutenticacionCliente AutenticarCliente(int idCliente)
        {
            ValidarIdCliente(idCliente);
            _sesionCliente.ValidarConexionActiva();

            if (!_clienteTcp.EstaConectado)
            {
                _sesionCliente.CerrarSesion();
                throw new InvalidOperationException("No existe una conexión TCP activa con el servidor.");
            }

            string respuestaTexto = _clienteTcp.Login(idCliente);
            RespuestaServidor respuesta = InterpretadorRespuestas.InterpretarRespuesta(respuestaTexto);

            if (respuesta.EsError)
            {
                _sesionCliente.LimpiarAutenticacion();

                return new ResultadoAutenticacionCliente(
                    false,
                    0,
                    string.Empty,
                    respuesta.MensajeError,
                    respuesta.Operacion);
            }

            respuesta.ValidarExitoYOperacion(MensajesProtocolo.OperacionLogin);

            int idClienteAutenticado = InterpretadorRespuestas.ObtenerIdClienteDesdeLogin(respuestaTexto);
            string nombreCliente = InterpretadorRespuestas.ObtenerNombreClienteDesdeLogin(respuestaTexto);

            _sesionCliente.RegistrarAutenticacion(idClienteAutenticado, nombreCliente);

            return new ResultadoAutenticacionCliente(
                true,
                idClienteAutenticado,
                nombreCliente,
                "Cliente autenticado correctamente.",
                respuesta.Operacion);
        }

        public ResultadoAutenticacionCliente ReautenticarClienteActual()
        {
            _sesionCliente.ValidarClienteAutenticado();
            int idCliente = _sesionCliente.ObtenerIdClienteRequerido();
            return AutenticarCliente(idCliente);
        }

        public ClienteAutenticadoInfo ConsultarClientePorId(int idCliente)
        {
            ValidarIdCliente(idCliente);
            _sesionCliente.ValidarConexionActiva();

            if (!_clienteTcp.EstaConectado)
            {
                _sesionCliente.CerrarSesion();
                throw new InvalidOperationException("No existe una conexión TCP activa con el servidor.");
            }

            string respuestaTexto = _clienteTcp.ObtenerClientePorId(idCliente);
            RespuestaServidor respuesta = InterpretadorRespuestas.InterpretarRespuesta(respuestaTexto);

            if (respuesta.EsError)
            {
                throw new InvalidOperationException(respuesta.MensajeError);
            }

            string[] columnas = InterpretadorRespuestas.ObtenerRegistroClientePorId(respuestaTexto);

            int id = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                0,
                "El id del cliente recibido no es válido.");

            string identificacion = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                1,
                "La identificación del cliente es obligatoria.");

            string nombreCompleto = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                2,
                "El nombre completo del cliente es obligatorio.");

            DateTime fechaNacimiento = InterpretadorRespuestas.ObtenerFechaColumna(
                columnas,
                3,
                "La fecha de nacimiento del cliente no es válida.");

            DateTime fechaRegistro = InterpretadorRespuestas.ObtenerFechaColumna(
                columnas,
                4,
                "La fecha de registro del cliente no es válida.");

            bool activo = InterpretadorRespuestas.ObtenerBooleanoBinarioColumna(
                columnas,
                5,
                "El estado activo del cliente no es válido.");

            return new ClienteAutenticadoInfo(
                id,
                identificacion,
                nombreCompleto,
                fechaNacimiento,
                fechaRegistro,
                activo);
        }

        public bool HayConexionActiva()
        {
            return _clienteTcp.EstaConectado && _sesionCliente.TieneConexionActiva;
        }

        public bool HayClienteAutenticado()
        {
            return _sesionCliente.TieneClienteAutenticado;
        }

        public int ObtenerIdClienteAutenticado()
        {
            return _sesionCliente.ObtenerIdClienteRequerido();
        }

        public string ObtenerNombreClienteAutenticado()
        {
            return _sesionCliente.ObtenerNombreClienteRequerido();
        }

        public string ObtenerResumenSesion()
        {
            return _sesionCliente.ResumenSesion;
        }

        public void CerrarAutenticacionLocal()
        {
            _sesionCliente.LimpiarAutenticacion();
        }

        private void ValidarIdCliente(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.", nameof(idCliente));
            }
        }
    }

    public sealed class ResultadoAutenticacionCliente
    {
        private readonly bool _autenticacionExitosa;
        private readonly int _idCliente;
        private readonly string _nombreCliente;
        private readonly string _mensaje;
        private readonly string _operacion;

        public bool AutenticacionExitosa
        {
            get => _autenticacionExitosa;
        }

        public int IdCliente
        {
            get => _idCliente;
        }

        public string NombreCliente
        {
            get => _nombreCliente;
        }

        public string Mensaje
        {
            get => _mensaje;
        }

        public string Operacion
        {
            get => _operacion;
        }

        public ResultadoAutenticacionCliente(
            bool autenticacionExitosa,
            int idCliente,
            string nombreCliente,
            string mensaje,
            string operacion)
        {
            _autenticacionExitosa = autenticacionExitosa;
            _idCliente = idCliente;
            _nombreCliente = nombreCliente?.Trim() ?? string.Empty;
            _mensaje = mensaje?.Trim() ?? string.Empty;
            _operacion = operacion?.Trim() ?? string.Empty;
        }
    }

    public sealed class ClienteAutenticadoInfo
    {
        private readonly int _idCliente;
        private readonly string _identificacion;
        private readonly string _nombreCompleto;
        private readonly DateTime _fechaNacimiento;
        private readonly DateTime _fechaRegistro;
        private readonly bool _activo;

        public int IdCliente
        {
            get => _idCliente;
        }

        public string Identificacion
        {
            get => _identificacion;
        }

        public string NombreCompleto
        {
            get => _nombreCompleto;
        }

        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
        }

        public DateTime FechaRegistro
        {
            get => _fechaRegistro;
        }

        public bool Activo
        {
            get => _activo;
        }

        public ClienteAutenticadoInfo(
            int idCliente,
            string identificacion,
            string nombreCompleto,
            DateTime fechaNacimiento,
            DateTime fechaRegistro,
            bool activo)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.", nameof(idCliente));
            }

            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.", nameof(identificacion));
            }

            string nombreNormalizado = nombreCompleto?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                throw new ArgumentException("El nombre completo del cliente es obligatorio.", nameof(nombreCompleto));
            }

            _idCliente = idCliente;
            _identificacion = identificacionNormalizada;
            _nombreCompleto = nombreNormalizado;
            _fechaNacimiento = fechaNacimiento;
            _fechaRegistro = fechaRegistro;
            _activo = activo;
        }
    }
}