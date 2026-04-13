/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica encargada de consultar sucursales activas desde el servidor TCP, interpretar sus respuestas protocolarias y convertirlas en objetos de entidad del sistema AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Collections.Generic;
using AutoMarket.Cliente.Comunicacion;
using AutoMarket.Entidades;

namespace AutoMarket.Cliente.Logica
{
    public sealed class SucursalClienteLogica
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

        public SucursalClienteLogica(ClienteTcp clienteTcp, SesionCliente sesionCliente)
        {
            _clienteTcp = clienteTcp ?? throw new ArgumentNullException(nameof(clienteTcp), "La instancia de ClienteTcp es obligatoria.");
            _sesionCliente = sesionCliente ?? throw new ArgumentNullException(nameof(sesionCliente), "La instancia de SesionCliente es obligatoria.");
        }

        public List<Sucursal> ObtenerSucursalesActivas()
        {
            ValidarConexionCliente();

            string respuestaTexto = _clienteTcp.ObtenerSucursalesActivas();
            RespuestaServidor respuesta = InterpretadorRespuestas.InterpretarRespuesta(respuestaTexto);

            if (respuesta.EsError)
            {
                throw new InvalidOperationException(respuesta.MensajeError);
            }

            List<string[]> registros = InterpretadorRespuestas.ObtenerRegistrosSucursalesActivas(respuestaTexto);
            List<Sucursal> sucursales = new List<Sucursal>();

            for (int i = 0; i < registros.Count; i++)
            {
                Sucursal sucursal = ConstruirSucursalDesdeColumnas(registros[i]);

                if (sucursal.Activo)
                {
                    sucursales.Add(sucursal);
                }
            }

            return sucursales;
        }

        public Sucursal ObtenerSucursalActivaPorId(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            List<Sucursal> sucursales = ObtenerSucursalesActivas();

            for (int i = 0; i < sucursales.Count; i++)
            {
                if (sucursales[i].IdSucursal == idSucursal)
                {
                    return sucursales[i];
                }
            }

            throw new InvalidOperationException("No existe una sucursal activa con el id indicado.");
        }

        public bool ExisteSucursalActiva(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                return false;
            }

            List<Sucursal> sucursales = ObtenerSucursalesActivas();

            for (int i = 0; i < sucursales.Count; i++)
            {
                if (sucursales[i].IdSucursal == idSucursal)
                {
                    return true;
                }
            }

