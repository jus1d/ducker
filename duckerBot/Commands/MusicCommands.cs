using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.VisualBasic;

namespace duckerBot
{
    public partial class Commands : BaseCommandModule
    {
        public ulong musicChannelId = 816659808627195915; // only for my own server)

        // -join
        [Command("join"),
         Description(("bot joined to your voice channel")),
         RequirePermissions(Permissions.Administrator)]
        public async Task Join(CommandContext msg, DiscordChannel channel = null)
        {
            if (channel == null)
            { 
                channel = msg.Member.VoiceState.Channel;
            }
            
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                var noConnectionEmbed = new DiscordEmbedBuilder
                {
                    Description = "Connection is not established",
                    Color = warningColor
                };
                noConnectionEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
                return;
            }
            
            var node = lava.ConnectedNodes.Values.First();
            
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                var noInVoice = new DiscordEmbedBuilder
                {
                    Description = "You are not in a voice channel",
                    Color = warningColor
                };
                noInVoice.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noInVoice);
                return;
            }
            
            await node.ConnectAsync(channel);
        }
        

        // -quit
        [Command("quit"),
         Description("bot quit from your channel"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Quit(CommandContext msg)
        {
            DiscordChannel channel = msg.Member.VoiceState.Channel;
            
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                var noConnectionEmbed = new DiscordEmbedBuilder
                {
                    Description = "Connection is not established",
                    Color = warningColor
                };
                noConnectionEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
                return;
            }
            var node = lava.ConnectedNodes.Values.First();
            if (channel.Type != ChannelType.Voice)
            {
                var invalidChannel = new DiscordEmbedBuilder
                {
                    Description = "Not a valid voice channel",
                    Color = warningColor
                };
                invalidChannel.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(invalidChannel);
                return;
            }
            var connection = node.GetGuildConnection(channel.Guild);
            if (connection == null)
            {
                var noConnectionEmbed = new DiscordEmbedBuilder
                {
                    Description = "I'm is not connected",
                    Color = warningColor
                };
                noConnectionEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
                return;
            }
            await connection.DisconnectAsync();
        }
        
        
        // -play url
        [Command("play")]
        [Description("bot joined to your voice, and playing video or track by your search query")]
        public async Task Play(CommandContext msg, [Description("URL")] Uri url)
        {
            if (msg.Channel.Id != musicChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(musicChannelId).Id}>",
                    Color = warningColor
                };
                incorrectChannel.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            await Join(msg);
            
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync("You are not in a voice channel.");
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                var noConnectionEmbed = new DiscordEmbedBuilder
                {
                    Description = "I'm is not connected",
                    Color = warningColor
                };
                noConnectionEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
                return;
            }
            var loadResult = await node.Rest.GetTracksAsync(url);
            var track = loadResult.Tracks.First();
            await connection.PlayAsync(track);
            
            var playEmbed = new DiscordEmbedBuilder
            {
                Title = "Now playing",
                Description = $"[{track.Title}]({url})",
                Color = mainEmbedColor
            };
            playEmbed.WithFooter("Ordered by " + msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(playEmbed);
        }
        
        // -play search
        [Command("play")]
        [Description("bot joined to your voice and playing video by your search query")]
        public async Task Play(CommandContext msg, [Description("search query")] params string[] searchInput)
        {
            if (msg.Channel.Id != musicChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(musicChannelId).Id}>",
                    Color = warningColor
                };
                incorrectChannel.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            await Join(msg);

            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                var noVoice = new DiscordEmbedBuilder
                {
                    Description = "You are not in a voice channel",
                    Color = warningColor
                };
                noVoice.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noVoice);
                return;
            }

            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);

            if (connection == null)
            {
                var noConnectionEmbed = new DiscordEmbedBuilder
                {
                    Description = "I'm is not connected",
                    Color = warningColor
                };
                noConnectionEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
                return;
            }

            string search = "";
            for (int i = 0; i < searchInput.Length; i++)
            {
                search += searchInput[i] + " ";
            }

            var loadResult = await node.Rest.GetTracksAsync(search);

            if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed 
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                var searchFaled = new DiscordEmbedBuilder
                {
                    Description = $"Track search failed for: {search}",
                    Color = mainEmbedColor
                };
                searchFaled.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(searchFaled);
                return;
            }

            var track = loadResult.Tracks.First();

            await connection.PlayAsync(track);
            
            var playEmbed = new DiscordEmbedBuilder
            {
                Title = "Now playing",
                Description = $"[{track.Title}]({track.Uri})",
                Color = mainEmbedColor
            };
            playEmbed.WithFooter("Ordered by " + msg.User.Username, msg.User.AvatarUrl);

            await msg.Channel.SendMessageAsync(playEmbed);
        }
        
        // -play (resume)
        [Command("play"), 
         Description("resume playing music")]
        public async Task Play(CommandContext msg)
        {
            if (msg.Channel.Id != musicChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(musicChannelId).Id}>",
                    Color = warningColor
                };
                incorrectChannel.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                var incorrectMusicCommand = new DiscordEmbedBuilder
                {
                    Title = "You are not in a voice channel"
                };
                incorrectMusicCommand.WithFooter("Ordered by " + msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(incorrectMusicCommand);
                return;
            }

            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);

            if (connection.CurrentState.CurrentTrack == null)
            {
                var incorrectMusicCommand = new DiscordEmbedBuilder
                {
                    Title = "There are no tracks loaded"
                };
                incorrectMusicCommand.WithFooter("Ordered by " + msg.User.Username, msg.User.AvatarUrl);
                return;
            }

            await connection.ResumeAsync();
        }
        
        // -pause
        [Command("pause"), 
         Description("pause playing music")]
        public async Task Pause(CommandContext msg)
        {
            if (msg.Channel.Id != musicChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(musicChannelId).Id}>",
                    Color = warningColor
                };
                incorrectChannel.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                var noVoice = new DiscordEmbedBuilder
                {
                    Description = "You are not in a voice channel",
                    Color = warningColor
                };
                noVoice.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noVoice);
                return;
            }

            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);

            if (connection == null)
            {
                var noVoice = new DiscordEmbedBuilder
                {
                    Description = "You are not in a voice channel",
                    Color = warningColor
                };
                noVoice.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noVoice);
                return;
            }

            if (connection.CurrentState.CurrentTrack == null)
            {
                var noTracksLoaded = new DiscordEmbedBuilder
                {
                    Description = "There are no tracks loaded",
                    Color = warningColor
                };
                noTracksLoaded.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noTracksLoaded);
                return;
            }
            await connection.PauseAsync();
        }

        [Command("pause"), 
         Description("pause playing music")]
        public async Task Pause(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Id != musicChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(musicChannelId).Id}>",
                    Color = warningColor
                };
                incorrectChannel.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            var incorrectPauseCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** -pause",
                Color = incorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectPauseCommandEmbed);
        }
        
        
        // -stop
        [Command("stop"), 
         Description("permanently stop bot playing and bot quit")]
        public async Task Stop(CommandContext msg)
        {
            if (msg.Channel.Id != musicChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(musicChannelId).Id}>",
                    Color = warningColor
                };
                incorrectChannel.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                var noTracksLoaded = new DiscordEmbedBuilder
                {
                    Description = "Connection is not established",
                    Color = warningColor
                };
                noTracksLoaded.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noTracksLoaded);
                return;
            }
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Channel.Guild);

            if (connection == null)
            {
                var noTracksLoaded = new DiscordEmbedBuilder
                {
                    Description = "I'm is not connected.",
                    Color = warningColor
                };
                noTracksLoaded.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(noTracksLoaded);
                return;
            }
            await connection.DisconnectAsync();
        }

        [Command("stop"), 
         Description("stop music, and kicks bof from voice channel")]
        public async Task Stop(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Id != musicChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(musicChannelId).Id}>",
                    Color = warningColor
                };
                incorrectChannel.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }
            await Quit(msg);
        }
    }
}