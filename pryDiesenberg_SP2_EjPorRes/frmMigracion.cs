using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            txtInformacion.Clear();

            Migracion m = new Migracion();

            m.OnMensaje += x =>
            {
                txtInformacion.AppendText(x + Environment.NewLine);
            };

            if (m.Conectar(Application.StartupPath + @"\Distribuidor.mdb"))
            {
                m.MigrarCategorias(@"E:\Escritorio\Categorias.txt");
                m.MigrarArticulos(@"E:\Escritorio\Articulos.txt");

                m.Cerrar();

                txtInformacion.AppendText("Migración finalizada.");
            }
        }
    }
}
