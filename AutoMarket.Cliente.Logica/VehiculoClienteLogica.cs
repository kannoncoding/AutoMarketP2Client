/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase de lógica encargada de consultar vehículos desde el servidor TCP, interpretar sus respuestas protocolarias y convertirlas en objetos de entidad del sistema AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Collections.Generic;
using AutoMarket.Cliente.Comunicacion;
using AutoMarket.Entidades;

namespace AutoMarket.Cliente.Logica
{
    public sealed class VehiculoClienteLogica
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

        public VehiculoClienteLogica(ClienteTcp clienteTcp, SesionCliente sesionCliente)
        {
            _clienteTcp = clienteTcp ?? throw new ArgumentNullException(nameof(clienteTcp), "La instancia de ClienteTcp es obligatoria.");
            _sesionCliente = sesionCliente ?? throw new ArgumentNullException(nameof(sesionCliente), "La instancia de SesionCliente es obligatoria.");
        }

        public List<VehiculoDisponibleInfo> ObtenerVehiculosDisponiblesPorSucursal(int idSucursal)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            ValidarConexionCliente();

            string respuestaTexto = _clienteTcp.ObtenerVehiculosPorSucursal(idSucursal);
            RespuestaServidor respuesta = InterpretadorRespuestas.InterpretarRespuesta(respuestaTexto);

            if (respuesta.EsError)
            {
                throw new InvalidOperationException(respuesta.MensajeError);
            }

            List<string[]> registros = InterpretadorRespuestas.ObtenerRegistrosVehiculosPorSucursal(respuestaTexto);
            List<VehiculoDisponibleInfo> vehiculos = new List<VehiculoDisponibleInfo>();

            for (int i = 0; i < registros.Count; i++)
            {
                VehiculoDisponibleInfo vehiculoDisponible = ConstruirVehiculoDisponibleDesdeColumnas(registros[i]);

                if (vehiculoDisponible.CantidadDisponible > 0)
                {
                    vehiculos.Add(vehiculoDisponible);
                }
            }

