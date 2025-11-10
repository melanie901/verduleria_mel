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
    public partial class FmrProveedores : Form
    {
        public FmrProveedores()
        {
            InitializeComponent();
        }

        private void FmrProveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
            CargarProductos();
            CargarPedidos();
        }
        private void CargarProveedores()
        {

            using (MySqlConnection conexion = new MySqlConnection("server=localhost;database=verduleria;uid=root;pwd=amatista;"))
            {
                conexion.Open();
                string query = "SELECT id_proveedor, nombre FROM proveedores";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbProveedor.DataSource = dt;
                cmbProveedor.DisplayMember = "nombre";
                cmbProveedor.ValueMember = "id_proveedor";
            }
        }
        private void CargarProductos()
        {
            using (MySqlConnection conexion = new MySqlConnection("server=localhost;database=verduleria;uid=root;pwd=amatista;"))
            {
                conexion.Open();
                string query = "SELECT id_producto, nombre FROM productos";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbProducto.DataSource = dt;
                cmbProducto.DisplayMember = "nombre";
                cmbProducto.ValueMember = "id_producto";
            }
        }
        private void CargarPedidos()
        {
            using (MySqlConnection conexion = new MySqlConnection("server=localhost;database=verduleria;uid=root;pwd=amatista;"))
            {
                conexion.Open();
                string query = @"SELECT p.id_pedido, pr.nombre AS proveedor, pro.nombre AS producto, 
                                 p.cantidad, p.fecha_pedido
                                 FROM pedidos_proveedor p
                                 JOIN proveedores pr ON p.id_proveedor = pr.id_proveedor
                                 JOIN productos pro ON p.id_producto = pro.id_producto";
                MySqlDataAdapter da = new MySqlDataAdapter(query, conexion);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPedidos.DataSource = dt;
            }
        }

       

        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {
            using (MySqlConnection conexion = new MySqlConnection("server=localhost;database=verduleria;uid=root;pwd=amatista;"))
            {
                try
                {
                    conexion.Open();
                  
                    string query = "INSERT INTO pedidos_proveedor (id_proveedor, id_producto, cantidad, fecha_pedido) VALUES (@prov, @prod, @cant, @fecha, @estado)";
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@idproveedor", cmbProveedor.SelectedValue);
                    cmd.Parameters.AddWithValue("@idproducto", cmbProducto.SelectedValue);
                    cmd.Parameters.AddWithValue("@cantidad", numCantidad.Value);
                    cmd.Parameters.AddWithValue("@fecha_pedido", DateTime.Now);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Pedido registrado correctamente ✅");
                    CargarPedidos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar pedido: " + ex.Message);
                }
            }

        }

        private void dgvPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
