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
        
    }
}