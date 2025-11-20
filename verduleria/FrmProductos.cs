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

    public partial class FrmProductos : Form
    {
        private MySqlConnection miConexion;
        private string sql;
        private MySqlCommand comando;
        private MySqlDataAdapter adaptador;

        public FrmProductos()
        {
            InitializeComponent();
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            mostrarProductos();
        }

        private void mostrarProductos()
        {
            try
            {
                Conexion c = new Conexion();
                miConexion = c.GetConexion();
                miConexion.Open();

                sql = "SELECT idProducto, nombre, precio, stock, idProveedor FROM productos";
                comando = new MySqlCommand(sql, miConexion);
                adaptador = new MySqlDataAdapter(comando);

                DataTable dt = new DataTable();
                adaptador.Fill(dt);
                dgvProductos.DataSource = dt;

                miConexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los productos: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Conexion c = new Conexion();
                miConexion = c.GetConexion();
                miConexion.Open();

                sql = "INSERT INTO productos (nombre, precio, stock, idProveedor) VALUES (@nombre, @precio, @stock, @idProveedor)";
                comando = new MySqlCommand(sql, miConexion);
                comando.Parameters.AddWithValue("@nombre", txtNombre.Text);
                comando.Parameters.AddWithValue("@precio", txtPrecio.Text);
                comando.Parameters.AddWithValue("@stock", txtStock.Text);
                comando.Parameters.AddWithValue("@idProveedor", txtIdProveedor.Text);

                comando.ExecuteNonQuery();

                MessageBox.Show("Producto agregado correctamente");
                miConexion.Close();
                mostrarProductos();
      
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar producto: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Conexion c = new Conexion();
                miConexion = c.GetConexion();
                miConexion.Open();

                sql = "UPDATE productos SET nombre=@nombre, precio=@precio, stock=@stock, idProveedor=@idProveedor WHERE idProducto=@id";
                comando = new MySqlCommand(sql, miConexion);
                comando.Parameters.AddWithValue("@id", txtId.Text);
                comando.Parameters.AddWithValue("@nombre", txtNombre.Text);
                comando.Parameters.AddWithValue("@precio", txtPrecio.Text);
                comando.Parameters.AddWithValue("@stock", txtStock.Text);
                comando.Parameters.AddWithValue("@idProveedor", txtIdProveedor.Text);

                comando.ExecuteNonQuery();

                MessageBox.Show("Producto modificado correctamente");
                miConexion.Close();
                mostrarProductos();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar producto: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                Conexion c = new Conexion();
                miConexion = c.GetConexion();
                miConexion.Open();

                sql = "DELETE FROM productos WHERE idProducto=@id";
                comando = new MySqlCommand(sql, miConexion);
                comando.Parameters.AddWithValue("@id", txtId.Text);

                comando.ExecuteNonQuery();

                MessageBox.Show("Producto eliminado correctamente");
                miConexion.Close();
                mostrarProductos();
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar producto: " + ex.Message);
            }
        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtId.Text = dgvProductos.Rows[e.RowIndex].Cells["idProducto"].Value.ToString();
                txtNombre.Text = dgvProductos.Rows[e.RowIndex].Cells["nombre"].Value.ToString();
                txtPrecio.Text = dgvProductos.Rows[e.RowIndex].Cells["precio"].Value.ToString();
                txtStock.Text = dgvProductos.Rows[e.RowIndex].Cells["stock"].Value.ToString();
                txtIdProveedor.Text = dgvProductos.Rows[e.RowIndex].Cells["idProveedor"].Value.ToString();
            }
        }
    }
}



