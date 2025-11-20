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
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            string clave = Microsoft.VisualBasic.Interaction.InputBox("ingrese la contraseña:", "acceso restringido", "");
            if (clave == "1234")
            {
                FmrProveedores fmr = new FmrProveedores();
                fmr.Show();
            }
            else if(clave !="")
            {
                MessageBox.Show("contraseña incorrecta", "acceso denegado",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
                private void btnProductos_Click(object sender, EventArgs e)
                {
                    FrmProductos fmr = new FrmProductos();
                    fmr.Show();
                }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnTienda_Click(object sender, EventArgs e)

        {

             FrmTienda fmr= new FrmTienda ();
            fmr.ShowDialog();
        }
    }
}
