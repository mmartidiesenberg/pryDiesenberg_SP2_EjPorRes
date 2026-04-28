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
                if (!File.Exists(ruta))
                {
                    Mostrar("ERROR: Base de datos no encontrada en: " + ruta);
                    return false;
                }

                cn = new OleDbConnection(
                @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ruta);

                cn.Open();
                Mostrar("Conectado a la base de datos.");
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
            try
            {
                if (cn != null && cn.State == System.Data.ConnectionState.Open)
                {
                    cn.Close();
                    cn.Dispose();
                    Mostrar("Conexión cerrada.");
                }
            }
            catch (Exception ex)
            {
                Mostrar("ERROR al cerrar: " + ex.Message);
            }
        }

        public void MigrarCategorias(string rutaTxt)
        {
            StreamReader sr = null;

            try
            {
                if (!File.Exists(rutaTxt))
                {
                    Mostrar("ERROR Categorías: No se pudo encontrar el archivo " + rutaTxt);
                    return;
                }

                Mostrar("Migrando datos de Categorías...");

                OleDbCommand borrar = new OleDbCommand(
                "DELETE FROM [Categorías]", cn);
                borrar.ExecuteNonQuery();

                int cont = 0;

                sr = new StreamReader(rutaTxt, Encoding.UTF8);

                while (!sr.EndOfStream)
                {
                    string linea = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(linea))
                        continue;

                    string[] datos = linea.Split(';');

                    if (datos.Length < 2)
                    {
                        Mostrar("ADVERTENCIA: Línea con formato incorrecto.");
                        continue;
                    }

                    try
                    {
                        OleDbCommand cmd = new OleDbCommand(
                        "INSERT INTO [Categorías] ([IdCategoría], [Nombre]) VALUES (@id,@nom)", cn);

                        cmd.Parameters.AddWithValue("@id", int.Parse(datos[0].Trim()));
                        cmd.Parameters.AddWithValue("@nom", datos[1].Trim());

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        cont++;
                    }
                    catch (Exception ex)
                    {
                        Mostrar("ERROR en línea: " + linea + " - " + ex.Message);
                    }
                }

                Mostrar("Se incorporaron " + cont + " registros nuevos.");
                Mostrar("");
            }
            catch (Exception ex)
            {
                Mostrar("ERROR Categorías: " + ex.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }

        public void MigrarArticulos(string rutaTxt)
        {
            StreamReader sr = null;

            try
            {
                if (!File.Exists(rutaTxt))
                {
                    Mostrar("ERROR Artículos: No se pudo encontrar el archivo " + rutaTxt);
                    return;
                }

                Mostrar("Migrando datos de Artículos...");

                OleDbCommand borrar = new OleDbCommand(
                "DELETE FROM [Artículos]", cn);
                borrar.ExecuteNonQuery();

                int cont = 0;

                sr = new StreamReader(rutaTxt, Encoding.UTF8);

                while (!sr.EndOfStream)
                {
                    string linea = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(linea))
                        continue;

                    string[] datos = linea.Split(';');

                    if (datos.Length < 4)
                    {
                        Mostrar("ADVERTENCIA: Línea con formato incorrecto.");
                        continue;
                    }

                    try
                    {
                        OleDbCommand cmd = new OleDbCommand(
                        "INSERT INTO [Artículos] ([IdArtículo], [Nombre], [IdCategoría], [Precio]) VALUES (@id,@nom,@cat,@pre)", cn);

                        cmd.Parameters.AddWithValue("@id", int.Parse(datos[0].Trim()));
                        cmd.Parameters.AddWithValue("@nom", datos[1].Trim());
                        cmd.Parameters.AddWithValue("@cat", int.Parse(datos[2].Trim()));
                        cmd.Parameters.AddWithValue("@pre", decimal.Parse(datos[3].Trim()));

                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                        cont++;
                    }
                    catch (Exception ex)
                    {
                        Mostrar("ERROR en línea: " + linea + " - " + ex.Message);
                    }
                }

                Mostrar("Se incorporaron " + cont + " registros nuevos.");
                Mostrar("");
            }
            catch (Exception ex)
            {
                Mostrar("ERROR Artículos: " + ex.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
        }
    }
}
