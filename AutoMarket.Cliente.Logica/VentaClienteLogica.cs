/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica encargada de registrar ventas y consultar las ventas del cliente autenticado mediante comunicación TCP con el servidor, interpretando las respuestas protocolarias y reconstruyendo las entidades del sistema AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Collections.Generic;
using AutoMarket.Cliente.Comunicacion;
using ClienteEntidad = AutoMarket.Entidades.Cliente;
using VentaEntidad = AutoMarket.Entidades.Venta;
using SucursalEntidad = AutoMarket.Entidades.Sucursal;
using VendedorEntidad = AutoMarket.Entidades.Vendedor;
using VehiculoEntidad = AutoMarket.Entidades.Vehiculo;
using CategoriaVehiculoEntidad = AutoMarket.Entidades.CategoriaVehiculo;

namespace AutoMarket.Cliente.Logica
{
    public sealed class VentaClienteLogica
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

        public VentaClienteLogica(ClienteTcp clienteTcp, SesionCliente sesionCliente)
        {
            _clienteTcp = clienteTcp ?? throw new ArgumentNullException(nameof(clienteTcp), "La instancia de ClienteTcp es obligatoria.");
            _sesionCliente = sesionCliente ?? throw new ArgumentNullException(nameof(sesionCliente), "La instancia de SesionCliente es obligatoria.");
        }

