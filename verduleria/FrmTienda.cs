using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace verduleria
{ 
        public partial class FrmTienda : Form
        {
            Conexion conexion = new Conexion();
            MySqlConnection c;

        
            //  CARRITO
          
            List<CarritoItem> carrito = new List<CarritoItem>();

            public class CarritoItem
            {
                public int IdProducto { get; set; }
                public string Nombre { get; set; }
                public int Cantidad { get; set; }
                public decimal PrecioUnitario { get; set; }
                public decimal Subtotal => Cantidad * PrecioUnitario;
            }
       


            public FrmTienda()
            {
                InitializeComponent();
            }

            private void FrmTienda_Load(object sender, EventArgs e)
            {
                cmbTipo.Items.Add("Fruta");
                cmbTipo.Items.Add("Verdura");

                CargarProductos();
                CargarPedidos();
            }

            private void CargarProductos()
            {
                try
                {
                    Conexion conexion = new Conexion();
                    MySqlConnection c = conexion.GetConexion();
                    c.Open();
                    string query = "SELECT id_producto, nombre, tipo, precio FROM productos";
                    MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbProducto.DataSource = dt;
                    cmbProducto.DisplayMember = "nombre";
                    cmbProducto.ValueMember = "id_producto";
                    c.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar productos: " + ex.Message);
                }
            }

            private void CargarPedidos()
            {
                try
                {
                    Conexion conexion = new Conexion();
                    MySqlConnection c = conexion.GetConexion();
                    c.Open();

                    string query = @"SELECT d.id_detalle AS 'ID Detalle', 
                                    p.nombre AS 'Producto', 
                                    d.cantidad AS 'Cantidad', 
                                    d.precio_unitario AS 'Precio Unitario', 
                                    (d.cantidad * d.precio_unitario) AS 'Subtotal'
                                 FROM detalleventas d
                                 INNER JOIN productos p ON d.id_producto = p.id_producto";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, c);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvProductos.DataSource = dt;

                    c.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar pedidos: " + ex.Message);
                }
            }

            private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
            {
                string tipoSeleccionado = cmbTipo.SelectedItem.ToString();

                try
                {
                    Conexion conexion = new Conexion();
                    MySqlConnection c = conexion.GetConexion();
                    c.Open();
                    string query = "SELECT id_producto, nombre FROM productos WHERE tipo = @tipo";
                    MySqlCommand cmd = new MySqlCommand(query, c);
                    cmd.Parameters.AddWithValue("@tipo", tipoSeleccionado);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbProducto.DataSource = dt;
                    cmbProducto.DisplayMember = "nombre";
                    cmbProducto.ValueMember = "id_producto";

                    c.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al filtrar productos: " + ex.Message);
                }
            }


         
            // OBTENER PRECIO DE PRODUCTO
          
            private decimal ObtenerPrecio(int idProducto)
            {
                Conexion conexion = new Conexion();
                MySqlConnection c = conexion.GetConexion();
                c.Open();
                string query = "SELECT precio FROM productos WHERE id_producto = @id";
                MySqlCommand cmd = new MySqlCommand(query, c);
                cmd.Parameters.AddWithValue("@id", idProducto);
                decimal precio = Convert.ToDecimal(cmd.ExecuteScalar());
                c.Close();
                return precio;
            }
         


            
            //  AGREGAR AL CARRITO
         
            private void btnAgregarCarrito_Click_1(object sender, EventArgs e)
            {
                if (cmbProducto.SelectedValue == null)
                {
                    MessageBox.Show("Seleccione un producto.");
                    return;
                }

                int idProducto = Convert.ToInt32(cmbProducto.SelectedValue);
                int cantidad = (int)numCantidad.Value;

                DataRowView row = (DataRowView)cmbProducto.SelectedItem;
                string nombre = row["nombre"].ToString();
                decimal precioUnitario = ObtenerPrecio(idProducto);

                carrito.Add(new CarritoItem
                {
                    IdProducto = idProducto,
                    Nombre = nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = precioUnitario
                });

                ActualizarCarrito();
            }
          


      
            // ACTUALIZAR DGV DEL CARRITO
        
            private void ActualizarCarrito()
            {
                dgvCarrito.DataSource = null;
                dgvCarrito.DataSource = carrito;
            }
           


           
            //  CONFIRMAR COMPRA (INSERTA TODO)
         
            private void btnConfirmarCompra_Click(object sender, EventArgs e)
            {
                if (carrito.Count == 0)
                {
                    MessageBox.Show("El carrito está vacío.");
                    return;
                }

                try
                {
                    Conexion conexion = new Conexion();
                    MySqlConnection c = conexion.GetConexion();
                    c.Open();

                    decimal totalVenta = carrito.Sum(item => item.Subtotal);

                    // Insertar venta
                    string query = @"INSERT INTO ventas (id_venta, fecha, total) 
                                 VALUES (NULL, @fecha, @total)";
                    MySqlCommand cmd = new MySqlCommand(query, c);
                    cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                    cmd.Parameters.AddWithValue("@total", totalVenta);
                    cmd.ExecuteNonQuery();

                    // Obtener id de esa venta
                    cmd = new MySqlCommand("SELECT id_venta FROM ventas ORDER BY id_venta DESC LIMIT 1", c);
                    int idVenta = Convert.ToInt32(cmd.ExecuteScalar());

                    // Insertar cada item
                    foreach (var item in carrito)
                    {
                        // detalleventas
                        query = @"INSERT INTO detalleventas 
                              (id_venta, id_producto, cantidad, precio_unitario)
                              VALUES (@id_venta, @id_producto, @cantidad, @precio)";

                        cmd = new MySqlCommand(query, c);
                        cmd.Parameters.AddWithValue("@id_venta", idVenta);
                        cmd.Parameters.AddWithValue("@id_producto", item.IdProducto);
                        cmd.Parameters.AddWithValue("@cantidad", item.Cantidad);
                        cmd.Parameters.AddWithValue("@precio", item.PrecioUnitario);
                        cmd.ExecuteNonQuery();

                        // actualizar stock
                        query = @"UPDATE productos 
                              SET stock = stock - @cantidad 
                              WHERE id_producto = @id";

                        cmd = new MySqlCommand(query, c);
                        cmd.Parameters.AddWithValue("@id", item.IdProducto);
                        cmd.Parameters.AddWithValue("@cantidad", item.Cantidad);
                        cmd.ExecuteNonQuery();
                    }

                    c.Close();

                    carrito.Clear();
                    ActualizarCarrito();
                    CargarPedidos();

                    MessageBox.Show("Compra registrada correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al procesar la compra: " + ex.Message);
                }
            }
            private void btnSalir_Click(object sender, EventArgs e)
            {
                this.Close();
            }

       
    }
    }