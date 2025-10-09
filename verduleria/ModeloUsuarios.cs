using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace verduleria
{
    public class ModeloUsuarios
    {
            Conexion miConexion;
            MySqlConnection conectar;

            public DataTable obtenerTipos()
            {
                try
                {
                    miConexion = new Conexion();
                    conectar = miConexion.GetConexion();
                    conectar.Open();

                    string consulta = "SELECT * FROM tipo_usuarios";
                    MySqlCommand comando = new MySqlCommand(consulta, conectar);
                    MySqlDataAdapter mysqldt = new MySqlDataAdapter(comando);
                    DataTable dt = new DataTable();
                    mysqldt.Fill(dt);

                    conectar.Close();
                    return dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener tipos de usuario: " + ex.Message);
                    return new DataTable();
                }
            }

    
            public bool registrarUsuario(string nombreApellido, string user, string password, int idTipoUser)
            {
                try
                {
                    miConexion = new Conexion();
                    conectar = miConexion.GetConexion();
                    conectar.Open();

                    string consulta = "INSERT INTO usuario (Nombre, User, Password, idTipoUser) VALUES (@nombre, @user, @pass, @tipo)";
                    MySqlCommand cmd = new MySqlCommand(consulta, conectar);
                    cmd.Parameters.AddWithValue("@nombre", nombreApellido);
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.Parameters.AddWithValue("@pass", password);
                    cmd.Parameters.AddWithValue("@tipo", idTipoUser);

                    int filas = cmd.ExecuteNonQuery();
                    conectar.Close();

                    return filas > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar usuario: " + ex.Message);
                    return false;
                }
            }
        }
    }