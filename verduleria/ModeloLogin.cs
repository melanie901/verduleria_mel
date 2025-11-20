using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace verduleria
{
    internal class ModeloLogin
    {
            private MySqlConnection miConexion;
            private string sql;
            private MySqlCommand comando;
            private MySqlDataReader reader;

            public Usuario buscarUsuario(Usuario u)
            {
                Usuario usuarioResultante = null;
                Conexion c = new Conexion();
                miConexion = c.GetConexion();
                miConexion.Open();

                sql = "SELECT * FROM usuario WHERE User = @user";
                comando = new MySqlCommand(sql, miConexion);
                comando.Parameters.AddWithValue("@user", u.User);

                reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        usuarioResultante = new Usuario();
                        usuarioResultante.IdUser = int.Parse(reader["idUser"].ToString());
                        usuarioResultante.User = reader["User"].ToString();
                        usuarioResultante.Password = reader["Password"].ToString();
                        usuarioResultante.Nombre = reader["Nombre"].ToString();
                        usuarioResultante.IdTipoUser = int.Parse(reader["idTipoUser"].ToString());
                    }
                }
                miConexion.Close();
                return usuarioResultante;
            }
        }
    }


