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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
       
        {
            try
            {
             
                string usuario = txtUsuario.Text;
                string pass = txtPassword.Text;

           
                Usuario u = new Usuario();
                u.User = usuario;
                u.Password = pass;
                
                // Validamos
                ControlLogin control = new ControlLogin();  
                Usuario resultado = control.usuarioValido(u);
                if (resultado != null)
                {
                    MessageBox.Show("¡Bienvenido!", "Inicio de Sesión",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FrmPrincipal p = new FrmPrincipal();
                    this.Hide();
                    p.Show();
                }
                else
                {
                    MessageBox.Show("Usuario/Contraseña inválido", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsuario.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult resp = MessageBox.Show("Cerrar sistema,¿confirma ? ", "Verduleria", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
            if (resp == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            txtUsuario.Text = "";
            txtPassword.Text = "";
            txtUsuario.Focus();
        }

        private void btnRegistar_Click(object sender, EventArgs e)
        {
            registro_de_usuarios formularioRegistro = new registro_de_usuarios();

            formularioRegistro.ShowDialog(); 
        }
    }
}
