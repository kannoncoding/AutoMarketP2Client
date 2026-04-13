/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase estática encargada de centralizar las operaciones y la construcción de solicitudes del protocolo TCP utilizado entre el cliente y el servidor de AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Globalization;

namespace AutoMarket.Cliente.Comunicacion
{
    public static class MensajesProtocolo
    {
        public const char DelimitadorCamposPrincipales = '|';
        public const char DelimitadorRegistros = ';';
        public const char DelimitadorColumnasInternas = ',';

        public const string EstadoOk = "OK";
        public const string EstadoError = "ERROR";

        public const string OperacionPing = "PING";
        public const string OperacionLogin = "LOGIN";
        public const string OperacionSucursalesActivas = "SUCURSALES_ACTIVAS";
        public const string OperacionVehiculosPorSucursal = "VEHICULOS_POR_SUCURSAL";
        public const string OperacionVenta = "VENTA";
        public const string OperacionVentasPorCliente = "VENTAS_POR_CLIENTE";
        public const string OperacionClientePorId = "CLIENTE_POR_ID";
        public const string OperacionVehiculoPorId = "VEHICULO_POR_ID";
        public const string OperacionConexion = "CONEXION";
        public const string OperacionDesconexion = "DESCONEXION";
        public const string OperacionLogout = "LOGOUT";

        public const string RespuestaPingEsperada = "OK|PING|PONG";
        public const string RespuestaConexionEsperada = "OK|CONEXION|ESTABLECIDA";
        public const string RespuestaDesconexionEsperada = "OK|DESCONEXION|ACEPTADA";

        public static string CrearPing()
        {
            return OperacionPing;
        }

        public static string CrearLogin(int idCliente)
        {
            return UnirCampos(
                OperacionLogin,
                ConvertirEnteroRequerido(idCliente, nameof(idCliente)));
        }

        public static string CrearSolicitudSucursalesActivas()
        {
            return OperacionSucursalesActivas;
        }

        public static string CrearSolicitudVehiculosPorSucursal(int idSucursal)
        {
            return UnirCampos(
                OperacionVehiculosPorSucursal,
                ConvertirEnteroRequerido(idSucursal, nameof(idSucursal)));
        }

        public static string CrearSolicitudVenta(int idCliente, int idSucursal, int idVehiculo)
        {
            return UnirCampos(
                OperacionVenta,
                ConvertirEnteroRequerido(idCliente, nameof(idCliente)),
                ConvertirEnteroRequerido(idSucursal, nameof(idSucursal)),
                ConvertirEnteroRequerido(idVehiculo, nameof(idVehiculo)));
        }

        public static string CrearSolicitudVentasPorCliente(int idCliente)
        {
            return UnirCampos(
                OperacionVentasPorCliente,
                ConvertirEnteroRequerido(idCliente, nameof(idCliente)));
        }

        public static string CrearSolicitudClientePorId(int idCliente)
        {
            return UnirCampos(
                OperacionClientePorId,
                ConvertirEnteroRequerido(idCliente, nameof(idCliente)));
        }

        public static string CrearSolicitudVehiculoPorId(int idVehiculo)
        {
            return UnirCampos(
                OperacionVehiculoPorId,
                ConvertirEnteroRequerido(idVehiculo, nameof(idVehiculo)));
        }

        public static string CrearLogout()
        {
            return OperacionLogout;
        }

        public static bool EsRespuestaOk(string estado)
        {
            string estadoNormalizado = NormalizarTextoBasico(estado).ToUpperInvariant();
            return estadoNormalizado == EstadoOk;
        }

        public static bool EsRespuestaError(string estado)
        {
            string estadoNormalizado = NormalizarTextoBasico(estado).ToUpperInvariant();
            return estadoNormalizado == EstadoError;
        }

        public static bool EsOperacionReconocida(string operacion)
        {
            string operacionNormalizada = NormalizarTextoBasico(operacion).ToUpperInvariant();

            return operacionNormalizada == OperacionPing
                || operacionNormalizada == OperacionLogin
                || operacionNormalizada == OperacionSucursalesActivas
                || operacionNormalizada == OperacionVehiculosPorSucursal
                || operacionNormalizada == OperacionVenta
                || operacionNormalizada == OperacionVentasPorCliente
                || operacionNormalizada == OperacionClientePorId
                || operacionNormalizada == OperacionVehiculoPorId
                || operacionNormalizada == OperacionConexion
                || operacionNormalizada == OperacionDesconexion
                || operacionNormalizada == OperacionLogout;
        }

        public static string[] SepararCamposPrincipales(string mensaje)
        {
            string mensajeNormalizado = NormalizarTextoMensaje(mensaje);

            if (string.IsNullOrWhiteSpace(mensajeNormalizado))
            {
                return Array.Empty<string>();
            }

            string[] partes = mensajeNormalizado.Split(DelimitadorCamposPrincipales);

            for (int i = 0; i < partes.Length; i++)
            {
                partes[i] = NormalizarTextoBasico(partes[i]);
            }

            return partes;
        }

        public static string[] SepararRegistros(string contenido)
        {
            string contenidoNormalizado = NormalizarTextoBasico(contenido);

            if (string.IsNullOrWhiteSpace(contenidoNormalizado))
            {
                return Array.Empty<string>();
            }

            string[] registros = contenidoNormalizado.Split(DelimitadorRegistros);

            for (int i = 0; i < registros.Length; i++)
            {
                registros[i] = NormalizarTextoBasico(registros[i]);
            }

            return registros;
        }

