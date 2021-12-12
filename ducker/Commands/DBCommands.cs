using System.Data;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MySqlConnector;

namespace ducker;

public partial class Commands
{
    [Command("set-music-channel"), Aliases("smc"), RequirePermissions(Permissions.Administrator)]
    public async Task LogIn(CommandContext msg, DiscordChannel channel)
    {
        Database database = new Database();
        DataTable table = new DataTable();
        DataTable findGuildTable = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();

        MySqlCommand findGuildCommand = new MySqlCommand($"SELECT * FROM `main` WHERE `guildId` = '{msg.Guild.Id}'", database.GetConnection());
        adapter.SelectCommand = findGuildCommand;
        adapter.Fill(findGuildTable);
        if (findGuildTable.Rows.Count > 0)
        {
            MySqlCommand command = new MySqlCommand($"UPDATE `main` SET `musicChannelId` = @musicChannelId WHERE `main`.`guildId` = @guildId",
                database.GetConnection());
            command.Parameters.Add("@guildId", MySqlDbType.VarChar).Value = msg.Channel.Guild.Id;
            command.Parameters.Add("@musicChannelId", MySqlDbType.VarChar).Value = channel.Id;
            
            adapter.SelectCommand = command;
            adapter.Fill(table);
        }
        else
        {
            MySqlCommand command = new MySqlCommand("INSERT INTO `main` (`guildId`, `musicChannelId`) VALUES (@guildId, @musicChannelId)", 
                database.GetConnection());
            command.Parameters.Add("@guildId", MySqlDbType.VarChar).Value = msg.Channel.Guild.Id;
            command.Parameters.Add("@musicChannelId", MySqlDbType.VarChar).Value = channel.Id;

            adapter.SelectCommand = command;
            adapter.Fill(table);
        }
    }

    [Command("t")]
    public async Task Test(CommandContext msg)
    {
        Database database = new Database();
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();

        MySqlCommand command = new MySqlCommand($"SELECT `musicChannelId` FROM `main` WHERE `guildId` = {msg.Guild.Id}", database.GetConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);
        ulong musicChannelIdFromDB = 0;
        if (table.Rows.Count > 0)
            musicChannelIdFromDB = ulong.Parse(table.Rows[0].ItemArray[0].ToString());

        await msg.Channel.SendMessageAsync(musicChannelIdFromDB.ToString());

        if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != musicChannelIdFromDB)
        {
            await msg.Channel.SendMessageAsync("not music channel");
        }
    }
}