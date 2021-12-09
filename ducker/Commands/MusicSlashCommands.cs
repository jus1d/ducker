using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using SpotifyAPI.Web;

namespace ducker
{
    public partial class SlashCommands
    {
        [SlashCommand("join", "Join your voice channel")]
        public async Task Join(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            await node.ConnectAsync(msg.Member.VoiceState.Channel);
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        }

        [SlashCommand("quit", "Quit voice channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Quit(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            await connection.DisconnectAsync();
            Bot.Queue.Clear();
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        }
        
        [SlashCommand("play", "Start playing track")]
        public async Task Play(InteractionContext msg, [Option("search", "Track name or url to play")] string search)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(ducker.Embed.IncorrectMusicChannelEmbed(msg)));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(ducker.Embed.NotInVoiceChannelEmbed(msg)));
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = await node.ConnectAsync(msg.Member.VoiceState.Channel);
            
            if (connection == null)
            {
                await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(ducker.Embed.NoConnectionEmbed(msg)));
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
                        }
                        else
                        {
                            await msg.CreateResponseAsync(new DiscordEmbedBuilder
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

                        var playButton = new DiscordButtonComponent(ButtonStyle.Secondary, "play_button", $"Play", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":arrow_forward:")));
                        var pauseButton = new DiscordButtonComponent(ButtonStyle.Secondary, "pause_button", $"Pause", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":pause_button:")));
                        var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                        var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                        await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .AddEmbed(ducker.Embed.NowPlayingEmbed(track, msg.User))
                                .AddComponents(playButton, pauseButton, nextButton, queueButton));
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
                        await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .AddEmbed(ducker.Embed.NowPlayingEmbed(track, msg.User))
                                .AddComponents(playButton, pauseButton, nextButton, queueButton));
                    }
                }
                else
                {
                    var loadResult = await node.Rest.GetTracksAsync(search);
                    if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                    {
                        await msg.CreateResponseAsync(ducker.Embed.SearchFailedEmbed(msg, search));
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    await connection.PlayAsync(track);
                    
                    var playButton = new DiscordButtonComponent(ButtonStyle.Secondary, "play_button", $"Play", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":arrow_forward:")));
                    var pauseButton = new DiscordButtonComponent(ButtonStyle.Secondary, "pause_button", $"Pause", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":pause_button:")));
                    var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                    var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                    await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
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
                        }
                        else
                        {
                            await msg.CreateResponseAsync(new DiscordEmbedBuilder
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
                        await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .AddEmbed(ducker.Embed.TrackQueuedEmbed(msg))
                                .AddComponents(nextButton, queueButton));
                    }
                    else 
                    {
                        var loadResult = await node.Rest.GetTracksAsync(url);
                        var track = loadResult.Tracks.First();
                        Bot.Queue.Add(track);
                        var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                        var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                        await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                            new DiscordInteractionResponseBuilder()
                                .AddEmbed(ducker.Embed.TrackQueuedEmbed(msg))
                                .AddComponents(nextButton, queueButton));
                    }
                }
                else
                {
                    var loadResult = await node.Rest.GetTracksAsync(search);

                    if (loadResult.LoadResultType == LavalinkLoadResultType.LoadFailed || loadResult.LoadResultType == LavalinkLoadResultType.NoMatches)
                    {
                        await msg.CreateResponseAsync(ducker.Embed.SearchFailedEmbed(msg, search));
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    Bot.Queue.Add(track);
                    var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Skip", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":track_next:")));
                    var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(msg.Client,":page_facing_up:")));
                    await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder()
                            .AddEmbed(ducker.Embed.TrackQueuedEmbed(msg))
                            .AddComponents(nextButton, queueButton));
                }
            }
        }

        [SlashCommand("np", "Display now playing track")]
        public async Task NowPlaying(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }

            await msg.CreateResponseAsync(ducker.Embed.NowPlayingEmbed(connection.CurrentState.CurrentTrack,
                msg.User));
        }

        [SlashCommand("repeat", "Repeat current track")]
        public async Task Repeat(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            
            Bot.Queue.Insert(0, connection.CurrentState.CurrentTrack);
            await msg.CreateResponseAsync(ducker.Embed.TrackRepeatEmbed(msg.User));
        }

        [SlashCommand("pause", "Pause current track")]
        public async Task Pause(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState.CurrentTrack == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            await connection.PauseAsync();
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        }

        [SlashCommand("resume", "Resume current track")]
        public async Task Resume(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection.CurrentState.CurrentTrack == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            await connection.ResumeAsync();
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        }

        [SlashCommand("skip", "Skip current track to next in queue")]
        public async Task Skip(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            
            try
            {
                LavalinkTrack lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
            }
            catch (Exception exception)
            {
                await msg.CreateResponseAsync(ducker.Embed.ClearQueueEmbed(msg.User));
                return;
            }

            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection.CurrentState.CurrentTrack == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            await connection.StopAsync();
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        }

        [SlashCommand("queue", "Send queue list")]
        public async Task Queue(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            await msg.CreateResponseAsync(ducker.Embed.Queue(msg.User));
        }
        
        [SlashCommand("clear-queue", "Clear queue list")]
        public async Task ClearQueue(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            Bot.Queue.Clear();
            await msg.CreateResponseAsync(ducker.Embed.Queue(msg.User));
        }

        [SlashCommand("stop", "Stop playing")]
        public async Task Stop(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.CreateResponseAsync(ducker.Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Channel.Guild);

            if (connection == null)
            {
                await msg.CreateResponseAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            await connection.DisconnectAsync();
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        }

        [SlashCommand("phonk", "Start playing 24/7 Memphis Phonk Radio")]
        public async Task Phonk(InteractionContext msg)
        {
            await Play(msg, "https://www.youtube.com/watch?v=3lwdObInlqU&ab_channel=Memphis66.6");
        }
    }
}