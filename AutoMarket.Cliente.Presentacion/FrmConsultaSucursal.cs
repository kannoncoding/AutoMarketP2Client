/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Formulario de presentación para consultar las sucursales activas disponibles para el cliente autenticado mediante comunicación TCP con el servidor AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AutoMarket.Cliente.Logica;

namespace AutoMarket.Cliente.Presentacion
{
    public partial class FrmConsultaSucursal : Form
    {
        private readonly SucursalClienteLogica _sucursalClienteLogica;
        private readonly SesionCliente _sesionCliente;

        public FrmConsultaSucursal(
            SucursalClienteLogica sucursalClienteLogica,
            SesionCliente sesionCliente)
        {
            _sucursalClienteLogica = sucursalClienteLogica ?? throw new ArgumentNullException(nameof(sucursalClienteLogica));
            _sesionCliente = sesionCliente ?? throw new ArgumentNullException(nameof(sesionCliente));

            InitializeComponent();
            ConfigurarFormulario();
            ConfigurarDataGridView();
            MostrarInformacionSesion();
        }

        private void FrmConsultaSucursal_Load(object sender, EventArgs e)
        {
            try
            {
                _sesionCliente.ValidarClienteAutenticado();
                CargarSucursales();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No fue posible cargar la consulta de sucursales.\n\n" + ex.Message,
                    "Error de carga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ConfigurarFormulario()
        {
            Text = "AutoMarket - Consulta de Sucursales";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = SystemColors.Control;
        }

        private void ConfigurarDataGridView()
        {
            dgvSucursales.AutoGenerateColumns = false;
            dgvSucursales.ReadOnly = true;
            dgvSucursales.AllowUserToAddRows = false;
            dgvSucursales.AllowUserToDeleteRows = false;
            dgvSucursales.AllowUserToResizeRows = false;
            dgvSucursales.MultiSelect = false;
            dgvSucursales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSucursales.RowHeadersVisible = false;
            dgvSucursales.BackgroundColor = SystemColors.Window;
            dgvSucursales.BorderStyle = BorderStyle.Fixed3D;
            dgvSucursales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSucursales.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvSucursales.EnableHeadersVisualStyles = false;
            dgvSucursales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvSucursales.ColumnHeadersHeight = 42;
            dgvSucursales.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSucursales.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvSucursales.Columns.Clear();

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdSucursal",
                HeaderText = "Id Sucursal",
                DataPropertyName = "IdSucursal",
                FillWeight = 55
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre",
                FillWeight = 110
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDireccion",
                HeaderText = "Dirección",
                DataPropertyName = "Direccion",
                FillWeight = 160
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTelefono",
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono",
                FillWeight = 80
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombreVendedor",
                HeaderText = "Vendedor\r\n(Nombre)",
                DataPropertyName = "NombreVendedor",
                FillWeight = 120
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colIdentificacionVendedor",
                HeaderText = "Vendedor\r\n(Identificación)",
                DataPropertyName = "IdentificacionVendedor",
                FillWeight = 105
            });

            dgvSucursales.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colActivo",
                HeaderText = "Activo",
                DataPropertyName = "ActivoDescripcion",
                FillWeight = 55
            });
        }

        private void MostrarInformacionSesion()
        {
            try
            {
                lblClienteValor.Text = _sesionCliente.ObtenerNombreClienteRequerido();
                lblConexionValor.Text = _sesionCliente.ResumenSesion;
            }
            catch
            {
                lblClienteValor.Text = "No disponible";
                lblConexionValor.Text = "Sin sesión válida.";
            }
        }

        private void CargarSucursales()
        {
            List<SucursalVistaInfo> sucursales = _sucursalClienteLogica.ObtenerSucursalesActivasParaVista();
            List<SucursalConsultaItem> items = new List<SucursalConsultaItem>();

            for (int i = 0; i < sucursales.Count; i++)
            {
                SucursalVistaInfo sucursal = sucursales[i];

                items.Add(new SucursalConsultaItem(
                    sucursal.IdSucursal,
                    sucursal.Nombre,
                    sucursal.Direccion,
                    sucursal.Telefono,
                    sucursal.IdVendedor,
                    sucursal.NombreVendedor,
                    sucursal.IdentificacionVendedor,
                    sucursal.Activo ? "Sí" : "No"));
            }

            dgvSucursales.DataSource = null;
            dgvSucursales.DataSource = items;
            lblCantidadRegistrosValor.Text = items.Count.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                _sesionCliente.ValidarClienteAutenticado();
                MostrarInformacionSesion();
                CargarSucursales();

                MessageBox.Show(
                    "La consulta de sucursales fue actualizada correctamente.",
                    "Consulta actualizada",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No fue posible actualizar la consulta de sucursales.\n\n" + ex.Message,
                    "Error de actualización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult resultado = MessageBox.Show(
                    "¿Desea cerrar la consulta de sucursales?",
                    "Cerrar formulario",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error al cerrar el formulario.\n\n" + ex.Message,
                    "Error al cerrar",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private sealed class SucursalConsultaItem
        {
            public int IdSucursal { get; }
            public string Nombre { get; }
            public string Direccion { get; }
            public string Telefono { get; }
            public int IdVendedor { get; }
            public string NombreVendedor { get; }
            public string IdentificacionVendedor { get; }
            public string ActivoDescripcion { get; }

            public SucursalConsultaItem(
                int idSucursal,
                string nombre,
                string direccion,
                string telefono,
                int idVendedor,
                string nombreVendedor,
                string identificacionVendedor,
                string activoDescripcion)
            {
                IdSucursal = idSucursal;
                Nombre = nombre;
                Direccion = direccion;
                Telefono = telefono;
                IdVendedor = idVendedor;
                NombreVendedor = nombreVendedor;
                IdentificacionVendedor = identificacionVendedor;
                ActivoDescripcion = activoDescripcion;
            }
        }
    }
}