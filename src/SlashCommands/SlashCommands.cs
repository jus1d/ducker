using System.Data;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using ducker.Attributes;
using ducker.Database;
using MySqlConnector;

namespace ducker.SlashCommands
{
    public partial class SlashCommands : ApplicationCommandModule
    {
        [SlashCommand("reaction-role-embed", "Send embed with reactions, press them to get role"),
         RequirePermissions(Permissions.Administrator)]
        public async Task ReactionRoleEmbed(InteractionContext msg)
        {
            DiscordEmoji twitchRgbEmoji = DiscordEmoji.FromName(msg.Client, ":twitchrgb:");
            DiscordEmoji chelEmoji = DiscordEmoji.FromName(msg.Client, ":chel:");
            var followButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_follow_role", "", false, new DiscordComponentEmoji(twitchRgbEmoji));
            var chelButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_chel_role", "", false, new DiscordComponentEmoji(chelEmoji));
            
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                .AddEmbed(ducker.Embed.ReactionRolesEmbed(msg.Client, msg.Guild))
                .AddComponents(followButton, chelButton));
        }


        [SlashCommand("stream", "Send stream announcement"), RequirePermissions(Permissions.Administrator)]
        public async Task StreamAnnouncement(InteractionContext msg, [Option("description", "Stream description")] string description = "")
        {
            await msg.CreateResponseAsync(ducker.Embed.StreamAnnouncementEmbed(msg, description));
            await (await msg.Channel.SendMessageAsync(msg.Guild.GetRole(Role.TwitchFollowerRoleId).Mention)).DeleteAsync();
        }

        [SlashCommand("set-channel", "Set music channel for this server"),
         RequirePermissions(Permissions.Administrator)]
        public async Task SetMusicCommand(InteractionContext msg,
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