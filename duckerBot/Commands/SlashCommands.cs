using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;

namespace duckerBot
{
    public class SlashCommands : ApplicationCommandModule
    {
        [SlashCommand("help", "Send help list to current channel")]
        public async Task TestCommand(InteractionContext msg, [Option("Command", "Command for detailed description")] string command = null)
        {
            await msg.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
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
                    Color = Bot.mainEmbedColor
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
                    Color = Bot.mainEmbedColor
                };
                await msg.Channel.SendMessageAsync(avatarHelpEmbed);
            }
        }
    }
}