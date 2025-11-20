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
           
            // CARRITO DE PROVEEDORES
         
            List<CarritoProv> carritoProv = new List<CarritoProv>();

            public class CarritoProv
            {
                public int IdProveedor { get; set; }
                public int IdProducto { get; set; }
                public string Proveedor { get; set; }
                public string Producto { get; set; }
                public int Cantidad { get; set; }
                public decimal PrecioUnitario { get; set; }
                public decimal Subtotal => Cantidad * PrecioUnitario;
            }
           


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

                c.Open();
                string query = "SELECT id_proveedor, nombre FROM proveedores";
                MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbProveedor.DataSource = dt;
                cmbProveedor.DisplayMember = "nombre";
                cmbProveedor.ValueMember = "id_proveedor";
                c.Close();
            }

            private void CargarProductos()
            {
                Conexion conexion = new Conexion();
                MySqlConnection c = conexion.GetConexion();

                c.Open();
                string query = "SELECT id_producto, nombre, precio FROM productos";
                MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbProducto.DataSource = dt;
                cmbProducto.DisplayMember = "nombre";
                cmbProducto.ValueMember = "id_producto";
                c.Close();
            }

            private void CargarPedidos()
            {
                Conexion conexion = new Conexion();
                MySqlConnection c = conexion.GetConexion();

                c.Open();
                string query = @"
                SELECT p.id_pedido,
                       pr.nombre AS proveedor,
                       pro.nombre AS producto,
                       p.cantidad,
                       p.fecha_pedido
                FROM pedidos_proveedor p
                JOIN proveedores pr ON p.id_proveedor = pr.id_proveedor
                JOIN productos pro ON p.id_producto = pro.id_producto";

                MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPedidos.DataSource = dt;
                c.Close();
            }


           
            // AGREGAR AL CARRITO DE PROVEEDOR
          
            private void btnAgregarCarritoProv_Click_1(object sender, EventArgs e)
            {
                if (cmbProducto.SelectedValue == null || cmbProveedor.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione proveedor y producto.");
                    return;
                }

                // Datos
                int idProv = (int)cmbProveedor.SelectedValue;
                int idProd = (int)cmbProducto.SelectedValue;
                string proveedor = cmbProveedor.Text;
                string producto = cmbProducto.Text;
                int cantidad = (int)numCantidad.Value;
                decimal precioUnitario = ObtenerPrecioProducto(idProd);

                carritoProv.Add(new CarritoProv
                {
                    IdProveedor = idProv,
                    IdProducto = idProd,
                    Proveedor = proveedor,
                    Producto = producto,
                    Cantidad = cantidad,
                    PrecioUnitario = precioUnitario
                });

                ActualizarCarritoProv();
            }


       
            //  ACTUALIZAR DGV DEL CARRITO
         
            private void ActualizarCarritoProv()
            {
                dgvCarritoProv.DataSource = null;
                dgvCarritoProv.DataSource = carritoProv;
            }


            //OBTENER PRECIO DE PRODUCTO
           
            private decimal ObtenerPrecioProducto(int idProd)
            {
                Conexion conexion = new Conexion();
                MySqlConnection c = conexion.GetConexion();
                c.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT precio FROM productos WHERE id_producto=@id", c);
                cmd.Parameters.AddWithValue("@id", idProd);
                decimal p = Convert.ToDecimal(cmd.ExecuteScalar());
                c.Close();
                return p;
            }


            // BOTÓN: CONFIRMAR PEDIDO (INSERTA TODO)
            
            private void btnConfirmarProv_Click_1(object sender, EventArgs e)
            {
                if (carritoProv.Count == 0)
                {
                    MessageBox.Show("El carrito está vacío.");
                    return;
                }

                try
                {
                    Conexion conexion = new Conexion();
                    MySqlConnection c = conexion.GetConexion();
                    c.Open();

                    foreach (var item in carritoProv)
                    {
                        // 1) Insertar pedido del proveedor
                        string query = @"INSERT INTO pedidos_proveedor 
                                    (id_proveedor, id_producto, cantidad, fecha_pedido)
                                     VALUES (@prov, @prod, @cant, @fecha)";

                        MySqlCommand cmd = new MySqlCommand(query, c);
                        cmd.Parameters.AddWithValue("@prov", item.IdProveedor);
                        cmd.Parameters.AddWithValue("@prod", item.IdProducto);
                        cmd.Parameters.AddWithValue("@cant", item.Cantidad);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                        cmd.ExecuteNonQuery();

                        // 2) Actualizar stock
                        string updateStock = @"UPDATE productos 
                                           SET stock = stock + @cant 
                                           WHERE id_producto = @prod";

                        MySqlCommand cmdStock = new MySqlCommand(updateStock, c);
                        cmdStock.Parameters.AddWithValue("@cant", item.Cantidad);
                        cmdStock.Parameters.AddWithValue("@prod", item.IdProducto);
                        cmdStock.ExecuteNonQuery();
                    }

                    c.Close();

                    carritoProv.Clear();
                    ActualizarCarritoProv();
                    CargarPedidos();
                    CargarProductos();

                    MessageBox.Show("Pedidos confirmados correctamente ✔");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }
    
