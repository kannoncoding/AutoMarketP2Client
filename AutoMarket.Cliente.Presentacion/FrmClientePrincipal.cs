/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario principal de la aplicación cliente AutoMarket, encargado de administrar la conexión TCP, la autenticación del cliente y el acceso a los módulos principales del sistema.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AutoMarket.Cliente.Comunicacion;
using AutoMarket.Cliente.Logica;

namespace AutoMarket.Cliente.Presentacion
{
    public partial class FrmClientePrincipal : Form
    {
        private ClienteTcp? _clienteTcp;
        private SesionCliente? _sesionCliente;
        private AutenticacionClienteLogica? _autenticacionClienteLogica;
        private SucursalClienteLogica? _sucursalClienteLogica;
        private VehiculoClienteLogica? _vehiculoClienteLogica;
        private VentaClienteLogica? _ventaClienteLogica;

        public FrmClientePrincipal()
        {
            InitializeComponent();
            InicializarFormulario();
        }

        private void InicializarFormulario()
        {
            CargarConfiguracionInicial();
            AplicarEstadoInicial();
            AgregarEvento("Aplicación cliente iniciada.");
        }

        private void CargarConfiguracionInicial()
        {
            txtServidorIp.Text = ObtenerConfiguracion("ServidorIP", "127.0.0.1");
            txtPuerto.Text = ObtenerConfiguracion("ServidorPuerto", "15500");
            txtIdCliente.Text = string.Empty;
        }

        private string ObtenerConfiguracion(string llave, string valorPredeterminado)
        {
            try
            {
                string? valor = ConfigurationManager.AppSettings[llave];

                if (string.IsNullOrWhiteSpace(valor))
                {
                    return valorPredeterminado;
                }

                return valor.Trim();
            }
            catch
            {
                return valorPredeterminado;
            }
        }

        private void AplicarEstadoInicial()
        {
            lblEstadoConexionValor.Text = "Desconectado";
            lblEstadoConexionValor.ForeColor = Color.Maroon;

            lblClienteAutenticadoValor.Text = "Sin autenticar";
            lblResumenSesionValor.Text = "Sin sesión iniciada.";

            btnConectar.Enabled = true;
            btnDesconectar.Enabled = false;
            btnProbarConexion.Enabled = false;
            btnAutenticar.Enabled = false;

            btnConsultarSucursales.Enabled = false;
            btnConsultarVehiculos.Enabled = false;
            btnRegistrarVenta.Enabled = false;
            btnConsultarVentas.Enabled = false;

            btnLimpiarBitacora.Enabled = true;
            btnSalir.Enabled = true;

            txtServidorIp.Enabled = true;
            txtPuerto.Enabled = true;
            txtIdCliente.Enabled = false;
        }

        private void RefrescarEstadoPantalla()
        {
            bool conectado = _sesionCliente != null && _sesionCliente.TieneConexionActiva;
            bool autenticado = _sesionCliente != null && _sesionCliente.TieneClienteAutenticado;

            lblEstadoConexionValor.Text = conectado ? "Conectado" : "Desconectado";
            lblEstadoConexionValor.ForeColor = conectado ? Color.DarkGreen : Color.Maroon;

            lblClienteAutenticadoValor.Text = autenticado
                ? _sesionCliente!.ObtenerNombreClienteRequerido()
                : "Sin autenticar";

            lblResumenSesionValor.Text = _sesionCliente != null
                ? _sesionCliente.ResumenSesion
                : "Sin sesión iniciada.";

            txtServidorIp.Enabled = !conectado;
            txtPuerto.Enabled = !conectado;
            txtIdCliente.Enabled = conectado;

            btnConectar.Enabled = !conectado;
            btnDesconectar.Enabled = conectado;
            btnProbarConexion.Enabled = conectado;
            btnAutenticar.Enabled = conectado;

            btnConsultarSucursales.Enabled = autenticado;
            btnConsultarVehiculos.Enabled = autenticado;
            btnRegistrarVenta.Enabled = autenticado;
            btnConsultarVentas.Enabled = autenticado;
        }

