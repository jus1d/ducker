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

        [SlashCommand("remove-role", "Removes role from mentioned member"),  RequirePermissions(Permissions.ManageRoles)]
        public async Task RemoveRole(InteractionContext msg,
            [Option("member", "Member for remove role")] DiscordUser user,
            [Option("role", "Role to remove it")] DiscordRole role)
        {
            DiscordMember member = (DiscordMember) user;
            
            if (!member.Roles.ToArray().Contains(role))
            {
                var memberHasRoleEmbed = new DiscordEmbedBuilder
                {
                    Description = "This member doesn't have this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(memberHasRoleEmbed));
                return;
            }
            
            try
            {
                await member.RevokeRoleAsync(role);
                var completeEmbed = new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} removed from {user.Mention}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(completeEmbed));
            }
            catch (Exception e)
            {
                var incorrectEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: You can't remove this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(incorrectEmbed));
            }
        }

        [SlashCommand("activity", "Changes activity to the bot")]
        public async Task ActivityChnger(InteractionContext msg,
            [Choice("playing", "playing")] 
            [Choice("streaming", "streaming")] 
            [Option("type", "Type for activity")] string activityType)
        {
            if (activityType == "playing")
            {
                var activity = new DiscordActivity
                {
                    ActivityType = ActivityType.Playing,
                    Name = "with ducks | -help"
                };
                var activityChanedEmbed = new DiscordEmbedBuilder
                {
                    Description = "Activity changed to playing type",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Client.UpdateStatusAsync(activity);
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(activityChanedEmbed));
            }
            else if (activityType == "streaming")
            {
                var activity = new DiscordActivity
                {
                    ActivityType = ActivityType.Streaming,
                    Name = "with ducks |  -help",
                    StreamUrl = "https://www.twitch.tv/itakash1"
                };
                var activityChanedEmbed = new DiscordEmbedBuilder
                {
                    Description = "Activity changed to streaming type",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Client.UpdateStatusAsync(activity);
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(activityChanedEmbed));
            }
        }


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