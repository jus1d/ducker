using System.Data;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MySqlConnector;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        [Command("set-channel"), 
         Description("Set music channel for this guild"),
         Aliases("sc"), 
         RequirePermissions(Permissions.Administrator)]
        public async Task SetMusicCommand(CommandContext msg, string channelType, DiscordChannel channel)
        {
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            Database database = new Database();
            DataTable table = new DataTable();
            DataTable findGuildTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand findGuildCommand = new MySqlCommand($"SELECT * FROM `ducker` WHERE `guildId` = '{msg.Guild.Id}'", 
                database.GetConnection());
            adapter.SelectCommand = findGuildCommand;
            adapter.Fill(findGuildTable);

            switch (channelType)
            {
                case "music":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `musicChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            database.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, {channel.Id}, NULL, NULL)", 
                            database.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
                case "cmd":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `cmdChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            database.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, NULL, {channel.Id}, NULL)", 
                            database.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
                case "logs":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `logsChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            database.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, NULL, NULL, {channel.Id})", 
                            database.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
            }
            await msg.Channel.SendMessageAsync(ducker.Embed.ChannelConfiguredEmbed(msg.User, channelType, channel));
        }
    }
}