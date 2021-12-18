using System.Data;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Attributes;
using ducker.Database;
using MySqlConnector;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("set-channel", "Set music channel for this server"),
         RequireAdmin]
        public async Task SetChannelCommand(InteractionContext msg,
            [Option("channelType", "Channel to set")] 
            [Choice("Command channel", "cmd")] 
            [Choice("Logs channel", "logs")] 
            [Choice("Music channel", "music")] string channelType,
            [Option("channel", "Music channel")] DiscordChannel channel)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            DB db = new DB();
            DataTable table = new DataTable();
            DataTable findGuildTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand findGuildCommand = new MySqlCommand($"SELECT * FROM `ducker` WHERE `guildId` = '{msg.Guild.Id}'", db.GetConnection());
            adapter.SelectCommand = findGuildCommand;
            adapter.Fill(findGuildTable);
            switch (channelType)
            {
                case "music":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `musicChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            db.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, {channel.Id}, NULL, NULL)", 
                            db.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
                case "cmd":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `cmdChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            db.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, NULL, {channel.Id}, NULL)", 
                            db.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
                case "logs":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `logsChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            db.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, NULL, NULL, {channel.Id})", 
                            db.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
            }
            await msg.Channel.SendMessageAsync(ducker.Embed.ChannelConfiguredEmbed(msg.User, channelType, channel));
        }
    }
}