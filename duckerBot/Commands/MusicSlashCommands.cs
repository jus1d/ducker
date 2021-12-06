﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using SpotifyAPI.Web;

namespace duckerBot
{
    public partial class SlashCommands
    {
        [SlashCommand("join", "Join your voice channel")]
        public async Task Join(InteractionContext msg)
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
            await node.ConnectAsync(msg.Member.VoiceState.Channel);
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }

        [SlashCommand("quit", "Quit voice channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Quit(InteractionContext msg)
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
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }
        
        [SlashCommand("play", "Start playing track")]
        public async Task Play(InteractionContext msg, [Option("search", "Track name or url to play")] string search = null)
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

            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = await node.ConnectAsync(msg.Member.VoiceState.Channel);
            if (connection == null)
            {
                await duckerBot.Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            
            if (connection.CurrentState.CurrentTrack == null)
            {
                Uri url;
                if (Uri.TryCreate(search, UriKind.Absolute, out url)) // by url
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
                if (Uri.TryCreate(search, UriKind.Absolute, out url)) // by url
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
    }
}