using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pryDiesenberg_SP2_EjPorRes
{
    public partial class frmMigracion : Form
    {
        public frmMigracion()
        {
            InitializeComponent();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            try
            {
                txtInformacion.Clear();

                // Application.StartupPath = C:\...\bin\Debug
                // Necesitamos ir 3 niveles arriba para llegar a la raíz del proyecto
                string rutaDebug = Application.StartupPath; // bin\Debug
                string rutaBin = Directory.GetParent(rutaDebug).FullName; // bin
                string rutaProyecto = Directory.GetParent(rutaBin).FullName; // raíz del proyecto

                // Rutas de los archivos
                string rutaCategorias = Path.Combine(rutaProyecto, "Categorias.txt");
                string rutaArticulos = Path.Combine(rutaProyecto, "Articulos.txt");
                string rutaBaseDatos = Path.Combine(rutaDebug, "Distribuidor.mdb");

                txtInformacion.AppendText("Buscando archivos en:" + Environment.NewLine);
                txtInformacion.AppendText("Proyecto: " + rutaProyecto + Environment.NewLine);
                txtInformacion.AppendText(Environment.NewLine);

                // Validar que los archivos existan
                if (!File.Exists(rutaCategorias))
                {
                    txtInformacion.AppendText($"ERROR: No se encontró Categorias.txt" + Environment.NewLine);
                    txtInformacion.AppendText($"Buscando en: {rutaCategorias}" + Environment.NewLine);
                    return;
                }

                if (!File.Exists(rutaArticulos))
                {
                    txtInformacion.AppendText($"ERROR: No se encontró Articulos.txt" + Environment.NewLine);
                    txtInformacion.AppendText($"Buscando en: {rutaArticulos}" + Environment.NewLine);
                    return;
                }

                if (!File.Exists(rutaBaseDatos))
                {
                    txtInformacion.AppendText($"ERROR: No se encontró Distribuidor.mdb" + Environment.NewLine);
                    txtInformacion.AppendText($"Buscando en: {rutaBaseDatos}" + Environment.NewLine);
                    return;
                }

                txtInformacion.AppendText("✓ Archivos encontrados correctamente." + Environment.NewLine);
                txtInformacion.AppendText(Environment.NewLine);

                Migracion m = new Migracion();

                m.OnMensaje += x =>
                {
                    txtInformacion.AppendText(x + Environment.NewLine);
                };

                if (m.Conectar(rutaBaseDatos))
                {
                    m.MigrarCategorias(rutaCategorias);
                    m.MigrarArticulos(rutaArticulos);

                    m.Cerrar();

                    txtInformacion.AppendText(Environment.NewLine + "✓ Migración finalizada correctamente." + Environment.NewLine);
                }
                else
                {
                    txtInformacion.AppendText("ERROR: No se pudo conectar a la base de datos." + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                txtInformacion.AppendText($"ERROR inesperado: {ex.Message}" + Environment.NewLine);
            }
        }

        private void frmMigracion_Load(object sender, EventArgs e)
        {

        }
    }
}
