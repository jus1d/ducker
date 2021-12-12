using System.Data;
using MySqlConnector;

namespace ducker;

public class Database
{
    private MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=ducker");

    public void OpenConnection()
    {
        if (connection.State == ConnectionState.Closed)
            connection.Open();
    }

    public void CloseConnection()
    {
        if (connection.State == ConnectionState.Open)
            connection.Close();
    }

    public MySqlConnection GetConnection()
    {
        return connection;
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
        {
            return ulong.Parse(table.Rows[0].ItemArray[0].ToString());
        }
        else
        {
            return 0;
        }
    }
}