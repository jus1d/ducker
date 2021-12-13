using System.Data;
using MySqlConnector;

namespace ducker;

public class Database
{
    private MySqlConnection _connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=ducker");

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

    public static ulong GetMusicChannel(ulong guildId)
    {
        Database database = new Database();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();

        MySqlCommand command = new MySqlCommand($"SELECT `musicChannelId` FROM `ducker` WHERE `guildId` = {guildId}", database.GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
            return ulong.Parse(table.Rows[0].ItemArray[0].ToString());
        return 0;
    }

    public static ulong GetLogsChannel(ulong guildId)
    {
        Database database = new Database();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();

        MySqlCommand command = new MySqlCommand($"SELECT `logsChannelId` FROM `ducker` WHERE `guildId` = {guildId}", 
            database.GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
            return ulong.Parse(table.Rows[0].ItemArray[0].ToString());
        return 0;
    }
    
    public static ulong GetCmdChannel(ulong guildId)
    {
        Database database = new Database();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();

        MySqlCommand command = new MySqlCommand($"SELECT `cmdChannelId` FROM `ducker` WHERE `guildId` = {guildId}", 
            database.GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
            return ulong.Parse(table.Rows[0].ItemArray[0].ToString());
        return 0;
    }

    public static ulong GetMuteRoleId(ulong guildId)
    {
        Database database = new Database();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        database.OpenConnection();
        MySqlCommand command = new MySqlCommand($"SELECT `muteRoleId` FROM `ducker` WHERE `guildId` = {guildId}", database.GetConnection());
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
}