        private void CrearDependenciasCliente(string direccionServidor, int puertoServidor)
        {
            _clienteTcp = new ClienteTcp(direccionServidor, puertoServidor);
            _clienteTcp.EventoCliente += ClienteTcp_EventoCliente;

            _sesionCliente = new SesionCliente();

            _autenticacionClienteLogica = new AutenticacionClienteLogica(_clienteTcp, _sesionCliente);
            _sucursalClienteLogica = new SucursalClienteLogica(_clienteTcp, _sesionCliente);
            _vehiculoClienteLogica = new VehiculoClienteLogica(_clienteTcp, _sesionCliente);
            _ventaClienteLogica = new VentaClienteLogica(_clienteTcp, _sesionCliente);
        }

        private void LiberarDependenciasCliente()
        {
            if (_clienteTcp != null)
            {
                _clienteTcp.EventoCliente -= ClienteTcp_EventoCliente;

                try
                {
                    _clienteTcp.Dispose();
                }
                catch
                {
                }
            }

            _clienteTcp = null;
            _sesionCliente = null;
            _autenticacionClienteLogica = null;
            _sucursalClienteLogica = null;
            _vehiculoClienteLogica = null;
            _ventaClienteLogica = null;
        }

        private void ClienteTcp_EventoCliente(string mensaje)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string>(ClienteTcp_EventoCliente), mensaje);
                return;
            }

            AgregarEvento(mensaje);
        }

        private void AgregarEvento(string mensaje)
        {
            string texto = mensaje?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(texto))
            {
                return;
            }

            txtBitacora.AppendText(texto + Environment.NewLine);
        }

        private bool ValidarDatosConexion(out string direccionServidor, out int puertoServidor)
        {
            direccionServidor = txtServidorIp.Text.Trim();

            if (string.IsNullOrWhiteSpace(direccionServidor))
            {
                MessageBox.Show(
                    "La dirección IP del servidor es obligatoria.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtServidorIp.Focus();
                puertoServidor = 0;
                return false;
            }

            if (!int.TryParse(txtPuerto.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out puertoServidor))
            {
                MessageBox.Show(
                    "El puerto indicado no es válido.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPuerto.Focus();
                return false;
            }

            if (puertoServidor <= 0 || puertoServidor > 65535)
            {
                MessageBox.Show(
                    "El puerto debe estar entre 1 y 65535.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPuerto.Focus();
                return false;
            }

            return true;
        }

        private bool ValidarIdCliente(out int idCliente)
        {
            if (!int.TryParse(txtIdCliente.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out idCliente))
            {
                MessageBox.Show(
                    "El Id del cliente no es válido.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtIdCliente.Focus();
                return false;
            }

            if (idCliente <= 0)
            {
                MessageBox.Show(
                    "El Id del cliente debe ser mayor que cero.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtIdCliente.Focus();
                return false;
            }

            return true;
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarDatosConexion(out string direccionServidor, out int puertoServidor))
                {
                    return;
                }

                LiberarDependenciasCliente();
                CrearDependenciasCliente(direccionServidor, puertoServidor);

                if (_autenticacionClienteLogica == null)
                {
                    throw new InvalidOperationException("No fue posible inicializar la lógica de autenticación del cliente.");
                }

                _autenticacionClienteLogica.ConectarServidor();

                AgregarEvento("Conexión realizada correctamente al servidor " + direccionServidor + ":" + puertoServidor.ToString(CultureInfo.InvariantCulture) + ".");
                RefrescarEstadoPantalla();

                MessageBox.Show(
                    "La conexión con el servidor se realizó correctamente.",
                    "Conexión exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AgregarEvento("Error al conectar: " + ex.Message);

                MessageBox.Show(
                    "No fue posible conectar con el servidor.\n\n" + ex.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                LiberarDependenciasCliente();
                AplicarEstadoInicial();
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_autenticacionClienteLogica != null)
                {
                    _autenticacionClienteLogica.DesconectarServidor();
                }

                AgregarEvento("Desconexión realizada correctamente.");
            }
            catch (Exception ex)
            {
                AgregarEvento("Error al desconectar: " + ex.Message);

                MessageBox.Show(
                    "Se produjo un error al desconectar.\n\n" + ex.Message,
                    "Error de desconexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                LiberarDependenciasCliente();
                AplicarEstadoInicial();
            }
        }

        private void btnProbarConexion_Click(object sender, EventArgs e)
        {
            try
            {
                if (_autenticacionClienteLogica == null)
                {
                    throw new InvalidOperationException("La lógica de autenticación no está disponible.");
                }

                bool resultado = _autenticacionClienteLogica.ProbarConexionServidor();

                if (resultado)
                {
                    AgregarEvento("Prueba de conexión exitosa.");

                    MessageBox.Show(
                        "La comunicación con el servidor es correcta.",
                        "Prueba de conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    AgregarEvento("La prueba de conexión no fue exitosa.");

                    MessageBox.Show(
                        "La comunicación con el servidor no respondió como se esperaba.",
                        "Prueba de conexión",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                RefrescarEstadoPantalla();
            }
            catch (Exception ex)
            {
                AgregarEvento("Error al probar conexión: " + ex.Message);

                MessageBox.Show(
                    "No fue posible probar la conexión con el servidor.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                RefrescarEstadoPantalla();
            }
        }

        private void btnAutenticar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidarIdCliente(out int idCliente))
                {
                    return;
                }

                if (_autenticacionClienteLogica == null)
                {
                    throw new InvalidOperationException("La lógica de autenticación no está disponible.");
                }

                ResultadoAutenticacionCliente resultado = _autenticacionClienteLogica.AutenticarCliente(idCliente);

                if (!resultado.AutenticacionExitosa)
                {
                    AgregarEvento("Autenticación rechazada para el cliente " + idCliente.ToString(CultureInfo.InvariantCulture) + ".");

                    MessageBox.Show(
                        resultado.Mensaje,
                        "Autenticación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    RefrescarEstadoPantalla();
                    return;
                }

                AgregarEvento("Cliente autenticado correctamente: " + resultado.IdCliente.ToString(CultureInfo.InvariantCulture) + " - " + resultado.NombreCliente + ".");
                RefrescarEstadoPantalla();

                MessageBox.Show(
                    "Autenticación realizada correctamente.\n\nCliente: " + resultado.NombreCliente,
                    "Autenticación exitosa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AgregarEvento("Error al autenticar cliente: " + ex.Message);

                MessageBox.Show(
                    "No fue posible autenticar el cliente.\n\n" + ex.Message,
                    "Error de autenticación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                RefrescarEstadoPantalla();
            }
        }

        private void btnConsultarSucursales_Click(object sender, EventArgs e)
        {
            try
            {
                if (_sucursalClienteLogica == null)
                {
                    throw new InvalidOperationException("La lógica de sucursales no está disponible.");
                }

                if (_sesionCliente == null)
                {
                    throw new InvalidOperationException("La sesión del cliente no está disponible.");
                }

                using (FrmConsultaSucursal formulario = new FrmConsultaSucursal(_sucursalClienteLogica, _sesionCliente))
                {
                    formulario.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                AgregarEvento("Error al abrir consulta de sucursales: " + ex.Message);

                MessageBox.Show(
                    "No fue posible abrir la consulta de sucursales.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnConsultarVehiculos_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "El formulario de consulta de vehículos aún no ha sido implementado en la capa de presentación.",
                "Módulo pendiente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnRegistrarVenta_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "El formulario de registro de venta aún no ha sido implementado en la capa de presentación.",
                "Módulo pendiente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnConsultarVentas_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "El formulario de consulta de ventas aún no ha sido implementado en la capa de presentación.",
                "Módulo pendiente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnLimpiarBitacora_Click(object sender, EventArgs e)
        {
            txtBitacora.Clear();
            AgregarEvento("Bitácora limpiada manualmente.");
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmClientePrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (_autenticacionClienteLogica != null && _clienteTcp != null && _clienteTcp.EstaConectado)
                {
                    _autenticacionClienteLogica.DesconectarServidor();
                }
            }
            catch
            {
            }
            finally
            {
                LiberarDependenciasCliente();
            }
        }
    }
}