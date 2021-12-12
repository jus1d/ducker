using System.Data;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MySqlConnector;

namespace ducker;

public partial class Commands
{
    [Command("login")]
    public async Task LogIn(CommandContext msg, string login, string password)
    {
        Database database = new Database();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        
        MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`id`, `login`, `pass`) VALUES (NULL, @ul, @up)", database.GetConnection());
        command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = login;
        command.Parameters.Add("@up", MySqlDbType.VarChar).Value = password;

        adapter.SelectCommand = command;
        adapter.Fill(table);
    }
}