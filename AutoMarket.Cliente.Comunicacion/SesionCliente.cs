/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase encargada de representar y administrar el estado actual de la sesión del cliente dentro de la aplicación AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Globalization;

namespace AutoMarket.Cliente.Logica
{
    public sealed class SesionCliente
    {
        private readonly object _bloqueoSesion;

        private string _direccionServidor;
        private int _puertoServidor;
        private bool _conectado;
        private bool _autenticado;
        private int _idCliente;
        private string _nombreCliente;
        private DateTime? _fechaHoraConexion;
        private DateTime? _fechaHoraAutenticacion;

        public string DireccionServidor
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _direccionServidor;
                }
            }
            private set
            {
                lock (_bloqueoSesion)
                {
                    _direccionServidor = value;
                }
            }
        }

        public int PuertoServidor
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _puertoServidor;
                }
            }
            private set
            {
                lock (_bloqueoSesion)
                {
                    _puertoServidor = value;
                }
            }
        }

        public bool Conectado
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _conectado;
                }
            }
            private set
            {
                lock (_bloqueoSesion)
                {
                    _conectado = value;
                }
            }
        }

        public bool Autenticado
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _autenticado;
                }
            }
            private set
            {
                lock (_bloqueoSesion)
                {
                    _autenticado = value;
                }
            }
        }

        public int IdCliente
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _idCliente;
                }
            }
            private set
            {
                lock (_bloqueoSesion)
                {
                    _idCliente = value;
                }
            }
        }

        public string NombreCliente
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _nombreCliente;
                }
            }
            private set
            {
                lock (_bloqueoSesion)
                {
                    _nombreCliente = value;
                }
            }
        }

        public DateTime? FechaHoraConexion
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _fechaHoraConexion;
                }
            }
            private set
            {
                lock (_bloqueoSesion)
                {
                    _fechaHoraConexion = value;
                }
            }
        }

        public DateTime? FechaHoraAutenticacion
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _fechaHoraAutenticacion;
                }
            }
            private set
            {
                lock (_bloqueoSesion)
                {
                    _fechaHoraAutenticacion = value;
                }
            }
        }

        public bool TieneConexionActiva
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _conectado;
                }
            }
        }

        public bool TieneClienteAutenticado
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    return _autenticado && _idCliente > 0 && !string.IsNullOrWhiteSpace(_nombreCliente);
                }
            }
        }

        public string ResumenSesion
        {
            get
            {
                lock (_bloqueoSesion)
                {
                    if (!_conectado)
                    {
                        return "Sin conexión activa.";
                    }

                    if (!_autenticado)
                    {
                        return "Conectado a " + _direccionServidor + ":" + _puertoServidor.ToString(CultureInfo.InvariantCulture) + " sin autenticación.";
                    }

                    return "Conectado a "
                        + _direccionServidor
                        + ":"
                        + _puertoServidor.ToString(CultureInfo.InvariantCulture)
                        + " como cliente "
                        + _idCliente.ToString(CultureInfo.InvariantCulture)
                        + " - "
                        + _nombreCliente
                        + ".";
                }
            }
        }

        public SesionCliente()
        {
            _bloqueoSesion = new object();
            _direccionServidor = string.Empty;
            _puertoServidor = 0;
            _conectado = false;
            _autenticado = false;
            _idCliente = 0;
            _nombreCliente = string.Empty;
            _fechaHoraConexion = null;
            _fechaHoraAutenticacion = null;
        }

        public void RegistrarConexion(string direccionServidor, int puertoServidor)
        {
            string direccionNormalizada = direccionServidor?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(direccionNormalizada))
            {
                throw new ArgumentException("La dirección del servidor es obligatoria.", nameof(direccionServidor));
            }

            if (puertoServidor <= 0 || puertoServidor > 65535)
            {
                throw new ArgumentException("El puerto del servidor no es válido.", nameof(puertoServidor));
            }

            lock (_bloqueoSesion)
            {
                _direccionServidor = direccionNormalizada;
                _puertoServidor = puertoServidor;
                _conectado = true;
                _fechaHoraConexion = DateTime.Now;

                _autenticado = false;
                _idCliente = 0;
                _nombreCliente = string.Empty;
                _fechaHoraAutenticacion = null;
            }
        }

        public void RegistrarAutenticacion(int idCliente, string nombreCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente autenticado debe ser mayor que cero.", nameof(idCliente));
            }

            string nombreNormalizado = nombreCliente?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                throw new ArgumentException("El nombre del cliente autenticado es obligatorio.", nameof(nombreCliente));
            }

            lock (_bloqueoSesion)
            {
                if (!_conectado)
                {
                    throw new InvalidOperationException("No es posible registrar autenticación porque no existe una conexión activa.");
                }

                _autenticado = true;
                _idCliente = idCliente;
                _nombreCliente = nombreNormalizado;
                _fechaHoraAutenticacion = DateTime.Now;
            }
        }

        public void LimpiarAutenticacion()
        {
            lock (_bloqueoSesion)
            {
                _autenticado = false;
                _idCliente = 0;
                _nombreCliente = string.Empty;
                _fechaHoraAutenticacion = null;
            }
        }

        public void CerrarSesion()
        {
            lock (_bloqueoSesion)
            {
                _direccionServidor = string.Empty;
                _puertoServidor = 0;
                _conectado = false;
                _autenticado = false;
                _idCliente = 0;
                _nombreCliente = string.Empty;
                _fechaHoraConexion = null;
                _fechaHoraAutenticacion = null;
            }
        }

        public void ValidarConexionActiva()
        {
            lock (_bloqueoSesion)
            {
                if (!_conectado)
                {
                    throw new InvalidOperationException("No existe una conexión activa con el servidor.");
                }
            }
        }

        public void ValidarClienteAutenticado()
        {
            lock (_bloqueoSesion)
            {
                if (!_conectado)
                {
                    throw new InvalidOperationException("No existe una conexión activa con el servidor.");
                }

                if (!_autenticado || _idCliente <= 0 || string.IsNullOrWhiteSpace(_nombreCliente))
                {
                    throw new InvalidOperationException("No existe un cliente autenticado en la sesión actual.");
                }
            }
        }

        public bool CorrespondeAlClienteAutenticado(int idCliente)
        {
            if (idCliente <= 0)
            {
                return false;
            }

            lock (_bloqueoSesion)
            {
                return _autenticado && _idCliente == idCliente;
            }
        }

        public string ObtenerDireccionServidorRequerida()
        {
            lock (_bloqueoSesion)
            {
                if (string.IsNullOrWhiteSpace(_direccionServidor))
                {
                    throw new InvalidOperationException("La sesión no contiene una dirección de servidor válida.");
                }

                return _direccionServidor;
            }
        }

        public int ObtenerPuertoServidorRequerido()
        {
            lock (_bloqueoSesion)
            {
                if (_puertoServidor <= 0 || _puertoServidor > 65535)
                {
                    throw new InvalidOperationException("La sesión no contiene un puerto de servidor válido.");
                }

                return _puertoServidor;
            }
        }

        public int ObtenerIdClienteRequerido()
        {
            lock (_bloqueoSesion)
            {
                if (!_autenticado || _idCliente <= 0)
                {
                    throw new InvalidOperationException("La sesión no contiene un id de cliente autenticado válido.");
                }

                return _idCliente;
            }
        }

        public string ObtenerNombreClienteRequerido()
        {
            lock (_bloqueoSesion)
            {
                if (!_autenticado || string.IsNullOrWhiteSpace(_nombreCliente))
                {
                    throw new InvalidOperationException("La sesión no contiene un nombre de cliente autenticado válido.");
                }

                return _nombreCliente;
            }
        }

        public TimeSpan? ObtenerDuracionConexion()
        {
            lock (_bloqueoSesion)
            {
                if (!_fechaHoraConexion.HasValue)
                {
                    return null;
                }

                return DateTime.Now - _fechaHoraConexion.Value;
            }
        }

        public TimeSpan? ObtenerDuracionAutenticacion()
        {
            lock (_bloqueoSesion)
            {
                if (!_fechaHoraAutenticacion.HasValue)
                {
                    return null;
                }

                return DateTime.Now - _fechaHoraAutenticacion.Value;
            }
        }

        public override string ToString()
        {
            return ResumenSesion;
        }
    }
}