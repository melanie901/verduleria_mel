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

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreApellido = txtNombreApellido.Text.Trim();
                string user = txtUser.Text.Trim();
                string pass = txtPassword.Text.Trim();
                string confirmar = txtConfirmarPassword.Text.Trim();

                
                if (nombreApellido == "" || user == "" || pass == "" || confirmar == "")
                {
                    MessageBox.Show("Por favor, complete todos los campos.");
                    return;
                }

                if (pass != confirmar)
                {
                    MessageBox.Show("Las contraseñas no coinciden.");
                    return;
                }

                if (comboTipoUser.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione un tipo de usuario.");
                    return;
                }

                int idTipoUser = Convert.ToInt32(comboTipoUser.SelectedValue);

                bool exito = modelo.registrarUsuario(nombreApellido, user, pass, idTipoUser);

                if (exito)
                {
                    MessageBox.Show("Usuario registrado correctamente ✅");

                   
                    txtNombreApellido.Clear();
                    txtUser.Clear();
                    txtPassword.Clear();
                    txtConfirmarPassword.Clear();
                    comboTipoUser.SelectedIndex = -1;
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
        }

        private void registro_de_usuarios_Load(object sender, EventArgs e)
        {

        }
    }
}