        public static string[] SepararColumnas(string registro)
        {
            string registroNormalizado = NormalizarTextoBasico(registro);

            if (string.IsNullOrWhiteSpace(registroNormalizado))
            {
                return Array.Empty<string>();
            }

            string[] columnas = registroNormalizado.Split(DelimitadorColumnasInternas);

            for (int i = 0; i < columnas.Length; i++)
            {
                columnas[i] = NormalizarTextoBasico(columnas[i]);
            }

            return columnas;
        }

        public static string UnirCampos(params string[] campos)
        {
            if (campos == null || campos.Length == 0)
            {
                throw new ArgumentException("Se requiere al menos un campo para construir el mensaje del protocolo.", nameof(campos));
            }

            string[] camposNormalizados = new string[campos.Length];

            for (int i = 0; i < campos.Length; i++)
            {
                camposNormalizados[i] = ValidarCampoProtocolo(campos[i], "Uno de los campos del protocolo es inválido.");
            }

            return string.Join(DelimitadorCamposPrincipales.ToString(), camposNormalizados);
        }

        public static string UnirRegistros(params string[] registros)
        {
            if (registros == null || registros.Length == 0)
            {
                return string.Empty;
            }

            string[] registrosNormalizados = new string[registros.Length];

            for (int i = 0; i < registros.Length; i++)
            {
                registrosNormalizados[i] = ValidarCampoProtocolo(registros[i], "Uno de los registros del protocolo es inválido.");
            }

            return string.Join(DelimitadorRegistros.ToString(), registrosNormalizados);
        }

        public static string UnirColumnas(params string[] columnas)
        {
            if (columnas == null || columnas.Length == 0)
            {
                return string.Empty;
            }

            string[] columnasNormalizadas = new string[columnas.Length];

            for (int i = 0; i < columnas.Length; i++)
            {
                columnasNormalizadas[i] = ValidarCampoProtocolo(columnas[i], "Una de las columnas del protocolo es inválida.");
            }

            return string.Join(DelimitadorColumnasInternas.ToString(), columnasNormalizadas);
        }

        public static string ValidarCampoProtocolo(string valor, string mensajeError)
        {
            string textoNormalizado = NormalizarTextoBasico(valor);

            if (string.IsNullOrWhiteSpace(textoNormalizado))
            {
                throw new ArgumentException(mensajeError);
            }

            if (ContieneDelimitadoresProtocolo(textoNormalizado))
            {
                throw new ArgumentException("El valor indicado contiene delimitadores reservados por el protocolo.");
            }

            return textoNormalizado;
        }

        public static bool ContieneDelimitadoresProtocolo(string valor)
        {
            string textoNormalizado = valor ?? string.Empty;

            return textoNormalizado.Contains(DelimitadorCamposPrincipales.ToString())
                || textoNormalizado.Contains(DelimitadorRegistros.ToString())
                || textoNormalizado.Contains(DelimitadorColumnasInternas.ToString())
                || textoNormalizado.Contains("\r")
                || textoNormalizado.Contains("\n");
        }

        public static string NormalizarTextoMensaje(string texto)
        {
            string textoNormalizado = texto?.Trim() ?? string.Empty;
            textoNormalizado = textoNormalizado.Replace("\r", string.Empty);
            textoNormalizado = textoNormalizado.Replace("\n", string.Empty);
            return textoNormalizado;
        }

        public static string NormalizarTextoBasico(string texto)
        {
            return texto?.Trim() ?? string.Empty;
        }

        public static string ConvertirEnteroRequerido(int valor, string nombreParametro)
        {
            if (valor <= 0)
            {
                throw new ArgumentException("El valor de " + nombreParametro + " debe ser mayor que cero.", nombreParametro);
            }

            return valor.ToString(CultureInfo.InvariantCulture);
        }

        public static string ConvertirDecimal(decimal valor, string nombreParametro)
        {
            if (valor < 0)
            {
                throw new ArgumentException("El valor de " + nombreParametro + " no puede ser negativo.", nombreParametro);
            }

            return valor.ToString(CultureInfo.InvariantCulture);
        }

        public static string ConvertirFechaHora(DateTime valor)
        {
            return valor.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static string ConvertirBooleanoBinario(bool valor)
        {
            return valor ? "1" : "0";
        }

        public static bool IntentarObtenerEntero(string valor, out int resultado)
        {
            string textoNormalizado = NormalizarTextoBasico(valor);

            return int.TryParse(
                textoNormalizado,
                NumberStyles.Integer,
                CultureInfo.InvariantCulture,
                out resultado);
        }

        public static bool IntentarObtenerDecimal(string valor, out decimal resultado)
        {
            string textoNormalizado = NormalizarTextoBasico(valor);

            return decimal.TryParse(
                textoNormalizado,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out resultado);
        }

        public static bool IntentarObtenerFechaHora(string valor, out DateTime resultado)
        {
            string textoNormalizado = NormalizarTextoBasico(valor);

            return DateTime.TryParseExact(
                textoNormalizado,
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out resultado);
        }

        public static bool IntentarObtenerBooleanoBinario(string valor, out bool resultado)
        {
            string textoNormalizado = NormalizarTextoBasico(valor);

            if (textoNormalizado == "1")
            {
                resultado = true;
                return true;
            }

            if (textoNormalizado == "0")
            {
                resultado = false;
                return true;
            }

            resultado = false;
            return false;
        }
    }
}