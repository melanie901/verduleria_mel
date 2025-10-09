using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace verduleria
{
   
        public partial class EliminarUsuario : Form
        {
            public EliminarUsuario()
            {
                InitializeComponent();

                // Conectamos el evento Click del botón
                btnEliminar.Click += btnEliminar_Click;

                // Cargamos los usuarios al abrir el formulario
                CargarUsuarios();
            }

            // Método para cargar los usuarios en el DataGridView
            private void CargarUsuarios()
            {
                try
                {
                    Conexion conexion = new Conexion();
                    using (MySqlConnection cn = conexion.GetConexion())
                    {
                        cn.Open();
                        string query = "SELECT idUser, Nombre, user, Password, t.descripcion AS tipoUsuario " +
                                       "FROM usuario u JOIN tipo_usuarios t ON u.idTipoUser = t.idTipoUser";

                        MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        dataGridViewUsuarios.DataSource = dt;

                        // Mejor presentación
                        dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridViewUsuarios.ReadOnly = true;
                        dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los usuarios: " + ex.Message);
                }
            }

            // Evento Click del botón Eliminar
            private void btnEliminar_Click(object sender, EventArgs e)
            {
                if (dataGridViewUsuarios.SelectedRows.Count > 0)
                {
                    int idUser = Convert.ToInt32(dataGridViewUsuarios.SelectedRows[0].Cells["idUser"].Value);

                    DialogResult respuesta = MessageBox.Show(
                        "¿Estás seguro de que querés eliminar este usuario?",
                        "Confirmar eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (respuesta == DialogResult.Yes)
                    {
                        try
                        {
                            Conexion conexion = new Conexion();
                            using (MySqlConnection cn = conexion.GetConexion())
                            {
                                cn.Open();
                                string query = "DELETE FROM usuario WHERE idUser = @idUser";

                                MySqlCommand cmd = new MySqlCommand(query, cn);
                                cmd.Parameters.AddWithValue("@idUser", idUser);
                                cmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Usuario eliminado correctamente.");

                            // Recargar el DataGridView
                            CargarUsuarios();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar el usuario: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seleccioná un usuario primero.");
                }
            }
        
    
        private void EliminarUsuario_Load_1(object sender, EventArgs e)
        {

        }

        private void dataGridViewUsuarios_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {


            // Crear instancia del formulario de registro
            registro_de_usuarios registro = new registro_de_usuarios();
            registro.Show();

            // Cerrar este formulario
            this.Close();
        
    }

}
}
 
