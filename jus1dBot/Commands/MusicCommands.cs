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

namespace jus1dBot
{
    public partial class Commands : BaseCommandModule
    {
        // define music channel name
        private string MusicChannelName = "🎶music";
        
        
        // -join
        [Command("join"), Description("bot joined to your voice channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Join(CommandContext msg)
        {
            DiscordChannel channel = msg.Member.VoiceState.Channel;
            
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await msg.Channel.SendMessageAsync("Connection is not established");
                return;
            }
            
            var node = lava.ConnectedNodes.Values.First();
            
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync("You are not in a voice channel.");
                return;
            }
            
            await node.ConnectAsync(channel);
        }
        
        
        // -join channel (admin perms required)
        [Command("join"), Description("bot joined to tagged voice channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Join(CommandContext msg, [Description("voice channel")] DiscordChannel channel)
        {
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await msg.Channel.SendMessageAsync("Connection is not established");
                return;
            }
            
            var node = lava.ConnectedNodes.Values.First();

            await node.ConnectAsync(channel);
        }


        // -play url
        [Command("play"), Description("bot joined to your voice, and playing video or track by your search query")]
        public async Task Play(CommandContext msg, [Description("URL")] Uri url)
        {
            if (msg.Channel.Name != MusicChannelName)
                return;
            
            await Join(msg);
            Thread.Sleep(1000);
            
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync("You are not in a voice channel.");
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (conn == null)
            {
                await msg.Channel.SendMessageAsync("ya is not connected.");
                return;
            }
            var loadResult = await node.Rest.GetTracksAsync(url);
            var track = loadResult.Tracks.First();
            await conn.PlayAsync(track);
            
            var playEmbed = new DiscordEmbedBuilder
            {
                Title = "Now playing",
                Description = track.Title + $"\nURL: {url} \n[ordered by {msg.Member.Mention}]",
                Color = DiscordColor.Azure
            };
            
            await msg.Channel.SendMessageAsync(playEmbed);
        }
        
        
        // -play search
        [Command("play"), Description("bot joined to your voice and playing video by your search query")]
        public async Task Play(CommandContext msg, [Description("search query")] string search)
        {
            if (msg.Channel.Name != MusicChannelName)
                return;
            
            await Join(msg);
            Thread.Sleep(1000);

            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync("You are not in a voice channel.");
                return;
            }

            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(msg.Member.VoiceState.Guild);

            if (conn == null)
            {
                await msg.Channel.SendMessageAsync("ya is not connected.");
                return;
            }

            var loadResult = await node.Rest.GetTracksAsync(search);

            if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed 
                || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
            {
                await msg.Channel.SendMessageAsync($"Track search failed for {search}.");
                return;
            }

            var track = loadResult.Tracks.First();

            await conn.PlayAsync(track);
            
            var playEmbed = new DiscordEmbedBuilder
            {
                Title = "Now playing",
                Description = track.Title + $"\nURL: {track.Uri} \n[ordered by {msg.Member.Mention}]",
                Color = DiscordColor.Azure
            };

            await msg.Channel.SendMessageAsync(playEmbed);
        }
        
        
        // -play (resume)
        [Command("play"), Description("resume playing music")]
        public async Task Play(CommandContext msg)
        {
            if (msg.Channel.Name != MusicChannelName)
                return;
            
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync("You are not in a voice channel.");
                return;
            }

            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(msg.Member.VoiceState.Guild);

            if (conn == null)
            {
                await msg.Channel.SendMessageAsync("Lavalink is not connected.");
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync("There are no tracks loaded.");
                return;
            }

            await conn.ResumeAsync();
        }
        
        
        // -pause
        [Command("pause"), Description("pause playing music")]
        public async Task Pause(CommandContext msg)
        {
            if (msg.Channel.Name != MusicChannelName)
                return;
            
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync("You are not in a voice channel.");
                return;
            }

            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var conn = node.GetGuildConnection(msg.Member.VoiceState.Guild);

            if (conn == null)
            {
                await msg.Channel.SendMessageAsync("Lavalink is not connected.");
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync("There are no tracks loaded.");
                return;
            }

            await conn.PauseAsync();
        }

        [Command("pause"), Description("pause playing music")]
        public async Task Pause(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Name != MusicChannelName)
                return;

            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** -pause",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }

        
        // -stop
        [Command("stop"), Description("permanently stop bot playing and bot quit")]
        public async Task Stop(CommandContext msg)
        {
            if (msg.Channel.Name != MusicChannelName)
                return;

            await Quit(msg, msg.Member.VoiceState.Channel);
        }

        [Command("stop"), Description("stop music, and kicks bof from voice channel")]
        public async Task Stop(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Name != MusicChannelName)
                return;

            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Missing argument", 
                Description = $"**Usage:** -stop [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
    }
}