using System.Data;
using MySql.Data.MySqlClient;

namespace ducker;

public class Database
{
    private MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=ducker");

    public void OpenConnection()
    {
        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }
    }

    public void CloseConnection()
    {
        if (connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }

    public MySqlConnection GetConnection()
    {
        return connection;
    }
}