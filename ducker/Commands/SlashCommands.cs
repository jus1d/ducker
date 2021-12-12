using System.Data;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using MySqlConnector;

namespace ducker
{
    public partial class SlashCommands : ApplicationCommandModule
    { 
        // avatar
        [SlashCommand("avatar", "Send embed with users avatar to current channel")]
        public async Task Avatar(InteractionContext msg,
            [Option("User", "User, whose avatar you need")] DiscordUser user) 
        {
            var avatarEmbed = new DiscordEmbedBuilder
            {
                Title = "User's avatar",
                Description = $"**{user.Mention}'s avatar**",
                ImageUrl = user.AvatarUrl,
                Url = user.AvatarUrl,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(avatarEmbed));
        }
        
        
        // invite-link
        [SlashCommand("invite-link", "Send invite link for this bot to current channel")]
        public async Task InviteLink(InteractionContext msg) 
        {
            var inviteLinkEmbed = new DiscordEmbedBuilder
            {
                Title = "Invite Link",
                Url = Bot.InviteLink,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(inviteLinkEmbed));
        }
        
        
        // random
        [SlashCommand("random", "Send random value in your range from min to max value to current channel")]
        public async Task Random(InteractionContext msg, 
            [Option("min", "Minimal value in your range")] long minValue, 
            [Option("max", "Maximal value in your range")] long maxValue) 
        {
            var rnd = new Random();
            var randomEmbed = new DiscordEmbedBuilder
            {
                Title = "Random number",
                Description = $"Your random number is: **{rnd.Next((int)minValue, (int)maxValue + 1)}**",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(randomEmbed));
        }
        
        
        // ban
        [SlashCommand("ban", "Ban mentioned user in current server"), RequirePermissions(Permissions.Administrator)]
        public async Task Ban(InteractionContext msg, [Option("user", "User for ban")] DiscordUser user, [Option("reason", "Reason for ban this user")] string reason = "") 
        {
            var banEmbed = new DiscordEmbedBuilder();
            try
            {
                if (reason == "")
                    await ((DiscordMember) user).BanAsync();
                else
                    await ((DiscordMember) user).BanAsync(0, reason);
                
                banEmbed = new DiscordEmbedBuilder
                {
                    Title = "User banned",
                    Description = $"gl, {user.Mention} :)",
                    ImageUrl = "https://static.wikia.nocookie.net/angrybirds-fiction/images/b/b7/%D0%91%D0%B0%D0%BD%D1%85%D0%B0%D0%BC%D0%BC%D0%B5%D1%80.png/revision/latest?cb=20190731080031&path-prefix=ru",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
            }
            catch (Exception e)
            {
                banEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: **You can't ban this user**",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
            }
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(banEmbed));
        }
        
        
        // kick
        [SlashCommand("kick", "Kicks mentioned user from current server"), RequirePermissions(Permissions.Administrator)]
        public async Task Kick(InteractionContext msg, [Option("user", "User for kick")] DiscordUser user, [Option("reason", "Reason for kick this member")] string reason = "")
        {
            try
            {
                if (reason == "")
                    await ((DiscordMember) user).RemoveAsync();
                else
                    await ((DiscordMember) user).RemoveAsync(reason);
            }
            catch (Exception e)
            {
                var incorrectKickEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: You can't kick this member",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(incorrectKickEmbed));
                throw;
            }
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent($"{user.Mention} kicked"));
        }
        
        
        // clear
        [SlashCommand("clear", "Clear amount messages in a current channel"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Clear(InteractionContext msg, [Option("amount", "Amount messages to delete")] long amount)
        {
            if (amount > 100 || amount < 0)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = "Missing argument",
                    Description = "**Usage:** `-clear <amount> (amount must be less than 100 and bigger than 0)`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(incorrectCommandEmbed));
            }
            else
            {
                await msg.Channel.DeleteMessagesAsync(await msg.Channel.GetMessagesAsync((int)amount + 1));

                string messageOrMessages;
                if (amount.ToString()[amount.ToString().Length - 1] == '1' && amount != 11)
                {
                    messageOrMessages = "message";
                }
                else
                {
                    messageOrMessages = "messages";
                }
            
                var deletedMessagesReport = new DiscordEmbedBuilder
                {
                    Title = "Deleted messages report", 
                    Description = $"I have deleted {amount} {messageOrMessages}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(deletedMessagesReport));
            }
        }


        // embed
        [SlashCommand("embed", "Sends to current channel embed with your title, description and other settings"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Embed(InteractionContext msg,
            [Option("description", "Set description tp your embed")] string description = null,
            [Option("title", "Set title for your embed")] string title = null,
            [Option("color", "Set color to your embed")] string colorHexCode = null,
            [Option("image", "Add image to your embed")] string imageUrl = null,
            [Option("titleURL", "Set title clickable to your URL")] string titleUrl = null)
        {
            var color = new DiscordColor(colorHexCode);
            
            var userCreatedEmbed = new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
                Url = titleUrl,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = color
            };
            
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(userCreatedEmbed));
        }
        
        
        // /reaction
        [SlashCommand("reaction", "Adds a reaction to message, which ID you enter")]
        public async Task Reaction(InteractionContext msg, 
            [Option("messageID", "Messages ID to add reaction")] string messageIdInput, 
            [Option("emoji", "Emojis to add")] DiscordEmoji emoji)
        {
            ulong messageId;
            try
            {
                messageId = ulong.Parse(messageIdInput);
            }
            catch (Exception e)
            {
                var incorrectMessageIdEmbed = new DiscordEmbedBuilder
                {
                    Description = "Your message ID is incorrect",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                throw;
            }
            await msg.Channel.GetMessageAsync(messageId).Result.CreateReactionAsync(emoji);
            var completeEmbed = new DiscordEmbedBuilder
            {
                Description = "Complete, reaction added",
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


        [SlashCommand("add-role", "Adds a role to mentioned member"),  RequirePermissions(Permissions.ManageRoles)]
        public async Task AddRole(InteractionContext msg,
            [Option("member", "Member to add role")] DiscordUser user,
            [Option("role", "Role to add it")] DiscordRole role)
        {
            DiscordMember member = (DiscordMember) user;
            
            if (member.Roles.ToArray().Contains(role))
            {
                var memberHasRoleEmbed = new DiscordEmbedBuilder
                {
                    Description = "This member currently has this role",
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
                await member.GrantRoleAsync(role);
                var completeEmbed = new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} added to {user.Mention}",
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
                    Description = ":x: You can't add this role",
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

        [SlashCommand("set-music-channel", "Set music channel for this server"),
         RequirePermissions(Permissions.Administrator)]
        public async Task SetMusicCommand(InteractionContext msg,
            [Option("channel", "Music channel")] DiscordChannel channel)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            Database database = new Database();
            DataTable table = new DataTable();
            DataTable findGuildTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand findGuildCommand = new MySqlCommand($"SELECT * FROM `ducker` WHERE `guildId` = '{msg.Guild.Id}'", database.GetConnection());
            adapter.SelectCommand = findGuildCommand;
            adapter.Fill(findGuildTable);
            if (findGuildTable.Rows.Count > 0)
            {
                MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `musicChannelId` = @musicChannelId WHERE `ducker`.`guildId` = @guildId",
                    database.GetConnection());
                command.Parameters.Add("@guildId", MySqlDbType.VarChar).Value = msg.Channel.Guild.Id;
                command.Parameters.Add("@musicChannelId", MySqlDbType.VarChar).Value = channel.Id;
            
                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
            else
            {
                MySqlCommand command = new MySqlCommand("INSERT INTO `ducker` (`guildId`, `musicChannelId`) VALUES (@guildId, @musicChannelId)", 
                    database.GetConnection());
                command.Parameters.Add("@guildId", MySqlDbType.VarChar).Value = msg.Channel.Guild.Id;
                command.Parameters.Add("@musicChannelId", MySqlDbType.VarChar).Value = channel.Id;

                adapter.SelectCommand = command;
                adapter.Fill(table);
            }
        }
    }
}