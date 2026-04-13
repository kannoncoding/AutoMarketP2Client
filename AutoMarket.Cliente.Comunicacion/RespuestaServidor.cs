/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Clase que representa y valida una respuesta protocolaria recibida desde el servidor TCP de AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Collections.Generic;
using System.Globalization;

namespace AutoMarket.Cliente.Comunicacion
{
    public sealed class RespuestaServidor
    {
        private readonly string _textoOriginal;
        private readonly string _estado;
        private readonly string _operacion;
        private readonly string _contenido;
        private readonly string[] _partes;
        private readonly bool _esExitosa;
        private readonly bool _esError;

        public string TextoOriginal
        {
            get => _textoOriginal;
        }

        public string Estado
        {
            get => _estado;
        }

        public string Operacion
        {
            get => _operacion;
        }

        public string Contenido
        {
            get => _contenido;
        }

        public string[] Partes
        {
            get
            {
                string[] copia = new string[_partes.Length];

                for (int i = 0; i < _partes.Length; i++)
                {
                    copia[i] = _partes[i];
                }

                return copia;
            }
        }

        public bool EsExitosa
        {
            get => _esExitosa;
        }

        public bool EsError
        {
            get => _esError;
        }

        public string MensajeError
        {
            get
            {
                if (!_esError)
                {
                    return string.Empty;
                }

                return _contenido;
            }
        }

        public bool TieneContenido
        {
            get => !string.IsNullOrWhiteSpace(_contenido);
        }

        public int CantidadPartes
        {
            get => _partes.Length;
        }

        public RespuestaServidor(string respuestaCruda)
        {
            string textoNormalizado = respuestaCruda?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(textoNormalizado))
            {
                throw new ArgumentException("La respuesta del servidor está vacía.", nameof(respuestaCruda));
            }

            string[] partes = textoNormalizado.Split('|');

            if (partes.Length < 2)
            {
                throw new FormatException("La respuesta del servidor no cumple con el formato mínimo esperado.");
            }

            string estado = NormalizarCampo(partes[0]).ToUpperInvariant();
            string operacion = NormalizarCampo(partes[1]).ToUpperInvariant();

            if (string.IsNullOrWhiteSpace(estado))
            {
                throw new FormatException("La respuesta del servidor no contiene un estado válido.");
            }

            if (string.IsNullOrWhiteSpace(operacion))
            {
                throw new FormatException("La respuesta del servidor no contiene una operación válida.");
            }

            if (estado != "OK" && estado != "ERROR")
            {
                throw new FormatException("El estado de la respuesta del servidor no es reconocido.");
            }

            string contenido = ObtenerContenidoDesdePartes(partes);

            _textoOriginal = textoNormalizado;
            _estado = estado;
            _operacion = operacion;
            _contenido = contenido;
            _partes = partes;
            _esExitosa = estado == "OK";
            _esError = estado == "ERROR";
        }

        public static RespuestaServidor CrearDesdeTexto(string respuestaCruda)
        {
            return new RespuestaServidor(respuestaCruda);
        }

        public void ValidarExito()
        {
            if (_esError)
            {
                throw new InvalidOperationException(
                    "El servidor devolvió un error en la operación '" + _operacion + "': " + _contenido);
            }

            if (!_esExitosa)
            {
                throw new InvalidOperationException(
                    "La respuesta del servidor no representa una operación exitosa válida.");
            }
        }

        public void ValidarOperacion(string operacionEsperada)
        {
            string operacionNormalizada = operacionEsperada?.Trim().ToUpperInvariant() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(operacionNormalizada))
            {
                throw new ArgumentException("La operación esperada es obligatoria.", nameof(operacionEsperada));
            }

            if (!string.Equals(_operacion, operacionNormalizada, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "La operación recibida del servidor no coincide con la esperada. Operación esperada: '"
                    + operacionNormalizada
                    + "'. Operación recibida: '"
                    + _operacion
                    + "'.");
            }
        }

        public void ValidarExitoYOperacion(string operacionEsperada)
        {
            ValidarExito();
            ValidarOperacion(operacionEsperada);
        }

        public string ObtenerParte(int indice)
        {
            if (indice < 0 || indice >= _partes.Length)
            {
                throw new IndexOutOfRangeException("El índice solicitado no existe dentro de la respuesta del servidor.");
            }

            return NormalizarCampo(_partes[indice]);
        }

        public string ObtenerParteRequerida(int indice, string mensajeError)
        {
            string valor = ObtenerParte(indice);

            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new InvalidOperationException(mensajeError);
            }

            return valor;
        }

        public int ObtenerEnteroEnParte(int indice, string mensajeError)
        {
            string valor = ObtenerParteRequerida(indice, mensajeError);

            if (!int.TryParse(valor, NumberStyles.Integer, CultureInfo.InvariantCulture, out int resultado))
            {
                throw new InvalidOperationException(mensajeError);
            }

            return resultado;
        }

        public decimal ObtenerDecimalEnParte(int indice, string mensajeError)
        {
            string valor = ObtenerParteRequerida(indice, mensajeError);

            if (!decimal.TryParse(valor, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal resultado))
            {
                throw new InvalidOperationException(mensajeError);
            }

            return resultado;
        }

        public DateTime ObtenerFechaEnParte(int indice, string mensajeError)
        {
            string valor = ObtenerParteRequerida(indice, mensajeError);

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

        public bool ObtenerBooleanoBinarioEnParte(int indice, string mensajeError)
        {
            string valor = ObtenerParteRequerida(indice, mensajeError);

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

        public List<string> ObtenerRegistrosDesdeContenido()
        {
            List<string> registros = new List<string>();

            if (string.IsNullOrWhiteSpace(_contenido))
            {
                return registros;
            }

            string[] partesRegistros = _contenido.Split(';');

            for (int i = 0; i < partesRegistros.Length; i++)
            {
                string registro = NormalizarCampo(partesRegistros[i]);

                if (!string.IsNullOrWhiteSpace(registro))
                {
                    registros.Add(registro);
                }
            }

            return registros;
        }

        public string[] ObtenerColumnasDeRegistro(string registro)
        {
            string registroNormalizado = registro?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(registroNormalizado))
            {
                return Array.Empty<string>();
            }

            string[] columnas = registroNormalizado.Split(',');

            for (int i = 0; i < columnas.Length; i++)
            {
                columnas[i] = NormalizarCampo(columnas[i]);
            }

            return columnas;
        }

        public string ObtenerContenidoRequerido(string mensajeError)
        {
            if (string.IsNullOrWhiteSpace(_contenido))
            {
                throw new InvalidOperationException(mensajeError);
            }

            return _contenido;
        }

        public override string ToString()
        {
            return _textoOriginal;
        }

        private string ObtenerContenidoDesdePartes(string[] partes)
        {
            if (partes == null || partes.Length <= 2)
            {
                return string.Empty;
            }

            if (partes.Length == 3)
            {
                return NormalizarCampo(partes[2]);
            }

            return string.Join("|", partes, 2, partes.Length - 2).Trim();
        }

        private string NormalizarCampo(string valor)
        {
            return valor?.Trim() ?? string.Empty;
        }
    }
}