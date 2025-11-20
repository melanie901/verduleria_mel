using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace verduleria
{
public class Conexion
    {
        private const string servidor = "datasource=127.0.0.1";
        private const string puerto = "port=3306";
        private const string username = "username=root";
        private const string password = "password=amatista";
        private const string bd = "database=verduleria";
    
        private String cadenaConexion;
      
        public Conexion()
        {

            cadenaConexion = servidor + ";" + puerto + ";" + username
            + ";" + password + ";" + bd;

        }
    
        public MySqlConnection GetConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }
    }
}
