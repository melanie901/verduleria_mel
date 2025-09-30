using System;

public class Class1
{
	public Class1()
	{
	}
}
using MySql.Data.MySqlClient;

public class Conexion
{
    // ATRIBUTOS DE CLASE
    private const string servidor = "datasource=127.0.0.1";
    private const string puerto = "port=3306";
    private const string username = "username=root";
    private const string password = "password=amatista";
    private const string bd = "database=verduleria";

    // ATRIBUTOS DE INSTANCIA
    private string cadenaConexion;

    // CONSTRUCTOR
    public Conexion()
    {
        cadenaConexion = servidor + ";" + puerto + ";" + username + ";" + password + ";" + bd;
    }

    // SERVICIOS
    public MySqlConnection getConexion()
    {
        return new MySqlConnection(cadenaConexion);
    }
}
