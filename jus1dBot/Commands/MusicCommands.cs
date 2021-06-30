using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public partial class Commands
    {
        // -join
        [Command("join")]
        [RequireRoles(RoleCheckMode.All, "admin")]
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
        [Command("join")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task Join(CommandContext msg, DiscordChannel channel)
        {
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

        // -quit
        [Command("quit")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task Quit(CommandContext msg, DiscordChannel channel)
        {
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await msg.Channel.SendMessageAsync("Connection is not established");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();

            if (channel.Type != ChannelType.Voice)
            {
                await msg.Channel.SendMessageAsync("Not a valid voice channel.");
                return;
            }

            var conn = node.GetGuildConnection(channel.Guild);

            if (conn == null)
            {
                await msg.Channel.SendMessageAsync("I'm is not connected.");
                return;
            }

            await conn.DisconnectAsync();
            await msg.Channel.SendMessageAsync($"Left {channel.Name}!");
        }
        [Command("quit")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task Quit(CommandContext msg)
        {
            DiscordChannel channel = msg.Member.VoiceState.Channel;
            
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await msg.Channel.SendMessageAsync("Connection is not established");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();

            if (channel.Type != ChannelType.Voice)
            {
                await msg.Channel.SendMessageAsync("Not a valid voice channel.");
                return;
            }

            var conn = node.GetGuildConnection(channel.Guild);

            if (conn == null)
            {
                await msg.Channel.SendMessageAsync("I'm is not connected.");
                return;
            }

            await conn.DisconnectAsync();
        }
        
        // -play
        [Command("play")]
        public async Task Play(CommandContext msg, [RemainingText] string search)
        {
            Join(msg);
            
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

            await msg.Channel.SendMessageAsync($"Now playing {track.Title}!");
        }
        [Command("play")]
        public async Task Play(CommandContext msg, Uri url)
        {
            await Join(msg);
            
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

            await msg.Channel.SendMessageAsync($"Now playing {track.Title}!\n {url}");
        }
        
        // -pause
        [Command("pause")]
        public async Task Pause(CommandContext msg)
        {
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

        [Command("stop")]
        public async Task Stop(CommandContext msg)
        {
            DiscordChannel channel = msg.Member.VoiceState.Channel;
            await Quit(msg, channel);
        }
    }
}