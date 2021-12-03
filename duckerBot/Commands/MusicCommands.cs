using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using SpotifyAPI.Web;

namespace duckerBot
{
    public partial class Commands : BaseCommandModule
    {
        // -join
        [Command("join")]
        public async Task Join(CommandContext msg, DiscordChannel channel = null)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await duckerBot.Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (channel == null)
            {
                channel = msg.Member.VoiceState.Channel;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            await node.ConnectAsync(channel);
        }
        

        // -quit
        [Command("quit"), Aliases("leave", "q"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Quit(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await duckerBot.Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.DisconnectAsync();
        }
        
        
        // -resume
        [Command("resume")]
        public async Task Resume(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await duckerBot.Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection.CurrentState.CurrentTrack == null)
            {
                await duckerBot.Embed.NoTracksPlaying(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.ResumeAsync();
        }
        
        
        // -play
        [Command("play"), Aliases("p")]
        public async Task Play(CommandContext msg, params string[] input) 
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await duckerBot.Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            await Join(msg, msg.Member.VoiceState.Channel);
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await duckerBot.Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }

            if (connection.CurrentState.CurrentTrack == null)
            {
                Uri url;
                if (Uri.TryCreate(input[0], UriKind.Absolute, out url)) // by url
                {
                    if (url.Authority == "open.spotify.com")
                    {
                        var config = SpotifyClientConfig.CreateDefault();
                        var request = new ClientCredentialsRequest(ConfigJson.GetConfigField().SpotifyId, ConfigJson.GetConfigField().SpotifySecret);
                        var response = await new OAuthClient(config).RequestToken(request);
                        var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
                        var trackSpotify = await spotify.Tracks.Get(url.ToString()[Range.StartAt(31)][Range.EndAt(22)]);
                        
                        string authors = "";
                        for (int i = 0; i < trackSpotify.Artists.Count; i++)
                        {
                            if (trackSpotify.Artists.Count == 1 || trackSpotify.Artists.Count == i + 1)
                            {
                                authors += trackSpotify.Artists[i].Name;
                            }
                            else
                            {
                                authors += trackSpotify.Artists[i].Name + ", ";
                            }
                        }

                        string search = trackSpotify.Name + " - " + authors;
                        await Join(msg, msg.Member.VoiceState.Channel);
                        var loadResult = await node.Rest.GetTracksAsync(search);
                        var track = loadResult.Tracks.First();
                        await connection.PlayAsync(track);
                        await duckerBot.Embed.NowPlaying(msg.Client, track, msg.User).SendAsync(msg.Channel);
                    }
                    else 
                    {
                        var loadResult = await node.Rest.GetTracksAsync(url);
                        var track = loadResult.Tracks.First();
                        await connection.PlayAsync(track);
                        await duckerBot.Embed.NowPlaying(msg.Client, track, msg.User).SendAsync(msg.Channel);
                    }
                }
                else // by search
                {
                    string search = "";
                    for (int i = 0; i < input.Length; i++)
                        search += input[i] + " ";

                    var loadResult = await node.Rest.GetTracksAsync(search);

                    if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                    {
                        await duckerBot.Embed.SearchFailed(msg, search).SendAsync(msg.Channel);
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    await connection.PlayAsync(track);
                    await duckerBot.Embed.NowPlaying(msg.Client, track, msg.User).SendAsync(msg.Channel);
                }
            }
            else
            {
                Uri url;
                if (Uri.TryCreate(input[0], UriKind.Absolute, out url)) // by url
                {
                    if (url.Authority == "open.spotify.com")
                    {
                        var config = SpotifyClientConfig.CreateDefault();
                        var request = new ClientCredentialsRequest(ConfigJson.GetConfigField().SpotifyId, ConfigJson.GetConfigField().SpotifySecret);
                        var response = await new OAuthClient(config).RequestToken(request);
                        var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
                        var trackSpotify = await spotify.Tracks.Get(url.ToString()[Range.StartAt(31)][Range.EndAt(22)]);
                        
                        string authors = "";
                        for (int i = 0; i < trackSpotify.Artists.Count; i++)
                        {
                            if (trackSpotify.Artists.Count == 1 || trackSpotify.Artists.Count == i + 1)
                            {
                                authors += trackSpotify.Artists[i].Name;
                            }
                            else
                            {
                                authors += trackSpotify.Artists[i].Name + ", ";
                            }
                        }

                        string search = trackSpotify.Name + " - " + authors;
                        await Join(msg, msg.Member.VoiceState.Channel);
                        var loadResult = await node.Rest.GetTracksAsync(search);
                        var track = loadResult.Tracks.First();
                        Bot.Queue.Add(track);
                        await duckerBot.Embed.TrackQueued(msg, track).SendAsync(msg.Channel);
                    }
                    else 
                    {
                        var loadResult = await node.Rest.GetTracksAsync(url);
                        var track = loadResult.Tracks.First();
                        Bot.Queue.Add(track);
                        await duckerBot.Embed.TrackQueued(msg, track).SendAsync(msg.Channel);
                    }
                }
                else // by search
                {
                    string search = "";
                    for (int i = 0; i < input.Length; i++)
                        search += input[i] + " ";

                    var loadResult = await node.Rest.GetTracksAsync(search);

                    if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                    {
                        await duckerBot.Embed.SearchFailed(msg, search).SendAsync(msg.Channel);
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    Bot.Queue.Add(track);
                    await duckerBot.Embed.TrackQueued(msg, track).SendAsync(msg.Channel);
                }
            }
        }


        // -pause
        [Command("pause")]
        public async Task Pause(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await duckerBot.Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection == null)
            {
                await duckerBot.Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            if (connection.CurrentState.CurrentTrack == null)
            {
                await duckerBot.Embed.NoTracksPlaying(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.PauseAsync();
        }

        [Command("pause")]
        public async Task Pause(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            await duckerBot.Embed.IncorrectCommand(msg, "-pause").SendAsync(msg.Channel);
        }
        
        
        // -skip
        [Command("skip")]
        public async Task Skip(CommandContext msg)
        {
            try
            {
                LavalinkTrack lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
            }
            catch (Exception exception)
            {
                await msg.Channel.SendMessageAsync(duckerBot.Embed.ClearQueue(msg.User));
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            await connection.StopAsync();
        }
        
        [Command("skip")]
        public async Task Skip(CommandContext msg, params string[] text)
        {
            await duckerBot.Embed.IncorrectCommand(msg, "-skip").SendAsync(msg.Channel);
        }
        
        
        // -queue
        [Command("queue")]
        public async Task Queue(CommandContext msg)
        {
            await msg.Channel.SendMessageAsync(duckerBot.Embed.Queue(msg.Client, msg.User));
        }

        [Command("clearqueue")]
        public async Task ClearQueue(CommandContext msg)
        {
            Bot.Queue.Clear();
            await msg.Channel.SendMessageAsync(duckerBot.Embed.ClearQueue(msg.User));
        }
        
        
        // -stop
        [Command("stop"), Aliases("s")]
        public async Task Stop(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Channel.Guild);

            if (connection == null)
            {
                await duckerBot.Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.DisconnectAsync();
        }

        [Command("stop")]
        public async Task Stop(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            await duckerBot.Embed.IncorrectCommand(msg, "-stop").SendAsync(msg.Channel);
        }


        [Command("phonk"), Aliases("ph")]
        public async Task Phonk(CommandContext msg)
        {
            await Play(msg, "https://www.youtube.com/watch?v=3lwdObInlqU&ab_channel=Memphis66.6");
        }
    }
}