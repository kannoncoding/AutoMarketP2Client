/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Diseño visual del formulario de consulta de sucursales del cliente AutoMarket con estilo clásico de escritorio y visualización en DataGridView.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-02-12
*/

namespace AutoMarket.Cliente.Presentacion
{
    partial class FrmConsultaSucursal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlEncabezado = new Panel();
            lblSubtitulo = new Label();
            lblTituloPrincipal = new Label();
            grpSesion = new GroupBox();
            lblConexionValor = new Label();
            lblConexion = new Label();
            lblClienteValor = new Label();
            lblCliente = new Label();
            grpConsulta = new GroupBox();
            dgvSucursales = new DataGridView();
            pnlResumen = new Panel();
            lblCantidadRegistrosValor = new Label();
            lblCantidadRegistros = new Label();
            pnlAcciones = new Panel();
            btnCerrar = new Button();
            btnActualizar = new Button();
            pnlEncabezado.SuspendLayout();
            grpSesion.SuspendLayout();
            grpConsulta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSucursales).BeginInit();
            pnlResumen.SuspendLayout();
            pnlAcciones.SuspendLayout();
            SuspendLayout();
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.Navy;
            pnlEncabezado.BorderStyle = BorderStyle.Fixed3D;
            pnlEncabezado.Controls.Add(lblSubtitulo);
            pnlEncabezado.Controls.Add(lblTituloPrincipal);
            pnlEncabezado.Location = new Point(12, 12);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(1060, 76);
            pnlEncabezado.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.BackColor = Color.Transparent;
            lblSubtitulo.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblSubtitulo.ForeColor = Color.White;
            lblSubtitulo.Location = new Point(18, 43);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(382, 13);
            lblSubtitulo.TabIndex = 1;
            lblSubtitulo.Text = "Consulta de sucursales activas disponibles para realizar operaciones de compra";
            // 
            // lblTituloPrincipal
            // 
            lblTituloPrincipal.AutoSize = true;
            lblTituloPrincipal.BackColor = Color.Transparent;
            lblTituloPrincipal.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Bold);
            lblTituloPrincipal.ForeColor = Color.White;
            lblTituloPrincipal.Location = new Point(16, 14);
            lblTituloPrincipal.Name = "lblTituloPrincipal";
            lblTituloPrincipal.Size = new Size(258, 24);
            lblTituloPrincipal.TabIndex = 0;
            lblTituloPrincipal.Text = "🏢 Consulta de Sucursales";
            // 
            // grpSesion
            // 
            grpSesion.Controls.Add(lblConexionValor);
            grpSesion.Controls.Add(lblConexion);
            grpSesion.Controls.Add(lblClienteValor);
            grpSesion.Controls.Add(lblCliente);
            grpSesion.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            grpSesion.Location = new Point(12, 102);
            grpSesion.Name = "grpSesion";
            grpSesion.Size = new Size(1060, 86);
            grpSesion.TabIndex = 1;
            grpSesion.TabStop = false;
            grpSesion.Text = "🔐 Información de sesión";
            // 
            // lblConexionValor
            // 
            lblConexionValor.BorderStyle = BorderStyle.Fixed3D;
            lblConexionValor.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblConexionValor.Location = new Point(172, 49);
            lblConexionValor.Name = "lblConexionValor";
            lblConexionValor.Size = new Size(868, 23);
            lblConexionValor.TabIndex = 3;
            lblConexionValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblConexion
            // 
            lblConexion.AutoSize = true;
            lblConexion.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblConexion.Location = new Point(22, 54);
            lblConexion.Name = "lblConexion";
            lblConexion.Size = new Size(114, 13);
            lblConexion.TabIndex = 2;
            lblConexion.Text = "Resumen de la sesión:";
            // 
            // lblClienteValor
            // 
            lblClienteValor.BorderStyle = BorderStyle.Fixed3D;
            lblClienteValor.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblClienteValor.Location = new Point(172, 21);
            lblClienteValor.Name = "lblClienteValor";
            lblClienteValor.Size = new Size(310, 23);
            lblClienteValor.TabIndex = 1;
            lblClienteValor.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCliente
            // 
            lblCliente.AutoSize = true;
            lblCliente.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblCliente.Location = new Point(22, 26);
            lblCliente.Name = "lblCliente";
            lblCliente.Size = new Size(101, 13);
            lblCliente.TabIndex = 0;
            lblCliente.Text = "Cliente autenticado:";
            // 
            // grpConsulta
            // 
            grpConsulta.Controls.Add(dgvSucursales);
            grpConsulta.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            grpConsulta.Location = new Point(12, 202);
            grpConsulta.Name = "grpConsulta";
            grpConsulta.Size = new Size(1060, 368);
            grpConsulta.TabIndex = 2;
            grpConsulta.TabStop = false;
            grpConsulta.Text = "📋 Sucursales activas";
            // 
            // dgvSucursales
            // 
            dgvSucursales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSucursales.Location = new Point(16, 24);
            dgvSucursales.Name = "dgvSucursales";
            dgvSucursales.Size = new Size(1028, 328);
            dgvSucursales.TabIndex = 0;
            // 
            // pnlResumen
            // 
            pnlResumen.BorderStyle = BorderStyle.Fixed3D;
            pnlResumen.Controls.Add(lblCantidadRegistrosValor);
            pnlResumen.Controls.Add(lblCantidadRegistros);
            pnlResumen.Location = new Point(12, 584);
            pnlResumen.Name = "pnlResumen";
            pnlResumen.Size = new Size(1060, 46);
            pnlResumen.TabIndex = 3;
            // 
            // lblCantidadRegistrosValor
            // 
            lblCantidadRegistrosValor.AutoSize = true;
            lblCantidadRegistrosValor.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            lblCantidadRegistrosValor.Location = new Point(175, 15);
            lblCantidadRegistrosValor.Name = "lblCantidadRegistrosValor";
            lblCantidadRegistrosValor.Size = new Size(14, 13);
            lblCantidadRegistrosValor.TabIndex = 1;
            lblCantidadRegistrosValor.Text = "0";
            // 
            // lblCantidadRegistros
            // 
            lblCantidadRegistros.AutoSize = true;
            lblCantidadRegistros.Font = new Font("Microsoft Sans Serif", 8.25F);
            lblCantidadRegistros.Location = new Point(14, 15);
            lblCantidadRegistros.Name = "lblCantidadRegistros";
            lblCantidadRegistros.Size = new Size(161, 13);
            lblCantidadRegistros.TabIndex = 0;
            lblCantidadRegistros.Text = "📌 Cantidad de registros visibles:";
            // 
            // pnlAcciones
            // 
            pnlAcciones.BorderStyle = BorderStyle.Fixed3D;
            pnlAcciones.Controls.Add(btnCerrar);
            pnlAcciones.Controls.Add(btnActualizar);
            pnlAcciones.Location = new Point(12, 644);
            pnlAcciones.Name = "pnlAcciones";
            pnlAcciones.Size = new Size(1060, 56);
            pnlAcciones.TabIndex = 4;
            // 
            // btnCerrar
            // 
            btnCerrar.BackColor = Color.FromArgb(200, 0, 0);
            btnCerrar.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(936, 12);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(108, 28);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "❌ Cerrar";
            btnCerrar.UseVisualStyleBackColor = false;
            btnCerrar.Click += btnCerrar_Click;
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.FromArgb(120, 200, 120);
            btnActualizar.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold);
            btnActualizar.Location = new Point(14, 12);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(128, 28);
            btnActualizar.TabIndex = 0;
            btnActualizar.Text = "🔄 Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // FrmConsultaSucursal
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1084, 712);
            Controls.Add(pnlAcciones);
            Controls.Add(pnlResumen);
            Controls.Add(grpConsulta);
            Controls.Add(grpSesion);
            Controls.Add(pnlEncabezado);
            Font = new Font("Microsoft Sans Serif", 8.25F);
            Name = "FrmConsultaSucursal";
            Text = "AutoMarket - Consulta de Sucursales";
            Load += FrmConsultaSucursal_Load;
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            grpSesion.ResumeLayout(false);
            grpSesion.PerformLayout();
            grpConsulta.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSucursales).EndInit();
            pnlResumen.ResumeLayout(false);
            pnlResumen.PerformLayout();
            pnlAcciones.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTituloPrincipal;
        private System.Windows.Forms.GroupBox grpSesion;
        private System.Windows.Forms.Label lblConexionValor;
        private System.Windows.Forms.Label lblConexion;
        private System.Windows.Forms.Label lblClienteValor;
        private System.Windows.Forms.Label lblCliente;
        private System.Windows.Forms.GroupBox grpConsulta;
        private System.Windows.Forms.DataGridView dgvSucursales;
        private System.Windows.Forms.Panel pnlResumen;
        private System.Windows.Forms.Label lblCantidadRegistrosValor;
        private System.Windows.Forms.Label lblCantidadRegistros;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnActualizar;
    }
}