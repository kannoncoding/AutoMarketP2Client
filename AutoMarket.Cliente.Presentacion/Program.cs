/*
Universidad: UNED
Cuatrimestre: I Cuatrimestre 2026
Proyecto: AutoMarket - Proyecto #2
Descripción: Punto de entrada de la aplicación cliente AutoMarket. Inicializa la configuración visual y arranca el formulario principal.
Estudiante: Jorge Arias
Fecha de desarrollo: 2026-04-12
*/

using System;
using System.Windows.Forms;

namespace AutoMarket.Cliente.Presentacion
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal de la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            IniciarAplicacion();
        }

        private static void IniciarAplicacion()
        {
            try
            {
                Application.Run(new FrmClientePrincipal());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error crítico al iniciar la aplicación:\n\n" + ex.Message,
                    "Error de inicio",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}