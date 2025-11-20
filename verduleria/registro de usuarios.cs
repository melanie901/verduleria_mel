using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace verduleria
{
    public partial class registro_de_usuarios : Form
    {

        ModeloUsuarios modelo = new ModeloUsuarios();

        public registro_de_usuarios()
        {
            InitializeComponent();

            btnRegistrar.Click += btnRegistrar_Click;
            btnCancelar.Click += btnCancelar_Click;

            cargarTiposDeUsuario();
            cargarUsuarios();
        }

        private void cargarTiposDeUsuario()
        {
            try
            {
                DataTable dtTipos = modelo.obtenerTipos();
                comboTipoUser.DataSource = dtTipos;
                comboTipoUser.DisplayMember = "descripcion"; 
                comboTipoUser.ValueMember = "idTipoUser";
                comboTipoUser.SelectedIndex = -1; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar tipos de usuario: " + ex.Message);
            }
        }
        private void cargarUsuarios()
        {
            try
            {
                DataTable dtUsuarios = modelo.obtenerUsuariosConTipo();
                dataGridViewUsuarios.DataSource = dtUsuarios;
                dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridViewUsuarios.ReadOnly = true;
                dataGridViewUsuarios.AllowUserToAddRows = false;
                dataGridViewUsuarios.AllowUserToDeleteRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los usuarios: " + ex.Message);
            }
        }
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreApellido = txtNombreApellido.Text.Trim();
                string user = txtUser.Text.Trim();
                string pass = txtPassword.Text.Trim();
                string confirmar = txtConfirmarPassword.Text.Trim();

                Usuario usuarioRegistrar = new Usuario();
                usuarioRegistrar.Nombre = nombreApellido;
                usuarioRegistrar.User = user;
                usuarioRegistrar.Password = pass;
                usuarioRegistrar.PasswordConfirma = confirmar;
                usuarioRegistrar.IdTipoUser = int.Parse(comboTipoUser.SelectedValue.ToString());

                ControlUsuario c = new ControlUsuario();

                bool exito = c.ControlRegistroUsuarios(usuarioRegistrar);

                if (exito)
                {
                    MessageBox.Show("Usuario registrado correctamente ✅");
                    txtNombreApellido.Clear();
                    txtUser.Clear();
                    txtPassword.Clear();
                    txtConfirmarPassword.Clear();
                    comboTipoUser.SelectedIndex = -1;
                    cargarUsuarios(); // recarga la tabla
                }
                else
                {
                    MessageBox.Show("Error al registrar el usuario ❌");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            EliminarUsuario formEliminar = new EliminarUsuario();
            formEliminar.ShowDialog();
            cargarUsuarios();
        }

        private void registro_de_usuarios_Load(object sender, EventArgs e)
        {

        }

        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridViewUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {

        }
    }
}

