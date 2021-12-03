﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;

namespace duckerBot
{
    public class Embed
    {
        public static DiscordMessageBuilder NowPlaying(DiscordClient client, DiscordUser user, LavalinkTrack track)
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
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Next", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":track_next:")));
            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":page_facing_up:")));
            
            return new DiscordMessageBuilder()
                .AddEmbed(playEmbed)
                .AddComponents(pauseButton, playButton, nextButton, queueButton);
        }
        
        public static DiscordMessageBuilder TrackSkipped(DiscordClient client, DiscordUser user, LavalinkTrack track)
        {
            var skipEmbed = new DiscordEmbedBuilder
            {
                Title = "Track skipped, now playing",
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
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Next", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":track_next:")));
            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":page_facing_up:")));
            
            return new DiscordMessageBuilder()
                .AddEmbed(skipEmbed)
                .AddComponents(pauseButton, playButton, nextButton, queueButton);
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
        
        public static DiscordMessageBuilder Queue(DiscordClient client, DiscordUser user)
        {
            string totalQueue = "";
            for (int i = 0; i < Bot.Queue.Count; i++)
                totalQueue += $"{i + 1}. " + Bot.Queue[i].Title + "\n";

            var e = new DiscordEmbedBuilder
            {
                Title = "Queue:",
                Description = totalQueue,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = user.AvatarUrl,
                    Text = user.Username
                },
                Color = Bot.MainEmbedColor
            };
            
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Next", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":track_next:")));
            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":page_facing_up:")));

            return new DiscordMessageBuilder()
                .AddEmbed(e)
                .AddComponents(nextButton, queueButton);
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

        public static DiscordMessageBuilder TrackQueued(CommandContext msg, LavalinkTrack track)
        {
            var trackQueued = new DiscordEmbedBuilder
            {
                Title = $"Track queued, position - {Bot.Queue.Count}",
                Description = $"[{track.Title}]({track.Uri})",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = "Queued by " + msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Next", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
            
            return new DiscordMessageBuilder()
                .AddEmbed(trackQueued)
                .AddComponents(nextButton, queueButton);
        }
        
    }
}