using System.Data;
using ducker.Config;
using MySqlConnector;

namespace ducker.Database;

public class DB
{
    private MySqlConnection _connection = new (ConfigJson.GetConfigField().MySqlConnectionString);

    public void OpenConnection()
    {
        if (_connection.State == ConnectionState.Closed)
            _connection.Open();
    }

    public void CloseConnection()
    {
        if (_connection.State == ConnectionState.Open)
            _connection.Close();
    }

    public MySqlConnection GetConnection()
    {
        return _connection;
    }

    public static void Update(ulong guildId, string field, string value)
    {
        DB db = new DB();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        db.OpenConnection();
        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `{field}` = {value} WHERE `ducker`.`guildId` = {guildId}", db.GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);
    }

    public static ulong GetId(ulong guildId, string field)
    {
        DB db = new DB();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        db.OpenConnection();
        MySqlCommand command = new MySqlCommand($"SELECT `{field}` FROM `ducker` WHERE `ducker`.`guildId` = {guildId}", db.GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
        {
            try
            {
                return ulong.Parse(table.Rows[0].ItemArray[0].ToString());
            }
            catch
            {
                return 0;
            }
        }
        return 0;
    }
    
    public static string GetString(ulong guildId, string field)
    {
        DB db = new DB();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        db.OpenConnection();
        MySqlCommand command = new MySqlCommand($"SELECT `{field}` FROM `ducker` WHERE `guildId` = {guildId}", db.GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
        {
            try
            {
                return table.Rows[0].ItemArray[0].ToString();
            }
            catch
            {
                return "No data found";
            }
        }
        return "No data found";
    }
}