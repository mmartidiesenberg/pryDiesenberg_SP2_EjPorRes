using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pryDiesenberg_SP2_EjPorRes
{
    public class Articulo
    {
        public int IdArticulo { get; set; }
        public string Nombre { get; set; }
        public int IdCategoria { get; set; }
        public double Precio { get; set; }
    }
}
