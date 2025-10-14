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
        private Conexion miConexion;
        private MySqlConnection conectar;
        private String sql = "";
        private MySqlCommand comando;
        private MySqlDataReader reader;

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

        public bool existeUsuario(Usuario u)
        {
            bool rta = false;
            miConexion = new Conexion();
            conectar = miConexion.GetConexion();
            conectar.Open();
            sql = "Select * from usuario where User Like @user";
            comando = new MySqlCommand(sql, conectar);
            comando.Parameters.AddWithValue("@User", u.User);
            reader = comando.ExecuteReader();
            if (reader.HasRows)
                rta = true;
            conectar.Close();
            return rta;
        }

        public bool registrarUsuario(Usuario u)
            {
                try
                {
                    miConexion = new Conexion();
                    conectar = miConexion.GetConexion();
                    conectar.Open();

                    string consulta = "INSERT INTO usuario (idUser, Nombre, User, Password, idTipoUser) VALUES (@id, @nombre, @user, @pass, @tipo)";
                    MySqlCommand cmd = new MySqlCommand(consulta, conectar);
                    cmd.Parameters.AddWithValue("@id", null);
                    cmd.Parameters.AddWithValue("@nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@user", u.User);
                    cmd.Parameters.AddWithValue("@pass", u.Password);
                    cmd.Parameters.AddWithValue("@tipo", u.IdTipoUser);

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