            return false;
        }

        public List<SucursalVistaInfo> ObtenerSucursalesActivasParaVista()
        {
            List<Sucursal> sucursales = ObtenerSucursalesActivas();
            List<SucursalVistaInfo> resultado = new List<SucursalVistaInfo>();

            for (int i = 0; i < sucursales.Count; i++)
            {
                Sucursal sucursal = sucursales[i];

                resultado.Add(new SucursalVistaInfo(
                    sucursal.IdSucursal,
                    sucursal.Nombre,
                    sucursal.Direccion,
                    sucursal.Telefono,
                    sucursal.VendedorEncargado.IdVendedor,
                    sucursal.VendedorEncargado.NombreCompleto,
                    sucursal.VendedorEncargado.Identificacion,
                    sucursal.Activo));
            }

            return resultado;
        }

        private void ValidarConexionCliente()
        {
            _sesionCliente.ValidarConexionActiva();

            if (!_clienteTcp.EstaConectado)
            {
                _sesionCliente.CerrarSesion();
                throw new InvalidOperationException("No existe una conexión TCP activa con el servidor.");
            }
        }

        private Sucursal ConstruirSucursalDesdeColumnas(string[] columnas)
        {
            if (columnas == null)
            {
                throw new ArgumentNullException(nameof(columnas), "Las columnas de la sucursal son obligatorias.");
            }

            InterpretadorRespuestas.ValidarCantidadColumnas(
                columnas,
                InterpretadorRespuestas.CantidadColumnasSucursal,
                MensajesProtocolo.OperacionSucursalesActivas);

            int idSucursal = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                0,
                "El id de la sucursal recibido no es válido.");

            string nombreSucursal = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                1,
                "El nombre de la sucursal es obligatorio.");

            string direccionSucursal = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                2,
                "La dirección de la sucursal es obligatoria.");

            string telefonoSucursal = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                3,
                "El teléfono de la sucursal es obligatorio.");

            int idVendedor = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                4,
                "El id del vendedor encargado no es válido.");

            string nombreVendedor = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                5,
                "El nombre del vendedor encargado es obligatorio.");

            string identificacionVendedor = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                6,
                "La identificación del vendedor encargado es obligatoria.");

            bool activo = InterpretadorRespuestas.ObtenerBooleanoBinarioColumna(
                columnas,
                7,
                "El estado activo de la sucursal no es válido.");

            Vendedor vendedor = ConstruirVendedorDesdeDatosMinimos(
                idVendedor,
                identificacionVendedor,
                nombreVendedor);

            return new Sucursal(
                idSucursal,
                nombreSucursal,
                direccionSucursal,
                telefonoSucursal,
                vendedor,
                activo);
        }

        private Vendedor ConstruirVendedorDesdeDatosMinimos(int idVendedor, string identificacion, string nombreCompleto)
        {
            if (idVendedor <= 0)
            {
                throw new ArgumentException("El id del vendedor debe ser mayor que cero.", nameof(idVendedor));
            }

            string identificacionNormalizada = identificacion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del vendedor es obligatoria.", nameof(identificacion));
            }

            string nombreNormalizado = nombreCompleto?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                throw new ArgumentException("El nombre completo del vendedor es obligatorio.", nameof(nombreCompleto));
            }

            DateTime fechaNacimientoTecnica = new DateTime(1900, 1, 1);
            DateTime fechaIngresoTecnica = new DateTime(2000, 1, 1);
            string telefonoTecnico = "NO DISPONIBLE";

            return new Vendedor(
                idVendedor,
                identificacionNormalizada,
                nombreNormalizado,
                fechaNacimientoTecnica,
                fechaIngresoTecnica,
                telefonoTecnico);
        }
    }

    public sealed class SucursalVistaInfo
    {
        private readonly int _idSucursal;
        private readonly string _nombre;
        private readonly string _direccion;
        private readonly string _telefono;
        private readonly int _idVendedor;
        private readonly string _nombreVendedor;
        private readonly string _identificacionVendedor;
        private readonly bool _activo;

        public int IdSucursal
        {
            get => _idSucursal;
        }

        public string Nombre
        {
            get => _nombre;
        }

        public string Direccion
        {
            get => _direccion;
        }

        public string Telefono
        {
            get => _telefono;
        }

        public int IdVendedor
        {
            get => _idVendedor;
        }

        public string NombreVendedor
        {
            get => _nombreVendedor;
        }

        public string IdentificacionVendedor
        {
            get => _identificacionVendedor;
        }

        public bool Activo
        {
            get => _activo;
        }

        public SucursalVistaInfo(
            int idSucursal,
            string nombre,
            string direccion,
            string telefono,
            int idVendedor,
            string nombreVendedor,
            string identificacionVendedor,
            bool activo)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            string nombreNormalizado = nombre?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                throw new ArgumentException("El nombre de la sucursal es obligatorio.", nameof(nombre));
            }

            string direccionNormalizada = direccion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(direccionNormalizada))
            {
                throw new ArgumentException("La dirección de la sucursal es obligatoria.", nameof(direccion));
            }

            string telefonoNormalizado = telefono?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(telefonoNormalizado))
            {
                throw new ArgumentException("El teléfono de la sucursal es obligatorio.", nameof(telefono));
            }

            if (idVendedor <= 0)
            {
                throw new ArgumentException("El id del vendedor debe ser mayor que cero.", nameof(idVendedor));
            }

            string nombreVendedorNormalizado = nombreVendedor?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreVendedorNormalizado))
            {
                throw new ArgumentException("El nombre del vendedor es obligatorio.", nameof(nombreVendedor));
            }

            string identificacionNormalizada = identificacionVendedor?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                throw new ArgumentException("La identificación del vendedor es obligatoria.", nameof(identificacionVendedor));
            }

            _idSucursal = idSucursal;
            _nombre = nombreNormalizado;
            _direccion = direccionNormalizada;
            _telefono = telefonoNormalizado;
            _idVendedor = idVendedor;
            _nombreVendedor = nombreVendedorNormalizado;
            _identificacionVendedor = identificacionNormalizada;
            _activo = activo;
        }

        public override string ToString()
        {
            return _idSucursal + " - " + _nombre;
        }
    }
}