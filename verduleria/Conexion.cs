using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Es importante utilizar la biblioteca de clases MySql,
// en particular, Data.MySqlClient
namespace verduleria
{
public class Conexion
    {
        //ATRITUBOS DE CLASE
        private const string servidor = "datasource=127.0.0.1";
        private const string puerto = "port=3306";
        private const string username = "username=root";
        private const string password = "password=amatista";
        private const string bd = "database=verduleria";
        //ATRIBUTOS DE INSTANCIA
        private String cadenaConexion;
        //CONSTRUCTOR
        public Conexion()
        {

            cadenaConexion = servidor + ";" + puerto + ";" + username
            + ";" + password + ";" + bd;

        }
        //SERVICIOS
        public MySqlConnection GetConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }
    }
}
