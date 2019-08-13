using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPPWindowsFormsAFIP
{
    class MetodosBD
    {



        public static void modificarComprobante(int id, int estado, string respuesta)
        {

            try
            {

                string sql1 = "UPDATE afip_comprobante SET comp_estado = " + estado + ", comp_numero = " + MetodosGenerales.numero_comprobante + ", comp_cae =\'" + MetodosGenerales.cae_comprobante;
                if (MetodosGenerales.cae_vencimiento != "")
                    sql1 += "\', cae_vencimiento =\'" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", MetodosGenerales.cae_vencimiento);
                sql1 += "\', comp_mensaje_obs =\'" + MetodosGenerales.mensaje_obs +
                "\', comp_mensaje_error =\'" + MetodosGenerales.mensaje_error +
                "\', comp_mensaje_afip =\'" + MetodosGenerales.mensaje_afip +
                "\', consulta_afip =\'" + respuesta +
                "\' where id = " + id;

                //MessageBox.Show(sql1);
                NpgsqlCommand Command = new NpgsqlCommand(sql1, Main.connection);
                int n = Command.ExecuteNonQuery();
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - COMPROBANTE REGISTRADO: " + respuesta);
                //MessageBox.Show(n.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR Updtae BD: " + ex.Message + " - " + ex.StackTrace);
                //MessageBox.Show("There is a error: " + ex.Message);
            }
        }



        public static void insertarLogin(string token, string sing, DateTime generacion, DateTime expiration)
        {

            try
            {
                String valor = "\'TEST\'";
                string sql1 = "INSERT INTO afip_login (afip_token,afip_sing,afip_generacion,afip_expiration) VALUES (\'"
                    + token + "\',\'" + sing + "\',\'" + String.Format("{0:dd/MM/yyyy HH:mm:ss}", generacion.ToString()) + "\',\'" +
                    String.Format("{0:dd/MM/yyyy HH:mm:ss}", expiration.ToString()) + "\')";

                //MessageBox.Show(sql1);
                NpgsqlCommand Command = new NpgsqlCommand(sql1, Main.connection);
                int n = Command.ExecuteNonQuery();
                Main.EXPIRATION = expiration;
                //MessageBox.Show(n.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR Insert BD: " + ex.Message + " - " + ex.StackTrace);
                //MessageBox.Show("there is a error: " + ex.Message);
            }
        }



        public static NpgsqlConnection ConnectRemote(String ApplicationName, String Host, int Port, String Username, String Password, String Database, bool Pooling)
        {

            NpgsqlConnectionStringBuilder sb = new NpgsqlConnectionStringBuilder();
            sb.ApplicationName = ApplicationName;
            sb.Host = Host;//remoteData.server;
            sb.Port = Port;//remoteData.port;
            sb.Username = Username;// remoteData.user;
            sb.Password = Password;// remoteData.password;
            sb.Database = Database;// remoteData.database;
            sb.Pooling = Pooling;

            NpgsqlConnection connection = new NpgsqlConnection(sb.ToString());

            try
            {
                connection.Open();

            }
            catch (NpgsqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                connection.Close();
                LogHelper.Log(LogTarget.File, DateTime.Now.ToString() + " - ERROR BD: " + ex.Message + " - " + ex.StackTrace);
                //MessageBox.Show("Error de conexion con BD. " + ex.Message);
                connection = null;
            }
            return connection;
        }

    }
}