        public ResultadoRegistroVenta RegistrarVenta(int idSucursal, int idVehiculo)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.", nameof(idVehiculo));
            }

            ValidarSesionAutenticada();

            int idCliente = _sesionCliente.ObtenerIdClienteRequerido();

            string respuestaTexto = _clienteTcp.RegistrarVenta(idCliente, idSucursal, idVehiculo);
            RespuestaServidor respuesta = InterpretadorRespuestas.InterpretarRespuesta(respuestaTexto);

            if (respuesta.EsError)
            {
                return new ResultadoRegistroVenta(
                    false,
                    0,
                    respuesta.MensajeError,
                    respuesta.Operacion);
            }

            int idVentaGenerada = InterpretadorRespuestas.ObtenerIdVentaRegistrada(respuestaTexto);

            return new ResultadoRegistroVenta(
                true,
                idVentaGenerada,
                "La venta fue registrada correctamente.",
                respuesta.Operacion);
        }

        public List<VentaEntidad> ObtenerVentasDelClienteAutenticado()
        {
            ValidarSesionAutenticada();

            int idCliente = _sesionCliente.ObtenerIdClienteRequerido();
            return ObtenerVentasPorCliente(idCliente);
        }

        public List<VentaEntidad> ObtenerVentasPorCliente(int idCliente)
        {
            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.", nameof(idCliente));
            }

            ValidarConexionCliente();

            string respuestaTexto = _clienteTcp.ObtenerVentasPorCliente(idCliente);
            RespuestaServidor respuesta = InterpretadorRespuestas.InterpretarRespuesta(respuestaTexto);

            if (respuesta.EsError)
            {
                throw new InvalidOperationException(respuesta.MensajeError);
            }

            List<string[]> registros = InterpretadorRespuestas.ObtenerRegistrosVentasPorCliente(respuestaTexto);
            List<VentaEntidad> ventas = new List<VentaEntidad>();

            for (int i = 0; i < registros.Count; i++)
            {
                ventas.Add(ConstruirVentaDesdeColumnas(registros[i]));
            }

            return ventas;
        }

        public List<VentaVistaInfo> ObtenerVentasDelClienteAutenticadoParaVista()
        {
            List<VentaEntidad> ventas = ObtenerVentasDelClienteAutenticado();
            return ConvertirVentasAVista(ventas);
        }

        public List<VentaVistaInfo> ObtenerVentasPorClienteParaVista(int idCliente)
        {
            List<VentaEntidad> ventas = ObtenerVentasPorCliente(idCliente);
            return ConvertirVentasAVista(ventas);
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

        private void ValidarSesionAutenticada()
        {
            _sesionCliente.ValidarClienteAutenticado();

            if (!_clienteTcp.EstaConectado)
            {
                _sesionCliente.CerrarSesion();
                throw new InvalidOperationException("No existe una conexión TCP activa con el servidor.");
            }
        }

        private List<VentaVistaInfo> ConvertirVentasAVista(List<VentaEntidad> ventas)
        {
            if (ventas == null)
            {
                throw new ArgumentNullException(nameof(ventas), "La lista de ventas es obligatoria.");
            }

            List<VentaVistaInfo> resultado = new List<VentaVistaInfo>();

            for (int i = 0; i < ventas.Count; i++)
            {
                VentaEntidad venta = ventas[i];

                resultado.Add(new VentaVistaInfo(
                    venta.IdVenta,
                    venta.Cliente.IdCliente,
                    venta.Cliente.NombreCompleto,
                    venta.Cliente.Identificacion,
                    venta.Sucursal.IdSucursal,
                    venta.Sucursal.Nombre,
                    venta.Vehiculo.IdVehiculo,
                    venta.Vehiculo.Marca,
                    venta.Vehiculo.Modelo,
                    venta.Vehiculo.Ano,
                    venta.Vehiculo.Precio,
                    venta.Vehiculo.Estado,
                    venta.Vehiculo.EstadoDescripcion,
                    venta.Vehiculo.Categoria.IdCategoria,
                    venta.Vehiculo.Categoria.NombreCategoria,
                    venta.Vehiculo.Categoria.Descripcion,
                    venta.FechaVenta,
                    venta.Monto));
            }

            return resultado;
        }

        private VentaEntidad ConstruirVentaDesdeColumnas(string[] columnas)
        {
            if (columnas == null)
            {
                throw new ArgumentNullException(nameof(columnas), "Las columnas de la venta son obligatorias.");
            }

            InterpretadorRespuestas.ValidarCantidadColumnas(
                columnas,
                InterpretadorRespuestas.CantidadColumnasVenta,
                MensajesProtocolo.OperacionVentasPorCliente);

            int idVenta = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                0,
                "El id de la venta no es válido.");

            int idCliente = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                1,
                "El id del cliente no es válido.");

            string nombreCliente = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                2,
                "El nombre del cliente es obligatorio.");

            string identificacionCliente = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                3,
                "La identificación del cliente es obligatoria.");

            int idSucursal = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                4,
                "El id de la sucursal no es válido.");

            string nombreSucursal = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                5,
                "El nombre de la sucursal es obligatorio.");

            int idVehiculo = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                6,
                "El id del vehículo no es válido.");

            string marcaVehiculo = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                7,
                "La marca del vehículo es obligatoria.");

            string modeloVehiculo = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                8,
                "El modelo del vehículo es obligatorio.");

            int anoVehiculo = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                9,
                "El año del vehículo no es válido.");

            decimal precioVehiculo = InterpretadorRespuestas.ObtenerDecimalColumna(
                columnas,
                10,
                "El precio del vehículo no es válido.");

            char estadoVehiculo = InterpretadorRespuestas.ObtenerCaracterColumna(
                columnas,
                11,
                "El estado del vehículo no es válido.");

            int idCategoria = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                13,
                "El id de la categoría del vehículo no es válido.");

            string nombreCategoria = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                14,
                "El nombre de la categoría del vehículo es obligatorio.");

            string descripcionCategoria = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                15,
                "La descripción de la categoría del vehículo es obligatoria.");

            DateTime fechaVenta = InterpretadorRespuestas.ObtenerFechaColumna(
                columnas,
                16,
                "La fecha de la venta no es válida.");

            decimal montoVenta = InterpretadorRespuestas.ObtenerDecimalColumna(
                columnas,
                17,
                "El monto de la venta no es válido.");

            ClienteEntidad cliente = ConstruirClienteDesdeDatosMinimos(
                idCliente,
                identificacionCliente,
                nombreCliente);

            SucursalEntidad sucursal = ConstruirSucursalDesdeDatosMinimos(
                idSucursal,
                nombreSucursal);

            CategoriaVehiculoEntidad categoria = new CategoriaVehiculoEntidad(
                idCategoria,
                nombreCategoria,
                descripcionCategoria);

            VehiculoEntidad vehiculo = new VehiculoEntidad(
                idVehiculo,
                marcaVehiculo,
                modeloVehiculo,
                anoVehiculo,
                precioVehiculo,
                categoria,
                estadoVehiculo);

            return new VentaEntidad(
                idVenta,
                cliente,
                sucursal,
                vehiculo,
                fechaVenta,
                montoVenta);
        }

        private ClienteEntidad ConstruirClienteDesdeDatosMinimos(int idCliente, string identificacion, string nombreCompleto)
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

            DateTime fechaNacimientoTecnica = new DateTime(1900, 1, 1);
            DateTime fechaRegistroTecnica = new DateTime(2000, 1, 1);

            return new ClienteEntidad(
                idCliente,
                identificacionNormalizada,
                nombreNormalizado,
                fechaNacimientoTecnica,
                fechaRegistroTecnica,
                true);
        }

        private SucursalEntidad ConstruirSucursalDesdeDatosMinimos(int idSucursal, string nombreSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            string nombreNormalizado = nombreSucursal?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreNormalizado))
            {
                throw new ArgumentException("El nombre de la sucursal es obligatorio.", nameof(nombreSucursal));
            }

            VendedorEntidad vendedorTecnico = new VendedorEntidad(
                1,
                "NO DISPONIBLE",
                "NO DISPONIBLE",
                new DateTime(1900, 1, 1),
                new DateTime(2000, 1, 1),
                "NO DISPONIBLE");

            return new SucursalEntidad(
                idSucursal,
                nombreNormalizado,
                "NO DISPONIBLE",
                "NO DISPONIBLE",
                vendedorTecnico,
                true);
        }
    }

    public sealed class ResultadoRegistroVenta
    {
        private readonly bool _registroExitoso;
        private readonly int _idVentaGenerada;
        private readonly string _mensaje;
        private readonly string _operacion;

        public bool RegistroExitoso
        {
            get => _registroExitoso;
        }

        public int IdVentaGenerada
        {
            get => _idVentaGenerada;
        }

        public string Mensaje
        {
            get => _mensaje;
        }

        public string Operacion
        {
            get => _operacion;
        }

        public ResultadoRegistroVenta(bool registroExitoso, int idVentaGenerada, string mensaje, string operacion)
        {
            _registroExitoso = registroExitoso;
            _idVentaGenerada = idVentaGenerada;
            _mensaje = mensaje?.Trim() ?? string.Empty;
            _operacion = operacion?.Trim() ?? string.Empty;
        }
    }

    public sealed class VentaVistaInfo
    {
        private readonly int _idVenta;
        private readonly int _idCliente;
        private readonly string _nombreCliente;
        private readonly string _identificacionCliente;
        private readonly int _idSucursal;
        private readonly string _nombreSucursal;
        private readonly int _idVehiculo;
        private readonly string _marcaVehiculo;
        private readonly string _modeloVehiculo;
        private readonly int _anoVehiculo;
        private readonly decimal _precioVehiculo;
        private readonly char _estadoVehiculo;
        private readonly string _estadoDescripcionVehiculo;
        private readonly int _idCategoria;
        private readonly string _nombreCategoria;
        private readonly string _descripcionCategoria;
        private readonly DateTime _fechaVenta;
        private readonly decimal _montoVenta;

        public int IdVenta => _idVenta;
        public int IdCliente => _idCliente;
        public string NombreCliente => _nombreCliente;
        public string IdentificacionCliente => _identificacionCliente;
        public int IdSucursal => _idSucursal;
        public string NombreSucursal => _nombreSucursal;
        public int IdVehiculo => _idVehiculo;
        public string MarcaVehiculo => _marcaVehiculo;
        public string ModeloVehiculo => _modeloVehiculo;
        public int AnoVehiculo => _anoVehiculo;
        public decimal PrecioVehiculo => _precioVehiculo;
        public char EstadoVehiculo => _estadoVehiculo;
        public string EstadoDescripcionVehiculo => _estadoDescripcionVehiculo;
        public int IdCategoria => _idCategoria;
        public string NombreCategoria => _nombreCategoria;
        public string DescripcionCategoria => _descripcionCategoria;
        public DateTime FechaVenta => _fechaVenta;
        public decimal MontoVenta => _montoVenta;

        public VentaVistaInfo(
            int idVenta,
            int idCliente,
            string nombreCliente,
            string identificacionCliente,
            int idSucursal,
            string nombreSucursal,
            int idVehiculo,
            string marcaVehiculo,
            string modeloVehiculo,
            int anoVehiculo,
            decimal precioVehiculo,
            char estadoVehiculo,
            string estadoDescripcionVehiculo,
            int idCategoria,
            string nombreCategoria,
            string descripcionCategoria,
            DateTime fechaVenta,
            decimal montoVenta)
        {
            if (idVenta < 0)
            {
                throw new ArgumentException("El id de la venta no puede ser negativo.", nameof(idVenta));
            }

            if (idCliente <= 0)
            {
                throw new ArgumentException("El id del cliente debe ser mayor que cero.", nameof(idCliente));
            }

            string nombreClienteNormalizado = nombreCliente?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreClienteNormalizado))
            {
                throw new ArgumentException("El nombre del cliente es obligatorio.", nameof(nombreCliente));
            }

            string identificacionClienteNormalizada = identificacionCliente?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(identificacionClienteNormalizada))
            {
                throw new ArgumentException("La identificación del cliente es obligatoria.", nameof(identificacionCliente));
            }

            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            string nombreSucursalNormalizado = nombreSucursal?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreSucursalNormalizado))
            {
                throw new ArgumentException("El nombre de la sucursal es obligatorio.", nameof(nombreSucursal));
            }

            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.", nameof(idVehiculo));
            }

            string marcaVehiculoNormalizada = marcaVehiculo?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(marcaVehiculoNormalizada))
            {
                throw new ArgumentException("La marca del vehículo es obligatoria.", nameof(marcaVehiculo));
            }

            string modeloVehiculoNormalizado = modeloVehiculo?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(modeloVehiculoNormalizado))
            {
                throw new ArgumentException("El modelo del vehículo es obligatorio.", nameof(modeloVehiculo));
            }

            if (anoVehiculo < 1900)
            {
                throw new ArgumentException("El año del vehículo no es válido.", nameof(anoVehiculo));
            }

            if (precioVehiculo <= 0)
            {
                throw new ArgumentException("El precio del vehículo debe ser mayor que cero.", nameof(precioVehiculo));
            }

            char estadoNormalizado = char.ToUpperInvariant(estadoVehiculo);
            if (estadoNormalizado != 'N' && estadoNormalizado != 'U')
            {
                throw new ArgumentException("El estado del vehículo no es válido.", nameof(estadoVehiculo));
            }

            string estadoDescripcionNormalizada = estadoDescripcionVehiculo?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(estadoDescripcionNormalizada))
            {
                throw new ArgumentException("La descripción del estado del vehículo es obligatoria.", nameof(estadoDescripcionVehiculo));
            }

            if (idCategoria <= 0)
            {
                throw new ArgumentException("El id de la categoría debe ser mayor que cero.", nameof(idCategoria));
            }

            string nombreCategoriaNormalizado = nombreCategoria?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(nombreCategoriaNormalizado))
            {
                throw new ArgumentException("El nombre de la categoría es obligatorio.", nameof(nombreCategoria));
            }

            string descripcionCategoriaNormalizada = descripcionCategoria?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(descripcionCategoriaNormalizada))
            {
                throw new ArgumentException("La descripción de la categoría es obligatoria.", nameof(descripcionCategoria));
            }

            if (fechaVenta == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de la venta es obligatoria.", nameof(fechaVenta));
            }

            if (montoVenta <= 0)
            {
                throw new ArgumentException("El monto de la venta debe ser mayor que cero.", nameof(montoVenta));
            }

            _idVenta = idVenta;
            _idCliente = idCliente;
            _nombreCliente = nombreClienteNormalizado;
            _identificacionCliente = identificacionClienteNormalizada;
            _idSucursal = idSucursal;
            _nombreSucursal = nombreSucursalNormalizado;
            _idVehiculo = idVehiculo;
            _marcaVehiculo = marcaVehiculoNormalizada;
            _modeloVehiculo = modeloVehiculoNormalizado;
            _anoVehiculo = anoVehiculo;
            _precioVehiculo = precioVehiculo;
            _estadoVehiculo = estadoNormalizado;
            _estadoDescripcionVehiculo = estadoDescripcionNormalizada;
            _idCategoria = idCategoria;
            _nombreCategoria = nombreCategoriaNormalizado;
            _descripcionCategoria = descripcionCategoriaNormalizada;
            _fechaVenta = fechaVenta;
            _montoVenta = montoVenta;
        }

        public override string ToString()
        {
            string textoIdVenta = _idVenta > 0 ? _idVenta.ToString() : "Nueva venta";
            return textoIdVenta + " - " + _nombreCliente + " - " + _marcaVehiculo + " " + _modeloVehiculo;
        }
    }
}