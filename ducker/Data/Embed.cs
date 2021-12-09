using System;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;

namespace ducker
{
    public class Embed
    {
        public static DiscordMessageBuilder NowPlaying(DiscordClient client, LavalinkTrack track, DiscordUser user)
        {
            var playEmbed = new DiscordEmbedBuilder
            {
                Title = "Now playing",
                Description = $"[{track.Title}]({track.Uri})",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = user.AvatarUrl,
                    Text = "Ordered by " + user.Username
                },
                Color = Bot.MainEmbedColor
            };
            var playButton = new DiscordButtonComponent(ButtonStyle.Secondary, "play_button", $"Play", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":arrow_forward:")));
            var pauseButton = new DiscordButtonComponent(ButtonStyle.Secondary, "pause_button", $"Pause", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":pause_button:")));
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":track_next:")));
            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":page_facing_up:")));
            
            return new DiscordMessageBuilder()
                .AddEmbed(playEmbed)
                .AddComponents(pauseButton, playButton, nextButton, queueButton);
        }
        public static DiscordEmbedBuilder NowPlayingEmbed(LavalinkTrack track, DiscordUser user)
        {
            return new DiscordEmbedBuilder
            {
                Title = "Now playing",
                Description = $"[{track.Title}]({track.Uri})",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = user.AvatarUrl,
                    Text = "Ordered by " + user.Username
                },
                Color = Bot.MainEmbedColor
            };
        }
        
        public static DiscordMessageBuilder IncorrectMusicChannel(CommandContext msg)
        {
            var incorrectMusicChannel = new DiscordEmbedBuilder
            {
                Title = "Incorrect channel for music commands",
                Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(incorrectMusicChannel);
        }
        public static DiscordMessageBuilder IncorrectMusicChannel(InteractionContext msg)
        {
            var incorrectMusicChannel = new DiscordEmbedBuilder
            {
                Title = "Incorrect channel for music commands",
                Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(incorrectMusicChannel);
        }
        public static DiscordEmbedBuilder IncorrectMusicChannelEmbed(InteractionContext msg)
        {
            return new DiscordEmbedBuilder
            {
                Title = "Incorrect channel for music commands",
                Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
        }
        public static DiscordEmbedBuilder IncorrectMusicChannelEmbed(CommandContext msg)
        {
            return new DiscordEmbedBuilder
            {
                Title = "Incorrect channel for music commands",
                Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
        }
        
        public static DiscordMessageBuilder NotInVoiceChannel(CommandContext msg)
        {
            var notInVoiceChannel = new DiscordEmbedBuilder
            {
                Description = "You are not in a voice channel",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(notInVoiceChannel);
        }
        public static DiscordMessageBuilder NotInVoiceChannel(InteractionContext msg)
        {
            var notInVoiceChannel = new DiscordEmbedBuilder
            {
                Description = "You are not in a voice channel",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(notInVoiceChannel);
        }
        public static DiscordEmbedBuilder NotInVoiceChannelEmbed(InteractionContext msg)
        {
            return new DiscordEmbedBuilder
            {
                Description = "You are not in a voice channel",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
        }
        public static DiscordEmbedBuilder NotInVoiceChannelEmbed(CommandContext msg)
        {
            return new DiscordEmbedBuilder
            {
                Description = "You are not in a voice channel",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
        }
        
        public static DiscordMessageBuilder NoConnection(CommandContext msg)
        {
            var noConnection = new DiscordEmbedBuilder
            {
                Description = "I'm is not connected",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(noConnection);
        }
        public static DiscordMessageBuilder NoConnection(InteractionContext msg)
        {
            var noConnection = new DiscordEmbedBuilder
            {
                Description = "I'm is not connected",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(noConnection);
        }
        public static DiscordEmbedBuilder NoConnectionEmbed(InteractionContext msg)
        {
            return new DiscordEmbedBuilder
            {
                Description = "I'm is not connected",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
        }
        public static DiscordEmbedBuilder NoConnectionEmbed(CommandContext msg)
        {
            return new DiscordEmbedBuilder
            {
                Description = "I'm is not connected",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
        }
        
        public static DiscordMessageBuilder SearchFailed(CommandContext msg, string search)
        {
            var searchFailed = new DiscordEmbedBuilder
            {
                Description = $"Track search failed for: {search}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(searchFailed);
        }
        public static DiscordMessageBuilder SearchFailed(InteractionContext msg, string search)
        {
            var searchFailed = new DiscordEmbedBuilder
            {
                Description = $"Track search failed for: {search}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(searchFailed);
        }
        public static DiscordEmbedBuilder SearchFailedEmbed(InteractionContext msg, string search)
        {
            return new DiscordEmbedBuilder
            {
                Description = $"Track search failed for: {search}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
        }
        
        public static DiscordMessageBuilder NoTracksPlaying(CommandContext msg)
        {
            var noPlayingTracks = new DiscordEmbedBuilder
            {
                Description = "There are no tracks loaded",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(noPlayingTracks);
        }
        public static DiscordMessageBuilder NoTracksPlaying(InteractionContext msg)
        {
            var noPlayingTracks = new DiscordEmbedBuilder
            {
                Description = "There are no tracks loaded",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(noPlayingTracks);
        }
        public static DiscordEmbedBuilder NoTracksPlayingEmbed(InteractionContext msg)
        {
            return new DiscordEmbedBuilder
            {
                Description = "There are no tracks loaded",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
        }
        public static DiscordEmbedBuilder NoTracksPlayingEmbed(CommandContext msg)
        {
            return new DiscordEmbedBuilder
            {
                Description = "There are no tracks loaded",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
        }
        
        public static DiscordMessageBuilder IncorrectCommand(CommandContext msg, string usage)
        {
            var incorrectCommand = new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = $"**Usage:** {usage}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(incorrectCommand);
        }
        
        public static DiscordEmbedBuilder Queue(DiscordUser user)
        {
            string title = "Queue:";
            try
            {
                LavalinkTrack lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
            }
            catch (Exception exception)
            {
                title = "Queue is clear";
            }
            
            string totalQueue = "";
            for (int i = 0; i < Bot.Queue.Count; i++)
                totalQueue += $"{i + 1}. " + Bot.Queue[i].Title + "\n";

            return new DiscordEmbedBuilder
            {
                Title = title,
                Description = totalQueue,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = user.AvatarUrl,
                    Text = user.Username
                },
                Color = Bot.MainEmbedColor
            };
        }
        
        public static DiscordMessageBuilder InvalidChannel(CommandContext msg)
        {
            var invalidChannel = new DiscordEmbedBuilder
            {
                Description = "Not a valid voice channel",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.WarningColor
            };
            return new DiscordMessageBuilder()
                .AddEmbed(invalidChannel);
        }

        public static DiscordMessageBuilder TrackQueued(CommandContext msg)
        {
            string totalQueue = "";
            for (int i = 0; i < Bot.Queue.Count; i++)
                totalQueue += $"{i + 1}. " + Bot.Queue[i].Title + "\n";
            
            var trackQueued = new DiscordEmbedBuilder
            {
                Title = $"Track **{Bot.Queue[0].Title}** queued, position - {Bot.Queue.Count}",
                Description = $"Queue:\n{totalQueue}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = "Queued by " + msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
            
            return new DiscordMessageBuilder()
                .AddEmbed(trackQueued)
                .AddComponents(nextButton, queueButton);
        }
        public static DiscordMessageBuilder TrackQueued(InteractionContext msg)
        {
            string totalQueue = "";
            for (int i = 0; i < Bot.Queue.Count; i++)
                totalQueue += $"{i + 1}. " + Bot.Queue[i].Title + "\n";
            
            var trackQueued = new DiscordEmbedBuilder
            {
                Title = $"Track **{Bot.Queue[0].Title}** queued, position - {Bot.Queue.Count}",
                Description = $"Queue:\n{totalQueue}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = "Queued by " + msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
            
            return new DiscordMessageBuilder()
                .AddEmbed(trackQueued)
                .AddComponents(nextButton, queueButton);
        }
        public static DiscordEmbedBuilder TrackQueuedEmbed(InteractionContext msg)
        {
            string totalQueue = "";
            for (int i = 0; i < Bot.Queue.Count; i++)
                totalQueue += $"{i + 1}. " + Bot.Queue[i].Title + "\n";
            
            return new DiscordEmbedBuilder
            {
                Title = $"Track **{Bot.Queue[0].Title}** queued, position - {Bot.Queue.Count}",
                Description = $"Queue:\n{totalQueue}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = "Queued by " + msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
        }

        public static DiscordEmbedBuilder ClearQueueEmbed(DiscordUser user)
        {
            return new DiscordEmbedBuilder
            {
                Title = "Queue is clear",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = user.AvatarUrl,
                    Text = user.Username
                },
                Color = Bot.WarningColor
            };
        }

        public static DiscordEmbedBuilder ReactionRolesEmbed(DiscordClient client, DiscordGuild guild)
        {
            DiscordEmoji vibeEmoji = DiscordEmoji.FromName(client, ":vibe:");
            DiscordEmoji twitchRgbEmoji = DiscordEmoji.FromName(client, ":twitchrgb:");
            DiscordEmoji chelEmoji = DiscordEmoji.FromName(client, ":chel:");
            return new DiscordEmbedBuilder
            {
                Title = $"{vibeEmoji} Welcome, tap buttons to get roles {vibeEmoji}",
                Description = $"Roles list:\n{twitchRgbEmoji} - twitch follower(by having this reaction, u will get stream notifications)\n{chelEmoji} - default role for this server\n\nGL",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = guild.GetMemberAsync(906179696516026419).Result.DisplayName,
                    IconUrl = guild.GetMemberAsync(906179696516026419).Result.AvatarUrl
                },
                Color = Bot.MainEmbedColor
            };
        }

        public static DiscordEmbedBuilder StreamAnnouncementEmbed(CommandContext msg, string description)
        {
            return new DiscordEmbedBuilder
            {
                Title = "Stream online!",
                Description = $"{description} \nhttps://www.twitch.tv/itakash1",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = msg.Guild.GetMemberAsync(Bot.Id).Result.DisplayName,
                    IconUrl = msg.Guild.GetMemberAsync(Bot.Id).Result.AvatarUrl
                },
                ImageUrl = msg.Guild.GetMemberAsync(857687574281453598).Result.AvatarUrl,
                Color = Bot.MainEmbedColor
            };
        }
        public static DiscordEmbedBuilder StreamAnnouncementEmbed(InteractionContext msg, string description)
        {
            return new DiscordEmbedBuilder
            {
                Title = "Stream online!",
                Description = $"{description} \nhttps://www.twitch.tv/itakash1",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = msg.Guild.GetMemberAsync(Bot.Id).Result.DisplayName,
                    IconUrl = msg.Guild.GetMemberAsync(Bot.Id).Result.AvatarUrl
                },
                ImageUrl = msg.Guild.GetMemberAsync(857687574281453598).Result.AvatarUrl,
                Color = Bot.MainEmbedColor
            };
        }

        public static DiscordEmbedBuilder HelpEmbed(DiscordUser user, string command)
        {
            if (command == "")
            {
                return new DiscordEmbedBuilder
                {
                    Title = "Help",
                    Description = "List of all server commands.\n" +
                                  "Prefix for this server: `-`, but you can use slash commands(just type `/`)\n" +
                                  "Use `-help <command>` to see certain command description\n" +
                                  "\n**Commands**\n" +
                                  "`avatar`, `invite-link`, `random`" +
                                  "\n**Music commands**\n" +
                                  "`join`, `play`, `stop`, `pause`, `resume`, `np`, `skip`, `queue`, `clear-queue`" +
                                  "\n**Admin commands**\n" +
                                  "`ban`, `kick`, `clear`, `quit`, `add-role`, `remove-role`, `mute`, `unmute`, `embed`, `reaction`, `activity`, `reaction-role-embed`, `stream`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = user.AvatarUrl,
                        Text = user.Username
                    },
                    Color = Bot.MainEmbedColor
                };
            }
            else
            {
                string helpEmbedDescription = "You try to use `-help <command>` with unknown command";
                string helpEmbedCommandUsage = "-help <command>";
                switch (command)
                {
                    case "avatar":
                        helpEmbedDescription = "Send you embed with users avatar";
                        helpEmbedCommandUsage = "-avatar <user>";
                        break;
                    case "invite-link":
                        helpEmbedDescription = "Send you invite link for this bot";
                        helpEmbedCommandUsage = "-invite-link";
                        break;
                    case "random":
                        helpEmbedDescription = "Send you random value in your range from min to max value";
                        helpEmbedCommandUsage = "-random <min> <max>";
                        break;
                    case "play":
                        helpEmbedDescription = "Start playing music from youtube by link or search request";
                        helpEmbedCommandUsage = "-play <link to track(youtube, soundcloud, twitch, spotify)> or <search>";
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
                    case "reaction":
                        helpEmbedDescription = "Create reaction on message with your emoji";
                        helpEmbedCommandUsage = "-reaction <message id> <emoji>";
                        break;
                    case "np":
                        helpEmbedDescription = "Send currently playing track";
                        helpEmbedCommandUsage = "-np";
                        break;
                    case "skip":
                        helpEmbedDescription = "Skip track to next in queue";
                        helpEmbedCommandUsage = "-skip";
                        break;
                    case "queue":
                        helpEmbedDescription = "Send queue list to current channel";
                        helpEmbedCommandUsage = "-queue";
                        break;
                    case "clear-queue":
                        helpEmbedDescription = "Clear queue list";
                        helpEmbedCommandUsage = "-clear-queue";
                        break;
                    case "quit":
                        helpEmbedDescription = "Quit from any voice channel";
                        helpEmbedCommandUsage = "-quit";
                        break;
                    case "add-role":
                        helpEmbedDescription = "Add role to mentioned user";
                        helpEmbedCommandUsage = "-add-role <user> <role>";
                        break;
                    case "remove-role":
                        helpEmbedDescription = "Remove role from mentioned user";
                        helpEmbedCommandUsage = "-remove-role <user> <role>";
                        break;
                    case "mute":
                        helpEmbedDescription = "Mute mentioned user";
                        helpEmbedCommandUsage = "-mute <user>";
                        break;
                    case "unmute":
                        helpEmbedDescription = "Unmute mentioned user";
                        helpEmbedCommandUsage = "-unmute <user>";
                        break;
                    case "activity":
                        helpEmbedDescription = "Change bot activity type";
                        helpEmbedCommandUsage = "-activity <playing / streaming>";
                        break;
                    case "reaction-role-embed":
                        helpEmbedDescription = "Send embed with buttons, press them to take roles";
                        helpEmbedCommandUsage = "-reaction-role-embed";
                        break;
                    case "stream":
                        helpEmbedDescription = "Send stream announcement";
                        helpEmbedCommandUsage = "-stream <description>";
                        break;
                }
                return new DiscordEmbedBuilder
                {
                    Title = "Help",
                    Description = $"**Description:**\n{helpEmbedDescription}\n**Usage:**\n`{helpEmbedCommandUsage}`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = user.AvatarUrl,
                        Text = user.Username
                    },
                    Color = Bot.MainEmbedColor
                };
            }
        }

        public static DiscordEmbedBuilder TrackRepeatEmbed(DiscordUser user)
        {
            string totalQueue = "";
            for (int i = 0; i < Bot.Queue.Count; i++)
                totalQueue += $"{i + 1}. " + Bot.Queue[i].Title + "\n";

            return new DiscordEmbedBuilder
            {
                Title = "Track will repeat",
                Description = $"**Queue:**\n{totalQueue}",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = user.AvatarUrl,
                    Text = $"Ordered by {user.Username}"
                },
                Color = Bot.MainEmbedColor
            };
        }
    }
}