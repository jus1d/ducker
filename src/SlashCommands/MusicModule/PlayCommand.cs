using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using ducker.Config;
using ducker.Database;
using SpotifyAPI.Web;

namespace ducker.SlashCommands.MusicModule
{
    public class PlayCommand
    {
        [SlashCommand("play", "Start playing track")]
        public async Task Play(InteractionContext msg, [Option("search", "Track name or url to play")] string search)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = DB.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = DB.GetCmdChannel(msg.Guild.Id);
            
            if (musicChannelIdFromDb == 0)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoMusicChannelConfigured(msg.User));
                return;
            }
            if (msg.Channel.Id != musicChannelIdFromDb && msg.Channel.Id != cmdChannelIdFromDb)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg, musicChannelIdFromDb));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = await node.ConnectAsync(msg.Member.VoiceState.Channel);
            
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }

            if (connection.CurrentState.CurrentTrack == null)
            {
                Uri url;
                if (Uri.TryCreate(search, UriKind.Absolute, out url))
                {
                    if (url.Authority == "open.spotify.com")
                    {

                        LavalinkTrack track = new LavalinkTrack();
                        if (url.LocalPath[Range.EndAt(7)] == "/track/")
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

                            string searchBySpotifyName = trackSpotify.Name + " - " + authors;
                            var loadResult = await node.Rest.GetTracksAsync(searchBySpotifyName);
                            track = loadResult.Tracks.First();
                            await connection.PlayAsync(track);
                            
                            var playButton = new DiscordButtonComponent(ButtonStyle.Secondary, "play_button", $"Play", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":arrow_forward:")));
                            var pauseButton = new DiscordButtonComponent(ButtonStyle.Secondary, "pause_button", $"Pause", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":pause_button:")));
                            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                            await msg.Channel.SendMessageAsync(new DiscordMessageBuilder().AddEmbed(ducker.Embed.NowPlayingEmbed(track, msg.User)).AddComponents(playButton, pauseButton, nextButton, queueButton));
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
                        await connection.PlayAsync(track);
                        var playButton = new DiscordButtonComponent(ButtonStyle.Secondary, "play_button", $"Play", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":arrow_forward:")));
                        var pauseButton = new DiscordButtonComponent(ButtonStyle.Secondary, "pause_button", $"Pause", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":pause_button:")));
                        var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                        var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                        await msg.Channel.SendMessageAsync(new DiscordMessageBuilder().AddEmbed(ducker.Embed.NowPlayingEmbed(track, msg.User)).AddComponents(playButton, pauseButton, nextButton, queueButton));

                    }
                }
                else
                {
                    var loadResult = await node.Rest.GetTracksAsync(search);
                    if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                    {
                        await msg.Channel.SendMessageAsync(ducker.Embed.SearchFailedEmbed(msg, search));
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    await connection.PlayAsync(track);
                    
                    var playButton = new DiscordButtonComponent(ButtonStyle.Secondary, "play_button", $"Play", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":arrow_forward:")));
                    var pauseButton = new DiscordButtonComponent(ButtonStyle.Secondary, "pause_button", $"Pause", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":pause_button:")));
                    var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                    var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                    await msg.Channel.SendMessageAsync(new DiscordMessageBuilder()
                        .AddEmbed(ducker.Embed.NowPlayingEmbed(track, msg.User))
                        .AddComponents(playButton, pauseButton, nextButton, queueButton));

                }
            }
            else
            {
                Uri url;
                if (Uri.TryCreate(search, UriKind.Absolute, out url))
                {
                    if (url.Authority == "open.spotify.com")
                    {
                        var config = SpotifyClientConfig.CreateDefault();
                        var request = new ClientCredentialsRequest(ConfigJson.GetConfigField().SpotifyId, ConfigJson.GetConfigField().SpotifySecret);
                        var response = await new OAuthClient(config).RequestToken(request);
                        var spotify = new SpotifyClient(config.WithToken(response.AccessToken));

                        LavalinkTrack track;
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

                            string searchBySpotifyName = trackSpotify.Name + " - " + authors;
                            var loadResult = await node.Rest.GetTracksAsync(searchBySpotifyName);
                            track = loadResult.Tracks.First();
                            Bot.Queue.Add(track);
                            await msg.Channel.SendMessageAsync(ducker.Embed.TrackQueuedEmbed(msg));
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
                        
                        
                        var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                        var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                        await msg.Channel.SendMessageAsync(new DiscordMessageBuilder()
                            .AddEmbed(ducker.Embed.NowPlayingEmbed(track, msg.User))
                            .AddComponents(nextButton, queueButton));

                    }
                    else 
                    {
                        var loadResult = await node.Rest.GetTracksAsync(url);
                        var track = loadResult.Tracks.First();
                        Bot.Queue.Add(track);
                        var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                        var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                        await msg.Channel.SendMessageAsync(new DiscordMessageBuilder()
                            .AddEmbed(ducker.Embed.NowPlayingEmbed(track, msg.User))
                            .AddComponents(nextButton, queueButton));

                    }
                }
                else
                {
                    var loadResult = await node.Rest.GetTracksAsync(search);

                    if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                    {
                        await msg.Channel.SendMessageAsync(ducker.Embed.SearchFailedEmbed(msg, search));
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    Bot.Queue.Add(track);
                    var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                    var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                    await msg.Channel.SendMessageAsync(new DiscordMessageBuilder()
                        .AddEmbed(ducker.Embed.NowPlayingEmbed(track, msg.User))
                        .AddComponents(nextButton, queueButton));

                }
            }
        }
    }
}