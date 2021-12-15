using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using ducker.Attributes;
using ducker.Config;
using SpotifyAPI.Web;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("play"), 
         Description("Start playing music from youtube, spotify or soundcloud by link or search request"),
         Aliases("p"),
         RequireMusicChannel]
        public async Task PlayCommand(CommandContext msg, [RemainingText] string input) 
        {
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
            if (musicChannelIdFromDb == 0)
            {
                await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));
                return;
            }
            if (msg.Channel.Id != musicChannelIdFromDb && msg.Channel.Id != cmdChannelIdFromDb)
            {
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelIdFromDb));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = await node.ConnectAsync(msg.Member.VoiceState.Channel);
            if (connection == null)
            {
                await Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }

            if (connection.CurrentState.CurrentTrack == null)
            {
                Uri url;
                if (Uri.TryCreate(input, UriKind.Absolute, out url))
                {
                    if (url.Authority == "open.spotify.com")
                    {
                        var config = SpotifyClientConfig.CreateDefault();
                        var request = new ClientCredentialsRequest(ConfigJson.GetConfigField().SpotifyId, ConfigJson.GetConfigField().SpotifySecret);
                        var response = await new OAuthClient(config).RequestToken(request);
                        var spotify = new SpotifyClient(config.WithToken(response.AccessToken));

                        LavalinkTrack track = new LavalinkTrack();
                        if (url.LocalPath[Range.EndAt(7)] == "/track/")
                        {
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
                            var loadResult = await node.Rest.GetTracksAsync(search);
                            track = loadResult.Tracks.First();
                            await connection.PlayAsync(track);
                        }
                        else
                        {
                            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                            {
                                Description = "Episodes and playlists will available in next version",
                                Footer = new DiscordEmbedBuilder.EmbedFooter
                                {
                                    IconUrl = msg.User.AvatarUrl,
                                    Text = msg.User.Username
                                },
                                Color = Bot.MainEmbedColor
                            });
                            return;
                        }
                        
                        await Embed.NowPlaying(msg.Client, track, msg.User).SendAsync(msg.Channel);
                    }
                    else 
                    {
                        var loadResult = await node.Rest.GetTracksAsync(url);
                        var track = loadResult.Tracks.First();
                        await connection.PlayAsync(track);
                        await Embed.NowPlaying(msg.Client, track, msg.User).SendAsync(msg.Channel);
                    }
                }
                else // by search
                {
                    string search = input;

                    var loadResult = await node.Rest.GetTracksAsync(search);

                    if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                    {
                        await Embed.SearchFailed(msg, search).SendAsync(msg.Channel);
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    await connection.PlayAsync(track);
                    await Embed.NowPlaying(msg.Client, track, msg.User).SendAsync(msg.Channel);
                }
            }
            else
            {
                Uri url;
                if (Uri.TryCreate(input, UriKind.Absolute, out url)) // by url
                {
                    if (url.Authority == "open.spotify.com")
                    {
                        var config = SpotifyClientConfig.CreateDefault();
                        var request = new ClientCredentialsRequest(ConfigJson.GetConfigField().SpotifyId, ConfigJson.GetConfigField().SpotifySecret);
                        var response = await new OAuthClient(config).RequestToken(request);
                        var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
                        
                        LavalinkTrack track = new LavalinkTrack();
                        if (url.LocalPath[Range.EndAt(7)] == "/track/")
                        {
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
                            var loadResult = await node.Rest.GetTracksAsync(search);
                            track = loadResult.Tracks.First();
                            Bot.Queue.Add(track);
                            await Embed.TrackQueued(msg).SendAsync(msg.Channel);
                        }
                        else
                        {
                            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                            {
                                Description = "Episodes and playlists will available in next version",
                                Footer = new DiscordEmbedBuilder.EmbedFooter
                                {
                                    IconUrl = msg.User.AvatarUrl,
                                    Text = msg.User.Username
                                },
                                Color = Bot.MainEmbedColor
                            });
                            return;
                        }
                    }
                    else 
                    {
                        var loadResult = await node.Rest.GetTracksAsync(url);
                        var track = loadResult.Tracks.First();
                        Bot.Queue.Add(track);
                        await Embed.TrackQueued(msg).SendAsync(msg.Channel);
                    }
                }
                else
                {
                    string search = input;

                    var loadResult = await node.Rest.GetTracksAsync(search);

                    if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                    {
                        await Embed.SearchFailed(msg, search).SendAsync(msg.Channel);
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    Bot.Queue.Add(track);
                    await Embed.TrackQueued(msg).SendAsync(msg.Channel);
                }
            }
        }
    }
}