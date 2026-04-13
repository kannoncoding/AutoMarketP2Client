/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase estática encargada de interpretar respuestas protocolarias del servidor TCP de AutoMarket, validar su estructura y extraer registros y valores tipados de manera segura.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace AutoMarket.Cliente.Comunicacion
{
    public static class InterpretadorRespuestas
    {
        public const int CantidadColumnasSucursal = 8;
        public const int CantidadColumnasVehiculoPorSucursal = 12;
        public const int CantidadColumnasVenta = 18;
        public const int CantidadColumnasCliente = 6;
        public const int CantidadColumnasVehiculo = 10;

        public static RespuestaServidor InterpretarRespuesta(string textoRespuesta)
        {
            return new RespuestaServidor(textoRespuesta);
        }

        public static RespuestaServidor InterpretarRespuestaExitosa(string textoRespuesta, string operacionEsperada)
        {
            RespuestaServidor respuesta = new RespuestaServidor(textoRespuesta);
            respuesta.ValidarExitoYOperacion(operacionEsperada);
            return respuesta;
        }

        public static bool EsConexionEstablecida(string textoRespuesta)
        {
            RespuestaServidor respuesta = new RespuestaServidor(textoRespuesta);

            return respuesta.EsExitosa
                && string.Equals(respuesta.Operacion, MensajesProtocolo.OperacionConexion, StringComparison.OrdinalIgnoreCase)
                && string.Equals(respuesta.Contenido, "ESTABLECIDA", StringComparison.OrdinalIgnoreCase);
        }

        public static bool EsPingExitoso(string textoRespuesta)
        {
            RespuestaServidor respuesta = new RespuestaServidor(textoRespuesta);

            return respuesta.EsExitosa
                && string.Equals(respuesta.Operacion, MensajesProtocolo.OperacionPing, StringComparison.OrdinalIgnoreCase)
                && string.Equals(respuesta.Contenido, "PONG", StringComparison.OrdinalIgnoreCase);
        }

        public static bool EsDesconexionAceptada(string textoRespuesta)
        {
            RespuestaServidor respuesta = new RespuestaServidor(textoRespuesta);

            return respuesta.EsExitosa
                && string.Equals(respuesta.Operacion, MensajesProtocolo.OperacionDesconexion, StringComparison.OrdinalIgnoreCase)
                && string.Equals(respuesta.Contenido, "ACEPTADA", StringComparison.OrdinalIgnoreCase);
        }

        public static int ObtenerIdClienteDesdeLogin(string textoRespuesta)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, MensajesProtocolo.OperacionLogin);

            if (respuesta.CantidadPartes < 4)
            {
                throw new InvalidOperationException("La respuesta de LOGIN no contiene la cantidad de campos esperada.");
            }

            return respuesta.ObtenerEnteroEnParte(2, "El id del cliente recibido en LOGIN no es válido.");
        }

        public static string ObtenerNombreClienteDesdeLogin(string textoRespuesta)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, MensajesProtocolo.OperacionLogin);

            if (respuesta.CantidadPartes < 4)
            {
                throw new InvalidOperationException("La respuesta de LOGIN no contiene la cantidad de campos esperada.");
            }

            return respuesta.ObtenerParteRequerida(3, "El nombre del cliente recibido en LOGIN es obligatorio.");
        }

        public static int ObtenerIdVentaRegistrada(string textoRespuesta)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, MensajesProtocolo.OperacionVenta);

            if (respuesta.CantidadPartes < 3)
            {
                throw new InvalidOperationException("La respuesta de VENTA no contiene el id de venta generado.");
            }

            return respuesta.ObtenerEnteroEnParte(2, "El id de venta recibido no es válido.");
        }

        public static List<string[]> ObtenerRegistrosSucursalesActivas(string textoRespuesta)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, MensajesProtocolo.OperacionSucursalesActivas);
            return ObtenerRegistrosConCantidadExacta(respuesta, CantidadColumnasSucursal, "SUCURSALES_ACTIVAS");
        }

        public static List<string[]> ObtenerRegistrosVehiculosPorSucursal(string textoRespuesta)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, MensajesProtocolo.OperacionVehiculosPorSucursal);
            return ObtenerRegistrosConCantidadExacta(respuesta, CantidadColumnasVehiculoPorSucursal, "VEHICULOS_POR_SUCURSAL");
        }

        public static List<string[]> ObtenerRegistrosVentasPorCliente(string textoRespuesta)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, MensajesProtocolo.OperacionVentasPorCliente);
            return ObtenerRegistrosConCantidadExacta(respuesta, CantidadColumnasVenta, "VENTAS_POR_CLIENTE");
        }

        public static string[] ObtenerRegistroClientePorId(string textoRespuesta)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, MensajesProtocolo.OperacionClientePorId);
            return ObtenerRegistroUnicoConCantidadExacta(respuesta, CantidadColumnasCliente, "CLIENTE_POR_ID");
        }

        public static string[] ObtenerRegistroVehiculoPorId(string textoRespuesta)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, MensajesProtocolo.OperacionVehiculoPorId);
            return ObtenerRegistroUnicoConCantidadExacta(respuesta, CantidadColumnasVehiculo, "VEHICULO_POR_ID");
        }

        public static List<string[]> ObtenerRegistrosDesdeContenido(string textoRespuesta, string operacionEsperada)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, operacionEsperada);
            return ObtenerRegistrosDesdeRespuesta(respuesta);
        }

        public static string[] ObtenerRegistroUnicoDesdeContenido(string textoRespuesta, string operacionEsperada)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, operacionEsperada);
            return ObtenerRegistroUnicoDesdeRespuesta(respuesta, operacionEsperada);
        }

        public static List<string[]> ObtenerRegistrosDesdeRespuesta(RespuestaServidor respuesta)
        {
            if (respuesta == null)
            {
                throw new ArgumentNullException(nameof(respuesta), "La respuesta del servidor es obligatoria.");
            }

            respuesta.ValidarExito();

            List<string> registrosTexto = respuesta.ObtenerRegistrosDesdeContenido();
            List<string[]> registros = new List<string[]>();

            for (int i = 0; i < registrosTexto.Count; i++)
            {
                string registroActual = registrosTexto[i];
                string[] columnas = respuesta.ObtenerColumnasDeRegistro(registroActual);

                if (columnas.Length > 0)
                {
                    registros.Add(columnas);
                }
            }

            return registros;
        }

        public static string[] ObtenerRegistroUnicoDesdeRespuesta(RespuestaServidor respuesta, string operacionEsperada)
        {
            if (respuesta == null)
            {
                throw new ArgumentNullException(nameof(respuesta), "La respuesta del servidor es obligatoria.");
            }

            if (string.IsNullOrWhiteSpace(operacionEsperada))
            {
                throw new ArgumentException("La operación esperada es obligatoria.", nameof(operacionEsperada));
            }

            respuesta.ValidarExitoYOperacion(operacionEsperada);

            string contenido = respuesta.ObtenerContenidoRequerido("La respuesta no contiene un registro válido.");
            string[] columnas = respuesta.ObtenerColumnasDeRegistro(contenido);

            if (columnas.Length == 0)
            {
                throw new InvalidOperationException("El registro recibido no contiene columnas válidas.");
            }

            return columnas;
        }

        public static int ObtenerEnteroColumna(string[] columnas, int indice, string mensajeError)
        {
            string valor = ObtenerColumnaRequerida(columnas, indice, mensajeError);

            if (!int.TryParse(valor, NumberStyles.Integer, CultureInfo.InvariantCulture, out int resultado))
            {
                throw new InvalidOperationException(mensajeError);
            }

            return resultado;
        }

        public static decimal ObtenerDecimalColumna(string[] columnas, int indice, string mensajeError)
        {
            string valor = ObtenerColumnaRequerida(columnas, indice, mensajeError);

            if (!decimal.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal resultado))
            {
                throw new InvalidOperationException(mensajeError);
            }

            return resultado;
        }

        public static DateTime ObtenerFechaColumna(string[] columnas, int indice, string mensajeError)
        {
            string valor = ObtenerColumnaRequerida(columnas, indice, mensajeError);

            if (!DateTime.TryParseExact(
                valor,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime resultado))
            {
                throw new InvalidOperationException(mensajeError);
            }

            return resultado;
        }

        public static bool ObtenerBooleanoBinarioColumna(string[] columnas, int indice, string mensajeError)
        {
            string valor = ObtenerColumnaRequerida(columnas, indice, mensajeError);

            if (valor == "1")
            {
                return true;
            }

            if (valor == "0")
            {
                return false;
            }

            throw new InvalidOperationException(mensajeError);
        }

        public static char ObtenerCaracterColumna(string[] columnas, int indice, string mensajeError)
        {
            string valor = ObtenerColumnaRequerida(columnas, indice, mensajeError);

            if (valor.Length != 1)
            {
                throw new InvalidOperationException(mensajeError);
            }

            return valor[0];
        }

        public static string ObtenerColumna(string[] columnas, int indice)
        {
            if (columnas == null)
            {
                throw new ArgumentNullException(nameof(columnas), "Las columnas del registro son obligatorias.");
            }

            if (indice < 0 || indice >= columnas.Length)
            {
                throw new IndexOutOfRangeException("El índice solicitado no existe dentro del registro recibido.");
            }

            return columnas[indice]?.Trim() ?? string.Empty;
        }

        public static string ObtenerColumnaRequerida(string[] columnas, int indice, string mensajeError)
        {
            string valor = ObtenerColumna(columnas, indice);

            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new InvalidOperationException(mensajeError);
            }

            return valor;
        }

        public static void ValidarCantidadColumnas(string[] columnas, int cantidadEsperada, string nombreOperacion)
        {
            if (columnas == null)
            {
                throw new ArgumentNullException(nameof(columnas), "Las columnas del registro son obligatorias.");
            }

            if (cantidadEsperada <= 0)
            {
                throw new ArgumentException("La cantidad esperada de columnas debe ser mayor que cero.", nameof(cantidadEsperada));
            }

            string operacionNormalizada = nombreOperacion?.Trim() ?? "OPERACION_DESCONOCIDA";

            if (columnas.Length != cantidadEsperada)
            {
                throw new InvalidOperationException(
                    "La operación '" + operacionNormalizada + "' devolvió un registro con una cantidad de columnas inválida. "
                    + "Cantidad esperada: " + cantidadEsperada.ToString(CultureInfo.InvariantCulture)
                    + ". Cantidad recibida: " + columnas.Length.ToString(CultureInfo.InvariantCulture) + ".");
            }
        }

        public static List<string[]> ObtenerRegistrosConCantidadExacta(RespuestaServidor respuesta, int cantidadColumnasEsperada, string nombreOperacion)
        {
            if (respuesta == null)
            {
                throw new ArgumentNullException(nameof(respuesta), "La respuesta del servidor es obligatoria.");
            }

            if (cantidadColumnasEsperada <= 0)
            {
                throw new ArgumentException("La cantidad esperada de columnas debe ser mayor que cero.", nameof(cantidadColumnasEsperada));
            }

            respuesta.ValidarExitoYOperacion(nombreOperacion);

            List<string[]> registros = ObtenerRegistrosDesdeRespuesta(respuesta);

            for (int i = 0; i < registros.Count; i++)
            {
                ValidarCantidadColumnas(registros[i], cantidadColumnasEsperada, nombreOperacion);
            }

            return registros;
        }

        public static string[] ObtenerRegistroUnicoConCantidadExacta(RespuestaServidor respuesta, int cantidadColumnasEsperada, string nombreOperacion)
        {
            if (respuesta == null)
            {
                throw new ArgumentNullException(nameof(respuesta), "La respuesta del servidor es obligatoria.");
            }

            if (cantidadColumnasEsperada <= 0)
            {
                throw new ArgumentException("La cantidad esperada de columnas debe ser mayor que cero.", nameof(cantidadColumnasEsperada));
            }

            string[] columnas = ObtenerRegistroUnicoDesdeRespuesta(respuesta, nombreOperacion);
            ValidarCantidadColumnas(columnas, cantidadColumnasEsperada, nombreOperacion);

            return columnas;
        }

        public static List<string[]> ObtenerRegistrosConCantidadExacta(string textoRespuesta, string operacionEsperada, int cantidadColumnasEsperada)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, operacionEsperada);
            return ObtenerRegistrosConCantidadExacta(respuesta, cantidadColumnasEsperada, operacionEsperada);
        }

        public static string[] ObtenerRegistroUnicoConCantidadExacta(string textoRespuesta, string operacionEsperada, int cantidadColumnasEsperada)
        {
            RespuestaServidor respuesta = InterpretarRespuestaExitosa(textoRespuesta, operacionEsperada);
            return ObtenerRegistroUnicoConCantidadExacta(respuesta, cantidadColumnasEsperada, operacionEsperada);
        }

        public static string ObtenerMensajeError(string textoRespuesta)
        {
            RespuestaServidor respuesta = new RespuestaServidor(textoRespuesta);

            if (!respuesta.EsError)
            {
                throw new InvalidOperationException("La respuesta recibida no corresponde a un error del servidor.");
            }

            return respuesta.MensajeError;
        }

        public static string ObtenerOperacionRespuesta(string textoRespuesta)
        {
            RespuestaServidor respuesta = new RespuestaServidor(textoRespuesta);
            return respuesta.Operacion;
        }

        public static bool EsRespuestaError(string textoRespuesta)
        {
            RespuestaServidor respuesta = new RespuestaServidor(textoRespuesta);
            return respuesta.EsError;
        }

        public static bool EsRespuestaExitosa(string textoRespuesta)
        {
            RespuestaServidor respuesta = new RespuestaServidor(textoRespuesta);
            return respuesta.EsExitosa;
        }
    }
}