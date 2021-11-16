using System;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace duckerBot
{
    public class SlashCommands : ApplicationCommandModule
    {
        // help
        [SlashCommand("help", "Send help list to current channel")]
        public async Task Help(InteractionContext msg, [Choice("avatar", "avatar")] [Choice("invitelink", "invitelink")] [Choice("random", "random")]
            [Choice("play", "play")] [Choice("pause", "pause")] [Choice("stop", "stop")] [Choice("ban", "ban")] 
            [Choice("kick", "kick")] [Choice("clear", "clear")] [Choice("embed", "embed")] [Choice("poll", "poll")]
            [Option("Command", "Command for detailed description")] string command = null) 
        {
            var helpMessageEmbed = new DiscordEmbedBuilder();
            if (command == null)
            {
                helpMessageEmbed = new DiscordEmbedBuilder
                {
                    Title = "Help",
                    Description = "List of all server commands.\n" +
                                  "Prefix for this server: '-'\n" +
                                  "Use `-help <command>` to see certain command description\n\n" +
                                  "**Commands**\n" +
                                  "`avatar`, `invitelink`, `random`, `play`, `pause`, `stop`\n" +
                                  "**Admin Commands**\n" +
                                  "`ban`, `kick`, `clear`, `embed`, `poll`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.mainEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(helpMessageEmbed));
            }
            else
            {
                string helpEmbedDescription = "";
                string helpEmbedCommandUsage = "";
                switch (command)
                {
                    case "avatar":
                        helpEmbedDescription = "Sends you embed with users avatar";
                        helpEmbedCommandUsage = "-avatar <user>";
                        break;
                    case "invitelink":
                        helpEmbedDescription = "Sends you invite link for this bot";
                        helpEmbedCommandUsage = "-invitelink";
                        break;
                    case "random":
                        helpEmbedDescription = "Sends you random value in your range from min to max value";
                        helpEmbedCommandUsage = "-random <min> <max>";
                        break;
                    case "play":
                        helpEmbedDescription = "Starts playing music from youtube by link or search request";
                        helpEmbedCommandUsage = "-play <link to youtube video> or <search>";
                        break;
                    case "pause":
                        helpEmbedDescription = "Pause now playing music (can use `-play` command to resume playing)";
                        helpEmbedCommandUsage = "-pause";
                        break;
                    case "stop":
                        helpEmbedDescription = "Permanently stop now playing music (can't use `-play` command to resume playing)";
                        helpEmbedCommandUsage = "-stop";
                        break;
                    case "ban":
                        helpEmbedDescription = "Ban mentioned user in current server";
                        helpEmbedCommandUsage = "-ban <user>";
                        break;
                    case "kick":
                        helpEmbedDescription = "Kick mentioned user from current server";
                        helpEmbedCommandUsage = "-kick <user>";
                        break;
                    case "clear":
                        helpEmbedDescription = "Clear certain number of messages in current channel";
                        helpEmbedCommandUsage = "-clear <amount> (amount must be less than 100)";
                        break;
                    case "embed":
                        helpEmbedDescription = "Send embed to current channel with your title, description, title URL, image (all optional, but title or description must be, if you use `-del` flag, message with config will be deleted)";
                        helpEmbedCommandUsage = "-embed -t <title> -d <description> -image <image URL> -titlelink <title URL> -del";
                        break;
                    case "poll":
                        helpEmbedDescription = "Creates embed with poll with your description in current channel, and create on this message :white_check_mark: and :x:";
                        helpEmbedCommandUsage = "-poll <poll description>";
                        break;
                    default:
                        helpEmbedDescription = "You try to use `-help <command>` with unknown command";
                        helpEmbedCommandUsage = "-help <command>";
                        break;
                }
                helpMessageEmbed = new DiscordEmbedBuilder
                {
                    Title = "Help",
                    Description = $"**Description:** {helpEmbedDescription}\n**Usage:** `{helpEmbedCommandUsage}`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.mainEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(helpMessageEmbed));
            }
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(helpMessageEmbed));
        }
        
        
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
                Color = Bot.mainEmbedColor
            };
            
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(avatarEmbed));
        }
        
        
        // invitelink
        [SlashCommand("invitelink", "Send invite link for this bot to current channel")]
        public async Task InviteLink(InteractionContext msg) 
        {
            var inviteLinkEmbed = new DiscordEmbedBuilder
            {
                Title = "Invite Link",
                Url = "https://discord.com/api/oauth2/authorize?client_id=906179696516026419&permissions=8&scope=bot",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.mainEmbedColor
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
                Color = Bot.mainEmbedColor
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
                    Color = Bot.mainEmbedColor
                };
            }
            catch (Exception e)
            {
                banEmbed = new DiscordEmbedBuilder
                {
                    Description = $":x: **You can't ban this user**",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.incorrectEmbedColor
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
                    Color = Bot.incorrectEmbedColor
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
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-clear <amount> (amount must be less than 100 and bigger than 0)`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.incorrectEmbedColor
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
                    Title = $"Deleted messages report", 
                    Description = $"I have deleted {amount} {messageOrMessages}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.mainEmbedColor
                };
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(deletedMessagesReport));
            }
        }
        
        
        // poll
        [SlashCommand("poll", "Sends poll embed with reactions"), RequirePermissions(Permissions.Administrator)]
        public async Task Poll(InteractionContext msg,
            [Option("description", "Set description to your poll embed")] string pollDescription)
        {
            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = pollDescription,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.mainEmbedColor
            };
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(pollEmbed));
        }
    }
}