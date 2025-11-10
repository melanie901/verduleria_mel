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

            Conexion conexion = new Conexion();
            MySqlConnection c = conexion.GetConexion();
            {
                c.Open();
                string query = "SELECT id_proveedor, nombre FROM proveedores";
                MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbProveedor.DataSource = dt;
                cmbProveedor.DisplayMember = "nombre";
                cmbProveedor.ValueMember = "id_proveedor";
            }
            c.Close();
        }
        private void CargarProductos()
        {
            Conexion conexion = new Conexion();
            MySqlConnection c = conexion.GetConexion();
            {
                c.Open();
                string query = "SELECT id_producto, nombre FROM productos";
                MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbProducto.DataSource = dt;
                cmbProducto.DisplayMember = "nombre";
                cmbProducto.ValueMember = "id_producto";
            }
            c.Close();
        }
        private void CargarPedidos()
        {
            Conexion conexion = new Conexion();
            MySqlConnection c = conexion.GetConexion();
            {
                c.Open();
                string query = @"SELECT p.id_pedido, pr.nombre AS proveedor, pro.nombre AS producto, 
                                 p.cantidad, p.fecha_pedido
                                 FROM pedidos_proveedor p
                                 JOIN proveedores pr ON p.id_proveedor = pr.id_proveedor
                                 JOIN productos pro ON p.id_producto = pro.id_producto";
                MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPedidos.DataSource = dt;
            }
            c.Close();
        }
        private void btnRegistrar_Click_1(object sender, EventArgs e)
        {
                try
                {
                    Conexion conexion = new Conexion();
                    MySqlConnection c = conexion.GetConexion();
                    c.Open();
                    string query = "INSERT INTO pedidos_proveedor (id_pedido, id_proveedor, id_producto, cantidad, fecha_pedido) VALUES (@pedido, @prov, @prod, @cant, @fecha)";
                    MySqlCommand cmd = new MySqlCommand(query, c);
                    cmd.Parameters.AddWithValue("@pedido", null);
                    cmd.Parameters.AddWithValue("@prov", cmbProveedor.SelectedValue);
                    cmd.Parameters.AddWithValue("@prod", cmbProducto.SelectedValue);
                    cmd.Parameters.AddWithValue("@cant", numCantidad.Value);
                //string fecha = DateTime.Now.ToString("yyyy-MM-dd");
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);//fecha);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Pedido registrado correctamente ✅");
                    CargarPedidos();
                c.Close();
            }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al registrar pedido: " + ex.Message);
                }
             
        }

        private void dgvPedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
