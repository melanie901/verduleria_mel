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
        public bool eliminarUsuario(int idUser)
        {
            try
            {
                miConexion = new Conexion();
                conectar = miConexion.GetConexion();
                conectar.Open();

                string consulta = "DELETE FROM usuario WHERE idUser = @idUser";
                MySqlCommand cmd = new MySqlCommand(consulta, conectar);
                cmd.Parameters.AddWithValue("@idUser", idUser);

                int filas = cmd.ExecuteNonQuery();
                conectar.Close();

                return filas > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar usuario: " + ex.Message);
                return false;
            }
        } // 👈 este cierra el método nuevo

        public DataTable obtenerUsuariosConTipo()
        {
            DataTable dt = new DataTable();
            try
            {
                miConexion = new Conexion();
                conectar = miConexion.GetConexion();
                conectar.Open();

                string consulta = @"SELECT 
                                        u.idUser AS ID,
                                        u.Nombre AS Nombre,
                                        u.User AS Usuario,
                                        t.descripcion AS TipoUsuario
                                    FROM usuario u
                                    INNER JOIN tipo_usuarios t ON u.idTipoUser = t.idTipoUser";

                MySqlCommand comando = new MySqlCommand(consulta, conectar);
                MySqlDataAdapter adapter = new MySqlDataAdapter(comando);
                adapter.Fill(dt);
                conectar.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los usuarios: " + ex.Message);
            }
            return dt;
        } 

    }
}