            return vehiculos;
        }

        public Vehiculo ObtenerVehiculoPorId(int idVehiculo)
        {
            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.", nameof(idVehiculo));
            }

            ValidarConexionCliente();

            string respuestaTexto = _clienteTcp.ObtenerVehiculoPorId(idVehiculo);
            RespuestaServidor respuesta = InterpretadorRespuestas.InterpretarRespuesta(respuestaTexto);

            if (respuesta.EsError)
            {
                throw new InvalidOperationException(respuesta.MensajeError);
            }

            string[] columnas = InterpretadorRespuestas.ObtenerRegistroVehiculoPorId(respuestaTexto);
            return ConstruirVehiculoDesdeColumnas(columnas);
        }

        public VehiculoDisponibleInfo ObtenerVehiculoDisponiblePorIdEnSucursal(int idSucursal, int idVehiculo)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.", nameof(idVehiculo));
            }

            List<VehiculoDisponibleInfo> vehiculos = ObtenerVehiculosDisponiblesPorSucursal(idSucursal);

            for (int i = 0; i < vehiculos.Count; i++)
            {
                if (vehiculos[i].Vehiculo.IdVehiculo == idVehiculo)
                {
                    return vehiculos[i];
                }
            }

            throw new InvalidOperationException("No existe un vehículo disponible con el id indicado en la sucursal seleccionada.");
        }

        public bool ExisteVehiculoDisponibleEnSucursal(int idSucursal, int idVehiculo)
        {
            if (idSucursal <= 0 || idVehiculo <= 0)
            {
                return false;
            }

            List<VehiculoDisponibleInfo> vehiculos = ObtenerVehiculosDisponiblesPorSucursal(idSucursal);

            for (int i = 0; i < vehiculos.Count; i++)
            {
                if (vehiculos[i].Vehiculo.IdVehiculo == idVehiculo)
                {
                    return true;
                }
            }

            return false;
        }

        public List<VehiculoDisponibleVistaInfo> ObtenerVehiculosDisponiblesPorSucursalParaVista(int idSucursal)
        {
            List<VehiculoDisponibleInfo> vehiculos = ObtenerVehiculosDisponiblesPorSucursal(idSucursal);
            List<VehiculoDisponibleVistaInfo> resultado = new List<VehiculoDisponibleVistaInfo>();

            for (int i = 0; i < vehiculos.Count; i++)
            {
                VehiculoDisponibleInfo vehiculoDisponible = vehiculos[i];
                Vehiculo vehiculo = vehiculoDisponible.Vehiculo;

                resultado.Add(new VehiculoDisponibleVistaInfo(
                    vehiculoDisponible.IdSucursal,
                    vehiculo.IdVehiculo,
                    vehiculo.Marca,
                    vehiculo.Modelo,
                    vehiculo.Ano,
                    vehiculo.Precio,
                    vehiculo.Estado,
                    vehiculo.EstadoDescripcion,
                    vehiculo.Categoria.IdCategoria,
                    vehiculo.Categoria.NombreCategoria,
                    vehiculo.Categoria.Descripcion,
                    vehiculoDisponible.CantidadDisponible));
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

        private VehiculoDisponibleInfo ConstruirVehiculoDisponibleDesdeColumnas(string[] columnas)
        {
            if (columnas == null)
            {
                throw new ArgumentNullException(nameof(columnas), "Las columnas del vehículo por sucursal son obligatorias.");
            }

            InterpretadorRespuestas.ValidarCantidadColumnas(
                columnas,
                InterpretadorRespuestas.CantidadColumnasVehiculoPorSucursal,
                MensajesProtocolo.OperacionVehiculosPorSucursal);

            int idSucursal = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                0,
                "El id de la sucursal recibido no es válido.");

            Vehiculo vehiculo = ConstruirVehiculoDesdeColumnasInventario(columnas);

            int cantidadDisponible = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                11,
                "La cantidad disponible del vehículo no es válida.");

            return new VehiculoDisponibleInfo(
                idSucursal,
                vehiculo,
                cantidadDisponible);
        }

        private Vehiculo ConstruirVehiculoDesdeColumnasInventario(string[] columnas)
        {
            if (columnas == null)
            {
                throw new ArgumentNullException(nameof(columnas), "Las columnas del vehículo son obligatorias.");
            }

            if (columnas.Length < 11)
            {
                throw new InvalidOperationException("El registro recibido del inventario no contiene las columnas mínimas requeridas para construir el vehículo.");
            }

            int idVehiculo = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                1,
                "El id del vehículo recibido no es válido.");

            string marca = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                2,
                "La marca del vehículo es obligatoria.");

            string modelo = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                3,
                "El modelo del vehículo es obligatorio.");

            int ano = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                4,
                "El año del vehículo no es válido.");

            decimal precio = InterpretadorRespuestas.ObtenerDecimalColumna(
                columnas,
                5,
                "El precio del vehículo no es válido.");

            char estado = InterpretadorRespuestas.ObtenerCaracterColumna(
                columnas,
                6,
                "El estado del vehículo no es válido.");

            int idCategoria = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                8,
                "El id de la categoría del vehículo no es válido.");

            string nombreCategoria = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                9,
                "El nombre de la categoría del vehículo es obligatorio.");

            string descripcionCategoria = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                10,
                "La descripción de la categoría del vehículo es obligatoria.");

            CategoriaVehiculo categoria = new CategoriaVehiculo(
                idCategoria,
                nombreCategoria,
                descripcionCategoria);

            return new Vehiculo(
                idVehiculo,
                marca,
                modelo,
                ano,
                precio,
                categoria,
                estado);
        }

        private Vehiculo ConstruirVehiculoDesdeColumnas(string[] columnas)
        {
            if (columnas == null)
            {
                throw new ArgumentNullException(nameof(columnas), "Las columnas del vehículo son obligatorias.");
            }

            InterpretadorRespuestas.ValidarCantidadColumnas(
                columnas,
                InterpretadorRespuestas.CantidadColumnasVehiculo,
                MensajesProtocolo.OperacionVehiculoPorId);

            int idVehiculo = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                0,
                "El id del vehículo recibido no es válido.");

            string marca = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                1,
                "La marca del vehículo es obligatoria.");

            string modelo = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                2,
                "El modelo del vehículo es obligatorio.");

            int ano = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                3,
                "El año del vehículo no es válido.");

            decimal precio = InterpretadorRespuestas.ObtenerDecimalColumna(
                columnas,
                4,
                "El precio del vehículo no es válido.");

            char estado = InterpretadorRespuestas.ObtenerCaracterColumna(
                columnas,
                5,
                "El estado del vehículo no es válido.");

            int idCategoria = InterpretadorRespuestas.ObtenerEnteroColumna(
                columnas,
                7,
                "El id de la categoría del vehículo no es válido.");

            string nombreCategoria = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                8,
                "El nombre de la categoría del vehículo es obligatorio.");

            string descripcionCategoria = InterpretadorRespuestas.ObtenerColumnaRequerida(
                columnas,
                9,
                "La descripción de la categoría del vehículo es obligatoria.");

            CategoriaVehiculo categoria = new CategoriaVehiculo(
                idCategoria,
                nombreCategoria,
                descripcionCategoria);

            return new Vehiculo(
                idVehiculo,
                marca,
                modelo,
                ano,
                precio,
                categoria,
                estado);
        }
    }

    public sealed class VehiculoDisponibleInfo
    {
        private readonly int _idSucursal;
        private readonly Vehiculo _vehiculo;
        private readonly int _cantidadDisponible;

        public int IdSucursal
        {
            get => _idSucursal;
        }

        public Vehiculo Vehiculo
        {
            get => _vehiculo;
        }

        public int CantidadDisponible
        {
            get => _cantidadDisponible;
        }

        public VehiculoDisponibleInfo(int idSucursal, Vehiculo vehiculo, int cantidadDisponible)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            _vehiculo = vehiculo ?? throw new ArgumentNullException(nameof(vehiculo), "El vehículo es obligatorio.");

            if (cantidadDisponible < 0)
            {
                throw new ArgumentException("La cantidad disponible no puede ser negativa.", nameof(cantidadDisponible));
            }

            _idSucursal = idSucursal;
            _cantidadDisponible = cantidadDisponible;
        }

        public override string ToString()
        {
            return _vehiculo.IdVehiculo + " - " + _vehiculo.Marca + " - " + _vehiculo.Modelo;
        }
    }

    public sealed class VehiculoDisponibleVistaInfo
    {
        private readonly int _idSucursal;
        private readonly int _idVehiculo;
        private readonly string _marca;
        private readonly string _modelo;
        private readonly int _ano;
        private readonly decimal _precio;
        private readonly char _estado;
        private readonly string _estadoDescripcion;
        private readonly int _idCategoria;
        private readonly string _nombreCategoria;
        private readonly string _descripcionCategoria;
        private readonly int _cantidadDisponible;

        public int IdSucursal
        {
            get => _idSucursal;
        }

        public int IdVehiculo
        {
            get => _idVehiculo;
        }

        public string Marca
        {
            get => _marca;
        }

        public string Modelo
        {
            get => _modelo;
        }

        public int Ano
        {
            get => _ano;
        }

        public decimal Precio
        {
            get => _precio;
        }

        public char Estado
        {
            get => _estado;
        }

        public string EstadoDescripcion
        {
            get => _estadoDescripcion;
        }

        public int IdCategoria
        {
            get => _idCategoria;
        }

        public string NombreCategoria
        {
            get => _nombreCategoria;
        }

        public string DescripcionCategoria
        {
            get => _descripcionCategoria;
        }

        public int CantidadDisponible
        {
            get => _cantidadDisponible;
        }

        public VehiculoDisponibleVistaInfo(
            int idSucursal,
            int idVehiculo,
            string marca,
            string modelo,
            int ano,
            decimal precio,
            char estado,
            string estadoDescripcion,
            int idCategoria,
            string nombreCategoria,
            string descripcionCategoria,
            int cantidadDisponible)
        {
            if (idSucursal <= 0)
            {
                throw new ArgumentException("El id de la sucursal debe ser mayor que cero.", nameof(idSucursal));
            }

            if (idVehiculo <= 0)
            {
                throw new ArgumentException("El id del vehículo debe ser mayor que cero.", nameof(idVehiculo));
            }

            string marcaNormalizada = marca?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(marcaNormalizada))
            {
                throw new ArgumentException("La marca del vehículo es obligatoria.", nameof(marca));
            }

            string modeloNormalizado = modelo?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(modeloNormalizado))
            {
                throw new ArgumentException("El modelo del vehículo es obligatorio.", nameof(modelo));
            }

            if (ano < 1900)
            {
                throw new ArgumentException("El año del vehículo no es válido.", nameof(ano));
            }

            if (precio <= 0)
            {
                throw new ArgumentException("El precio del vehículo debe ser mayor que cero.", nameof(precio));
            }

            char estadoNormalizado = char.ToUpperInvariant(estado);
            if (estadoNormalizado != 'N' && estadoNormalizado != 'U')
            {
                throw new ArgumentException("El estado del vehículo no es válido.", nameof(estado));
            }

            string estadoDescripcionNormalizada = estadoDescripcion?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(estadoDescripcionNormalizada))
            {
                throw new ArgumentException("La descripción del estado del vehículo es obligatoria.", nameof(estadoDescripcion));
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

            if (cantidadDisponible < 0)
            {
                throw new ArgumentException("La cantidad disponible no puede ser negativa.", nameof(cantidadDisponible));
            }

            _idSucursal = idSucursal;
            _idVehiculo = idVehiculo;
            _marca = marcaNormalizada;
            _modelo = modeloNormalizado;
            _ano = ano;
            _precio = precio;
            _estado = estadoNormalizado;
            _estadoDescripcion = estadoDescripcionNormalizada;
            _idCategoria = idCategoria;
            _nombreCategoria = nombreCategoriaNormalizado;
            _descripcionCategoria = descripcionCategoriaNormalizada;
            _cantidadDisponible = cantidadDisponible;
        }

        public override string ToString()
        {
            return _idVehiculo + " - " + _marca + " - " + _modelo;
        }
    }
}