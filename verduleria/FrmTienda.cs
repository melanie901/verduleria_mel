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
        public FrmTienda()
        {
            InitializeComponent();
           
        }



        private void btnComprar_Click(object sender, EventArgs e)
        {
            Conexion conexion = new Conexion();
            MySqlConnection c = conexion.GetConexion();
            try
            {
                {
                    c.Open();

                    // Insertar una nueva venta
                    string insertVenta = "INSERT INTO ventas (fecha, id_cliente, id_empleado) VALUES (NOW(), 1, 1)";
                    MySqlCommand cmdVenta = new MySqlCommand(insertVenta, c);
                    cmdVenta.ExecuteNonQuery();

                    // Obtener el ID de esa venta
                    long idVenta = cmdVenta.LastInsertedId;

                    // Obtener datos del formulario
                    string producto = cmbProducto.Text;
                    decimal cantidad = numCantidad.Value;
                    decimal total = decimal.Parse(txtTotal.Text);

                    // Insertar detalle de venta
                    string insertDetalle = "INSERT INTO detalleventas (id_venta, id_producto, cantidad, precio_unitario) " +
                                           "VALUES (@idVenta, (SELECT id_producto FROM productos WHERE nombre = @producto LIMIT 1), @cantidad, @precio)";
                    MySqlCommand cmdDetalle = new MySqlCommand(insertDetalle, c);
                    cmdDetalle.Parameters.AddWithValue("@idVenta", idVenta);
                    cmdDetalle.Parameters.AddWithValue("@producto", producto);
                    cmdDetalle.Parameters.AddWithValue("@cantidad", cantidad);
                    cmdDetalle.Parameters.AddWithValue("@precio", total / cantidad);

                    cmdDetalle.ExecuteNonQuery();

                    MessageBox.Show(" Venta registrada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Error al registrar la venta: " + ex.Message);
            }
            c.Close();
        } 
        private void FrmTienda_Load(object sender, EventArgs e)
        {
            CargarTipos();
            CargarProductos();
        }
        private void CargarTipos()
        { 
            cmbTipo.Items.Clear();
            cmbTipo.Items.Add("Fruta");
            cmbTipo.Items.Add("Verdura");
        }
        private void CargarProductos()

        {
            Conexion conexion = new Conexion();
            MySqlConnection c = conexion.GetConexion();
            try
            {
                {
                    c.Open();
                    string query = "SELECT id_producto, nombre FROM productos";
                    MySqlCommand cmd = new MySqlCommand(query , c);
                    MySqlDataAdapter da= new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbProducto.DataSource = dt;
                    cmbProducto.DisplayMember = "nombre";
                    cmbProducto.ValueMember = "id_producto";
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
            c.Close();

        }
       

        }
    }