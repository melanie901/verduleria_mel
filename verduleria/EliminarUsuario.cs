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
using Microsoft.VisualBasic;
namespace verduleria
{
   
        public partial class EliminarUsuario : Form
        {
        private ModeloUsuarios modelo = new ModeloUsuarios(); // 👈 usamos el modelo

        public EliminarUsuario()
        {
            InitializeComponent();

            btnEliminar.Click += btnEliminar_Click;
            btnVolver.Click += btnVolver_Click;

            // 🟢 (Como dice el profe)
            // Cargamos los datos en un método privado
            // ejecutado en el constructor
            cargarUsuarios();
        }

        // 🔹 Servicio privado para llenar el DataGridView
        private void cargarUsuarios()
        {
            try
            {
                Conexion conexion = new Conexion();
                using (MySqlConnection cn = conexion.GetConexion())
                {
                    cn.Open();
                    string query = @"SELECT 
                                        u.idUser, 
                                        u.Nombre, 
                                        u.User, 
                                        t.descripcion AS TipoUsuario 
                                     FROM usuario u 
                                     JOIN tipo_usuarios t ON u.idTipoUser = t.idTipoUser";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewUsuarios.DataSource = dt;

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

        // 🔹 Botón Eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewUsuarios.SelectedRows.Count > 0)
            {
                int idUser = Convert.ToInt32(dataGridViewUsuarios.SelectedRows[0].Cells["idUser"].Value);

                // 🔒 Solicitar contraseña antes de permitir borrar
                string contrasena = Microsoft.VisualBasic.Interaction.InputBox(
                    "Ingrese la contraseña de administrador para confirmar:",
                    "Autenticación requerida"
                );

                if (contrasena != "admin123") // 🔐 podés cambiar esta contraseña
                {
                    MessageBox.Show("Contraseña incorrecta ❌");
                    return;
                }

                DialogResult respuesta = MessageBox.Show(
                    "¿Estás seguro de que querés eliminar este usuario?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (respuesta == DialogResult.Yes)
                {
                    bool eliminado = modelo.eliminarUsuario(idUser); // 👈 usamos el modelo

                    if (eliminado)
                    {
                        MessageBox.Show("Usuario eliminado correctamente ✅");
                        cargarUsuarios(); // refrescamos la tabla
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el usuario ❌");
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccioná un usuario primero.");
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            registro_de_usuarios registro = new registro_de_usuarios();
            registro.Show();
            this.Close();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {

        }
    }
}