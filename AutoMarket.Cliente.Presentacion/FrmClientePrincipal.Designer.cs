/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Archivo de diseño del formulario principal de la aplicación cliente AutoMarket.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

namespace AutoMarket.Cliente.Presentacion
{
    partial class FrmClientePrincipal
    {
        /// <summary>
        /// Variable requerida por el diseñador.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se debe modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlEncabezado = new System.Windows.Forms.Panel();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTituloPrincipal = new System.Windows.Forms.Label();
            this.grpConexion = new System.Windows.Forms.GroupBox();
            this.btnProbarConexion = new System.Windows.Forms.Button();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.btnConectar = new System.Windows.Forms.Button();
            this.txtPuerto = new System.Windows.Forms.TextBox();
            this.txtServidorIp = new System.Windows.Forms.TextBox();
            this.lblPuerto = new System.Windows.Forms.Label();
            this.lblServidorIp = new System.Windows.Forms.Label();
            this.grpAutenticacion = new System.Windows.Forms.GroupBox();
            this.lblClienteAutenticadoValor = new System.Windows.Forms.Label();
            this.lblClienteAutenticado = new System.Windows.Forms.Label();
            this.btnAutenticar = new System.Windows.Forms.Button();
            this.txtIdCliente = new System.Windows.Forms.TextBox();
            this.lblIdCliente = new System.Windows.Forms.Label();
            this.grpEstado = new System.Windows.Forms.GroupBox();
            this.lblResumenSesionValor = new System.Windows.Forms.Label();
            this.lblResumenSesion = new System.Windows.Forms.Label();
            this.lblEstadoConexionValor = new System.Windows.Forms.Label();
            this.lblEstadoConexion = new System.Windows.Forms.Label();
            this.grpModulos = new System.Windows.Forms.GroupBox();
            this.tlpModulos = new System.Windows.Forms.TableLayoutPanel();
            this.btnConsultarSucursales = new System.Windows.Forms.Button();
            this.btnConsultarVehiculos = new System.Windows.Forms.Button();
            this.btnRegistrarVenta = new System.Windows.Forms.Button();
            this.btnConsultarVentas = new System.Windows.Forms.Button();
            this.grpBitacora = new System.Windows.Forms.GroupBox();
            this.txtBitacora = new System.Windows.Forms.TextBox();
            this.pnlAcciones = new System.Windows.Forms.Panel();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnLimpiarBitacora = new System.Windows.Forms.Button();
            this.pnlEncabezado.SuspendLayout();
            this.grpConexion.SuspendLayout();
            this.grpAutenticacion.SuspendLayout();
            this.grpEstado.SuspendLayout();
            this.grpModulos.SuspendLayout();
            this.tlpModulos.SuspendLayout();
            this.grpBitacora.SuspendLayout();
            this.pnlAcciones.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlEncabezado
            // 
            this.pnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(128)))));
            this.pnlEncabezado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEncabezado.Controls.Add(this.lblSubtitulo);
            this.pnlEncabezado.Controls.Add(this.lblTituloPrincipal);
            this.pnlEncabezado.Location = new System.Drawing.Point(12, 12);
            this.pnlEncabezado.Name = "pnlEncabezado";
            this.pnlEncabezado.Size = new System.Drawing.Size(960, 72);
            this.pnlEncabezado.TabIndex = 0;
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitulo.ForeColor = System.Drawing.Color.White;
            this.lblSubtitulo.Location = new System.Drawing.Point(18, 42);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(389, 13);
            this.lblSubtitulo.TabIndex = 1;
            this.lblSubtitulo.Text = "Cliente TCP - Conexión, autenticación y operaciones de compra";
            // 
            // lblTituloPrincipal
            // 
            this.lblTituloPrincipal.AutoSize = true;
            this.lblTituloPrincipal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTituloPrincipal.ForeColor = System.Drawing.Color.White;
            this.lblTituloPrincipal.Location = new System.Drawing.Point(16, 12);
            this.lblTituloPrincipal.Name = "lblTituloPrincipal";
            this.lblTituloPrincipal.Size = new System.Drawing.Size(336, 25);
            this.lblTituloPrincipal.TabIndex = 0;
            this.lblTituloPrincipal.Text = "🚗 AutoMarket - Cliente Principal";
            // 
            // grpConexion
            // 
            this.grpConexion.Controls.Add(this.btnProbarConexion);
            this.grpConexion.Controls.Add(this.btnDesconectar);
            this.grpConexion.Controls.Add(this.btnConectar);
            this.grpConexion.Controls.Add(this.txtPuerto);
            this.grpConexion.Controls.Add(this.txtServidorIp);
            this.grpConexion.Controls.Add(this.lblPuerto);
            this.grpConexion.Controls.Add(this.lblServidorIp);
            this.grpConexion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpConexion.Location = new System.Drawing.Point(12, 94);
            this.grpConexion.Name = "grpConexion";
            this.grpConexion.Size = new System.Drawing.Size(470, 120);
            this.grpConexion.TabIndex = 1;
            this.grpConexion.TabStop = false;
            this.grpConexion.Text = "🌐 Conexión con servidor";
            // 
            // btnProbarConexion
            // 
            this.btnProbarConexion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(220)))), ((int)(((byte)(255)))));
            this.btnProbarConexion.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnProbarConexion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnProbarConexion.Location = new System.Drawing.Point(317, 79);
            this.btnProbarConexion.Name = "btnProbarConexion";
            this.btnProbarConexion.Size = new System.Drawing.Size(135, 26);
            this.btnProbarConexion.TabIndex = 6;
            this.btnProbarConexion.Text = "📡 Probar";
            this.btnProbarConexion.UseVisualStyleBackColor = false;
            this.btnProbarConexion.Click += new System.EventHandler(this.btnProbarConexion_Click);
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(210)))), ((int)(((byte)(170)))));
            this.btnDesconectar.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnDesconectar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnDesconectar.Location = new System.Drawing.Point(166, 79);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(135, 26);
            this.btnDesconectar.TabIndex = 5;
            this.btnDesconectar.Text = "🔌 Desconectar";
            this.btnDesconectar.UseVisualStyleBackColor = false;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // btnConectar
            // 
            this.btnConectar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(220)))), ((int)(((byte)(170)))));
            this.btnConectar.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnConectar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnConectar.Location = new System.Drawing.Point(15, 79);
            this.btnConectar.Name = "btnConectar";
            this.btnConectar.Size = new System.Drawing.Size(135, 26);
            this.btnConectar.TabIndex = 4;
            this.btnConectar.Text = "✅ Conectar";
            this.btnConectar.UseVisualStyleBackColor = false;
            this.btnConectar.Click += new System.EventHandler(this.btnConectar_Click);
            // 
            // txtPuerto
            // 
            this.txtPuerto.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtPuerto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPuerto.Location = new System.Drawing.Point(344, 35);
            this.txtPuerto.Name = "txtPuerto";
            this.txtPuerto.Size = new System.Drawing.Size(108, 20);
            this.txtPuerto.TabIndex = 3;
            // 
            // txtServidorIp
            // 
            this.txtServidorIp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtServidorIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtServidorIp.Location = new System.Drawing.Point(96, 35);
            this.txtServidorIp.Name = "txtServidorIp";
            this.txtServidorIp.Size = new System.Drawing.Size(142, 20);
            this.txtServidorIp.TabIndex = 1;
            // 
            // lblPuerto
            // 
            this.lblPuerto.AutoSize = true;
            this.lblPuerto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPuerto.Location = new System.Drawing.Point(289, 38);
            this.lblPuerto.Name = "lblPuerto";
            this.lblPuerto.Size = new System.Drawing.Size(42, 13);
            this.lblPuerto.TabIndex = 2;
            this.lblPuerto.Text = "Puerto";
            // 
            // lblServidorIp
            // 
            this.lblServidorIp.AutoSize = true;
            this.lblServidorIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblServidorIp.Location = new System.Drawing.Point(15, 38);
            this.lblServidorIp.Name = "lblServidorIp";
            this.lblServidorIp.Size = new System.Drawing.Size(65, 13);
            this.lblServidorIp.TabIndex = 0;
            this.lblServidorIp.Text = "Servidor IP";
            // 
            // grpAutenticacion
            // 
            this.grpAutenticacion.Controls.Add(this.lblClienteAutenticadoValor);
            this.grpAutenticacion.Controls.Add(this.lblClienteAutenticado);
            this.grpAutenticacion.Controls.Add(this.btnAutenticar);
            this.grpAutenticacion.Controls.Add(this.txtIdCliente);
            this.grpAutenticacion.Controls.Add(this.lblIdCliente);
            this.grpAutenticacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpAutenticacion.Location = new System.Drawing.Point(502, 94);
            this.grpAutenticacion.Name = "grpAutenticacion";
            this.grpAutenticacion.Size = new System.Drawing.Size(470, 120);
            this.grpAutenticacion.TabIndex = 2;
            this.grpAutenticacion.TabStop = false;
            this.grpAutenticacion.Text = "🔐 Autenticación del cliente";
            // 
            // lblClienteAutenticadoValor
            // 
            this.lblClienteAutenticadoValor.BackColor = System.Drawing.SystemColors.Window;
            this.lblClienteAutenticadoValor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblClienteAutenticadoValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblClienteAutenticadoValor.Location = new System.Drawing.Point(15, 81);
            this.lblClienteAutenticadoValor.Name = "lblClienteAutenticadoValor";
            this.lblClienteAutenticadoValor.Size = new System.Drawing.Size(437, 22);
            this.lblClienteAutenticadoValor.TabIndex = 4;
            this.lblClienteAutenticadoValor.Text = "Sin autenticar";
            this.lblClienteAutenticadoValor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClienteAutenticado
            // 
            this.lblClienteAutenticado.AutoSize = true;
            this.lblClienteAutenticado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblClienteAutenticado.Location = new System.Drawing.Point(15, 63);
            this.lblClienteAutenticado.Name = "lblClienteAutenticado";
            this.lblClienteAutenticado.Size = new System.Drawing.Size(119, 13);
            this.lblClienteAutenticado.TabIndex = 3;
            this.lblClienteAutenticado.Text = "Cliente autenticado";
            // 
            // btnAutenticar
            // 
            this.btnAutenticar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(200)))));
            this.btnAutenticar.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnAutenticar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAutenticar.Location = new System.Drawing.Point(317, 31);
            this.btnAutenticar.Name = "btnAutenticar";
            this.btnAutenticar.Size = new System.Drawing.Size(135, 26);
            this.btnAutenticar.TabIndex = 2;
            this.btnAutenticar.Text = "🔑 Autenticar";
            this.btnAutenticar.UseVisualStyleBackColor = false;
            this.btnAutenticar.Click += new System.EventHandler(this.btnAutenticar_Click);
            // 
            // txtIdCliente
            // 
            this.txtIdCliente.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIdCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtIdCliente.Location = new System.Drawing.Point(90, 34);
            this.txtIdCliente.Name = "txtIdCliente";
            this.txtIdCliente.Size = new System.Drawing.Size(142, 20);
            this.txtIdCliente.TabIndex = 1;
            // 
            // lblIdCliente
            // 
            this.lblIdCliente.AutoSize = true;
            this.lblIdCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblIdCliente.Location = new System.Drawing.Point(15, 37);
            this.lblIdCliente.Name = "lblIdCliente";
            this.lblIdCliente.Size = new System.Drawing.Size(58, 13);
            this.lblIdCliente.TabIndex = 0;
            this.lblIdCliente.Text = "Id Cliente";
            // 
            // grpEstado
            // 
            this.grpEstado.Controls.Add(this.lblResumenSesionValor);
            this.grpEstado.Controls.Add(this.lblResumenSesion);
            this.grpEstado.Controls.Add(this.lblEstadoConexionValor);
            this.grpEstado.Controls.Add(this.lblEstadoConexion);
            this.grpEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpEstado.Location = new System.Drawing.Point(12, 224);
            this.grpEstado.Name = "grpEstado";
            this.grpEstado.Size = new System.Drawing.Size(960, 90);
            this.grpEstado.TabIndex = 3;
            this.grpEstado.TabStop = false;
            this.grpEstado.Text = "📋 Estado de la sesión";
            // 
            // lblResumenSesionValor
            // 
            this.lblResumenSesionValor.BackColor = System.Drawing.SystemColors.Window;
            this.lblResumenSesionValor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblResumenSesionValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblResumenSesionValor.Location = new System.Drawing.Point(136, 52);
            this.lblResumenSesionValor.Name = "lblResumenSesionValor";
            this.lblResumenSesionValor.Size = new System.Drawing.Size(804, 22);
            this.lblResumenSesionValor.TabIndex = 3;
            this.lblResumenSesionValor.Text = "Sin sesión iniciada.";
            this.lblResumenSesionValor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblResumenSesion
            // 
            this.lblResumenSesion.AutoSize = true;
            this.lblResumenSesion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblResumenSesion.Location = new System.Drawing.Point(15, 57);
            this.lblResumenSesion.Name = "lblResumenSesion";
            this.lblResumenSesion.Size = new System.Drawing.Size(100, 13);
            this.lblResumenSesion.TabIndex = 2;
            this.lblResumenSesion.Text = "Resumen sesión";
            // 
            // lblEstadoConexionValor
            // 
            this.lblEstadoConexionValor.BackColor = System.Drawing.SystemColors.Window;
            this.lblEstadoConexionValor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblEstadoConexionValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblEstadoConexionValor.Location = new System.Drawing.Point(136, 23);
            this.lblEstadoConexionValor.Name = "lblEstadoConexionValor";
            this.lblEstadoConexionValor.Size = new System.Drawing.Size(180, 22);
            this.lblEstadoConexionValor.TabIndex = 1;
            this.lblEstadoConexionValor.Text = "Desconectado";
            this.lblEstadoConexionValor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEstadoConexion
            // 
            this.lblEstadoConexion.AutoSize = true;
            this.lblEstadoConexion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblEstadoConexion.Location = new System.Drawing.Point(15, 28);
            this.lblEstadoConexion.Name = "lblEstadoConexion";
            this.lblEstadoConexion.Size = new System.Drawing.Size(100, 13);
            this.lblEstadoConexion.TabIndex = 0;
            this.lblEstadoConexion.Text = "Estado conexión";
            // 
            // grpModulos
            // 
            this.grpModulos.Controls.Add(this.tlpModulos);
            this.grpModulos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpModulos.Location = new System.Drawing.Point(12, 324);
            this.grpModulos.Name = "grpModulos";
            this.grpModulos.Size = new System.Drawing.Size(960, 105);
            this.grpModulos.TabIndex = 4;
            this.grpModulos.TabStop = false;
            this.grpModulos.Text = "🧭 Módulos del cliente";
            // 
            // tlpModulos
            // 
            this.tlpModulos.ColumnCount = 4;
            this.tlpModulos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpModulos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpModulos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpModulos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpModulos.Controls.Add(this.btnConsultarSucursales, 0, 0);
            this.tlpModulos.Controls.Add(this.btnConsultarVehiculos, 1, 0);
            this.tlpModulos.Controls.Add(this.btnRegistrarVenta, 2, 0);
            this.tlpModulos.Controls.Add(this.btnConsultarVentas, 3, 0);
            this.tlpModulos.Location = new System.Drawing.Point(15, 28);
            this.tlpModulos.Name = "tlpModulos";
            this.tlpModulos.RowCount = 1;
            this.tlpModulos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpModulos.Size = new System.Drawing.Size(925, 58);
            this.tlpModulos.TabIndex = 0;
            // 
            // btnConsultarSucursales
            // 
            this.btnConsultarSucursales.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(225)))), ((int)(((byte)(245)))));
            this.btnConsultarSucursales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConsultarSucursales.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnConsultarSucursales.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnConsultarSucursales.Location = new System.Drawing.Point(3, 3);
            this.btnConsultarSucursales.Name = "btnConsultarSucursales";
            this.btnConsultarSucursales.Size = new System.Drawing.Size(225, 52);
            this.btnConsultarSucursales.TabIndex = 0;
            this.btnConsultarSucursales.Text = "🏢 Consultar\r\nSucursales";
            this.btnConsultarSucursales.UseVisualStyleBackColor = false;
            this.btnConsultarSucursales.Click += new System.EventHandler(this.btnConsultarSucursales_Click);
            // 
            // btnConsultarVehiculos
            // 
            this.btnConsultarVehiculos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(220)))));
            this.btnConsultarVehiculos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConsultarVehiculos.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnConsultarVehiculos.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnConsultarVehiculos.Location = new System.Drawing.Point(234, 3);
            this.btnConsultarVehiculos.Name = "btnConsultarVehiculos";
            this.btnConsultarVehiculos.Size = new System.Drawing.Size(225, 52);
            this.btnConsultarVehiculos.TabIndex = 1;
            this.btnConsultarVehiculos.Text = "🚘 Consultar\r\nVehículos";
            this.btnConsultarVehiculos.UseVisualStyleBackColor = false;
            this.btnConsultarVehiculos.Click += new System.EventHandler(this.btnConsultarVehiculos_Click);
            // 
            // btnRegistrarVenta
            // 
            this.btnRegistrarVenta.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(230)))), ((int)(((byte)(180)))));
            this.btnRegistrarVenta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRegistrarVenta.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnRegistrarVenta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnRegistrarVenta.Location = new System.Drawing.Point(465, 3);
            this.btnRegistrarVenta.Name = "btnRegistrarVenta";
            this.btnRegistrarVenta.Size = new System.Drawing.Size(225, 52);
            this.btnRegistrarVenta.TabIndex = 2;
            this.btnRegistrarVenta.Text = "💰 Registrar\r\nVenta";
            this.btnRegistrarVenta.UseVisualStyleBackColor = false;
            this.btnRegistrarVenta.Click += new System.EventHandler(this.btnRegistrarVenta_Click);
            // 
            // btnConsultarVentas
            // 
            this.btnConsultarVentas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(220)))), ((int)(((byte)(245)))));
            this.btnConsultarVentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnConsultarVentas.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnConsultarVentas.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnConsultarVentas.Location = new System.Drawing.Point(696, 3);
            this.btnConsultarVentas.Name = "btnConsultarVentas";
            this.btnConsultarVentas.Size = new System.Drawing.Size(226, 52);
            this.btnConsultarVentas.TabIndex = 3;
            this.btnConsultarVentas.Text = "🧾 Consultar\r\nMis Ventas";
            this.btnConsultarVentas.UseVisualStyleBackColor = false;
            this.btnConsultarVentas.Click += new System.EventHandler(this.btnConsultarVentas_Click);
            // 
            // grpBitacora
            // 
            this.grpBitacora.Controls.Add(this.txtBitacora);
            this.grpBitacora.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpBitacora.Location = new System.Drawing.Point(12, 439);
            this.grpBitacora.Name = "grpBitacora";
            this.grpBitacora.Size = new System.Drawing.Size(960, 220);
            this.grpBitacora.TabIndex = 5;
            this.grpBitacora.TabStop = false;
            this.grpBitacora.Text = "📝 Bitácora del cliente";
            // 
            // txtBitacora
            // 
            this.txtBitacora.BackColor = System.Drawing.SystemColors.Window;
            this.txtBitacora.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtBitacora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBitacora.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtBitacora.Location = new System.Drawing.Point(3, 16);
            this.txtBitacora.Multiline = true;
            this.txtBitacora.Name = "txtBitacora";
            this.txtBitacora.ReadOnly = true;
            this.txtBitacora.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBitacora.Size = new System.Drawing.Size(954, 201);
            this.txtBitacora.TabIndex = 0;
            // 
            // pnlAcciones
            // 
            this.pnlAcciones.Controls.Add(this.btnSalir);
            this.pnlAcciones.Controls.Add(this.btnLimpiarBitacora);
            this.pnlAcciones.Location = new System.Drawing.Point(12, 668);
            this.pnlAcciones.Name = "pnlAcciones";
            this.pnlAcciones.Size = new System.Drawing.Size(960, 38);
            this.pnlAcciones.TabIndex = 6;
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSalir.ForeColor = System.Drawing.Color.White;
            this.btnSalir.Location = new System.Drawing.Point(0, 6);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(140, 26);
            this.btnSalir.TabIndex = 0;
            this.btnSalir.Text = "❌ Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnLimpiarBitacora
            // 
            this.btnLimpiarBitacora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpiarBitacora.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(210)))), ((int)(((byte)(120)))));
            this.btnLimpiarBitacora.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.btnLimpiarBitacora.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnLimpiarBitacora.Location = new System.Drawing.Point(820, 6);
            this.btnLimpiarBitacora.Name = "btnLimpiarBitacora";
            this.btnLimpiarBitacora.Size = new System.Drawing.Size(140, 26);
            this.btnLimpiarBitacora.TabIndex = 1;
            this.btnLimpiarBitacora.Text = "🧹 Limpiar bitácora";
            this.btnLimpiarBitacora.UseVisualStyleBackColor = false;
            this.btnLimpiarBitacora.Click += new System.EventHandler(this.btnLimpiarBitacora_Click);
            // 
            // FrmClientePrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(984, 718);
            this.Controls.Add(this.pnlAcciones);
            this.Controls.Add(this.grpBitacora);
            this.Controls.Add(this.grpModulos);
            this.Controls.Add(this.grpEstado);
            this.Controls.Add(this.grpAutenticacion);
            this.Controls.Add(this.grpConexion);
            this.Controls.Add(this.pnlEncabezado);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmClientePrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoMarket - Cliente Principal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmClientePrincipal_FormClosing);
            this.pnlEncabezado.ResumeLayout(false);
            this.pnlEncabezado.PerformLayout();
            this.grpConexion.ResumeLayout(false);
            this.grpConexion.PerformLayout();
            this.grpAutenticacion.ResumeLayout(false);
            this.grpAutenticacion.PerformLayout();
            this.grpEstado.ResumeLayout(false);
            this.grpEstado.PerformLayout();
            this.grpModulos.ResumeLayout(false);
            this.tlpModulos.ResumeLayout(false);
            this.grpBitacora.ResumeLayout(false);
            this.grpBitacora.PerformLayout();
            this.pnlAcciones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTituloPrincipal;
        private System.Windows.Forms.GroupBox grpConexion;
        private System.Windows.Forms.Button btnProbarConexion;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.Button btnConectar;
        private System.Windows.Forms.TextBox txtPuerto;
        private System.Windows.Forms.TextBox txtServidorIp;
        private System.Windows.Forms.Label lblPuerto;
        private System.Windows.Forms.Label lblServidorIp;
        private System.Windows.Forms.GroupBox grpAutenticacion;
        private System.Windows.Forms.Label lblClienteAutenticadoValor;
        private System.Windows.Forms.Label lblClienteAutenticado;
        private System.Windows.Forms.Button btnAutenticar;
        private System.Windows.Forms.TextBox txtIdCliente;
        private System.Windows.Forms.Label lblIdCliente;
        private System.Windows.Forms.GroupBox grpEstado;
        private System.Windows.Forms.Label lblResumenSesionValor;
        private System.Windows.Forms.Label lblResumenSesion;
        private System.Windows.Forms.Label lblEstadoConexionValor;
        private System.Windows.Forms.Label lblEstadoConexion;
        private System.Windows.Forms.GroupBox grpModulos;
        private System.Windows.Forms.TableLayoutPanel tlpModulos;
        private System.Windows.Forms.Button btnConsultarSucursales;
        private System.Windows.Forms.Button btnConsultarVehiculos;
        private System.Windows.Forms.Button btnRegistrarVenta;
        private System.Windows.Forms.Button btnConsultarVentas;
        private System.Windows.Forms.GroupBox grpBitacora;
        private System.Windows.Forms.TextBox txtBitacora;
        private System.Windows.Forms.Panel pnlAcciones;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnLimpiarBitacora;
    }
}