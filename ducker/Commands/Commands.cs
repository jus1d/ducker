using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker
{
    public partial class Commands : BaseCommandModule
    {
        // -help
        //[Command("help")]
        public async Task Help(CommandContext msg, string command = null)
        {
            if (command == null)
            {
                var helpMessageEmbed = new DiscordEmbedBuilder
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
                    Color = Bot.MainEmbedColor
                };
                await msg.Channel.SendMessageAsync(helpMessageEmbed);
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
                var avatarHelpEmbed = new DiscordEmbedBuilder
                {
                    Title = "Help",
                    Description = $"**Description:** {helpEmbedDescription}\n**Usage:** `{helpEmbedCommandUsage}`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Channel.SendMessageAsync(avatarHelpEmbed);
            }
        }

        //[Command("help")]
        public async Task Help(CommandContext msg, params string[] text)
        {
            var incorrectHelpEmbed = new DiscordEmbedBuilder
            {
                Title = "Help",
                Description = "**Description:** You try to use `-help <command>` with unknown command\n**Usage:** `-help <command>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectHelpEmbed);
        }
        
        
        // -avatar
        [Command("avatar"), Aliases("ava"), GroupCommand]
        public async Task Avatar(CommandContext msg, [Description("user, whose avatar URL will send bot")] DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectAvatarCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = "Missing argument",
                    Description = "**Usage:** `-avatar <user>`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectAvatarCommandEmbed);
            }
            else
            {
                var userAvatarEmbed = new DiscordEmbedBuilder
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
                await msg.Channel.SendMessageAsync(userAvatarEmbed);
            }
        }
        
        [Command("avatar")]
        public async Task Avatar(CommandContext msg, params string[] text)
        {
            var incorrectAvatarCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** `-avatar <user>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectAvatarCommandEmbed);
        }
        
        
        // -invite-link
        [Command("invite-link"), Aliases("invite")]
        public async Task InviteLink(CommandContext msg)
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
                Color = Bot.MainEmbedColor
            };
            await msg.Channel.SendMessageAsync(inviteLinkEmbed);
        }

        [Command("invite-link")]
        public async Task InviteLink(CommandContext msg, params string[] text)
        {
            var incorrectInviteLinkCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** `-invitelink`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectInviteLinkCommandEmbed);
        }
        
        
        // -random <min> <max>
        [Command("random"), Aliases("rnd")]
        public async Task Random(CommandContext msg, [Description("min value")] int minValue, [Description("max value")] int maxValue)
        {
            var rnd = new Random();
            if (minValue > maxValue)
            {
                var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = "Missing argument",
                    Description = "**Usage:** `-random <min value> <max value>`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
                return;
            }
            var randomEmbed = new DiscordEmbedBuilder
            {
                Title = "Random number",
                Description = $"Your random number is: **{rnd.Next(minValue, maxValue + 1)}**",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            await msg.Channel.SendMessageAsync(randomEmbed);
        }

        [Command("random")]
        public async Task Random(CommandContext msg)
        {
            var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** `-random <min value> <max value>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
        }

        [Command("random")]
        public async Task Random(CommandContext msg, params string[] text)
        {
            var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = $"**Usage:** `-random <min value> <max value>`\n [for {msg.Member.Mention}]",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
        }
    }
}