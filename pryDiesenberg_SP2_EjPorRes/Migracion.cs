using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;

namespace pryDiesenberg_SP2_EjPorRes
{
    public class Migracion
    {
        private OleDbConnection cn;

        public event Action<string> OnMensaje;

        private void Mostrar(string texto)
        {
            OnMensaje?.Invoke(texto);
        }

        public bool Conectar(string ruta)
        {
            try
            {
                cn = new OleDbConnection(
                @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ruta);

                cn.Open();
                return true;
            }
            catch (Exception ex)
            {
                Mostrar("ERROR al conectar: " + ex.Message);
                return false;
            }
        }

        public void Cerrar()
        {
            if (cn != null)
                cn.Close();
        }

        public void MigrarCategorias(string rutaTxt)
        {
            try
            {
                Mostrar("Migrando datos de Categorias...");

                OleDbCommand borrar = new OleDbCommand(
                "DELETE FROM [Categorías]", cn);
                borrar.ExecuteNonQuery();

                int cont = 0;

                StreamReader sr = new StreamReader(rutaTxt);

                while (!sr.EndOfStream)
                {
                    string linea = sr.ReadLine();
                    string[] datos = linea.Split(';');

                    OleDbCommand cmd = new OleDbCommand(
                    "INSERT INTO [Categorías] ([IdCategoría], [Nombre]) VALUES (@id,@nom)", cn);

                    cmd.Parameters.AddWithValue("@id", datos[0]);
                    cmd.Parameters.AddWithValue("@nom", datos[1]);

                    cmd.ExecuteNonQuery();
                    cont++;
                }

                sr.Close();

                Mostrar("Se incorporaron " + cont + " registros nuevos.");
                Mostrar("");
            }
            catch (Exception ex)
            {
                Mostrar("ERROR Categorias: " + ex.Message);
            }
        }

        public void MigrarArticulos(string rutaTxt)
        {
            try
            {
                Mostrar("Migrando datos de Artículos...");

                OleDbCommand borrar = new OleDbCommand(
                "DELETE FROM [Artículos]", cn);
                borrar.ExecuteNonQuery();

                int cont = 0;

                StreamReader sr = new StreamReader(rutaTxt);

                while (!sr.EndOfStream)
                {
                    string linea = sr.ReadLine();
                    string[] datos = linea.Split(';');

                    OleDbCommand cmd = new OleDbCommand(
                    "INSERT INTO [Artículos] ([IdArtículo], [Nombre], [IdCategoría], [Precio]) VALUES (@id,@nom,@cat,@pre)", cn);

                    cmd.Parameters.AddWithValue("@id", datos[0]);
                    cmd.Parameters.AddWithValue("@nom", datos[1]);
                    cmd.Parameters.AddWithValue("@cat", datos[2]);
                    cmd.Parameters.AddWithValue("@pre", datos[3]);

                    cmd.ExecuteNonQuery();
                    cont++;
                }

                sr.Close();

                Mostrar("Se incorporaron " + cont + " registros nuevos.");
                Mostrar("");
            }
            catch (Exception ex)
            {
                Mostrar("ERROR Articulos: " + ex.Message);
            }
        }
    }
}
