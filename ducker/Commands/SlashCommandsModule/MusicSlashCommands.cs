using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using MySqlConnector;
using SpotifyAPI.Web;
using ducker;

namespace ducker
{
    public partial class SlashCommands
    {
        [SlashCommand("join", "Join your voice channel")]
        public async Task Join(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            await node.ConnectAsync(msg.Member.VoiceState.Channel);
        }

        [SlashCommand("quit", "Quit voice channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Quit(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            await connection.DisconnectAsync();
            Bot.Queue.Clear();
        }
        
        [SlashCommand("play", "Start playing track")]
        public async Task Play(InteractionContext msg, [Option("search", "Track name or url to play")] string search)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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

        [SlashCommand("np", "Display now playing track")]
        public async Task NowPlaying(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }

            await msg.Channel.SendMessageAsync(ducker.Embed.NowPlayingEmbed(connection.CurrentState.CurrentTrack,
                msg.User));
        }

        [SlashCommand("repeat", "Repeat current track")]
        public async Task Repeat(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            
            Bot.Queue.Insert(0, connection.CurrentState.CurrentTrack);
            await msg.Channel.SendMessageAsync(ducker.Embed.TrackRepeatEmbed(msg.User));
        }

        [SlashCommand("pause", "Pause current track")]
        public async Task Pause(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            await connection.PauseAsync();
        }

        [SlashCommand("resume", "Resume current track")]
        public async Task Resume(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            await connection.ResumeAsync();
        }

        [SlashCommand("skip", "Skip to the next track in queue")]
        public async Task Skip(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            
            try
            {
                LavalinkTrack lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
            }
            catch (Exception exception)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.ClearQueueEmbed(msg.User));
                return;
            }

            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            await connection.StopAsync();
        }

        [SlashCommand("queue", "Send queue list")]
        public async Task Queue(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            await msg.Channel.SendMessageAsync(ducker.Embed.Queue(msg.User));
        }
        
        [SlashCommand("clear-queue", "Clear queue list")]
        public async Task ClearQueue(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            Bot.Queue.Clear();
            await msg.Channel.SendMessageAsync(ducker.Embed.Queue(msg.User));
        }

        [SlashCommand("remove-from-queue", "Remove track from queue by his index")]
        public async Task RemoveFromQueue(InteractionContext msg, [Option("position", "Track's position in queue")] string positionInput)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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

            if (!int.TryParse(positionInput, out int position))
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.InvalidTrackPositionEmbed(msg.User));
                return;
            }
            try
            {
                Bot.Queue.RemoveAt(position - 1);
            }
            catch
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.InvalidTrackPositionEmbed(msg.User));
                return;
            }

            await msg.Channel.SendMessageAsync(ducker.Embed.TrackRemovedFromQueueEmbed(msg.User));
        }

        [SlashCommand("stop", "Stop playing")]
        public async Task Stop(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
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
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Channel.Guild);

            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(ducker.Embed.NoConnectionEmbed(msg));
                return;
            }
            await connection.DisconnectAsync();
        }

        [SlashCommand("phonk", "Start playing 24/7 Memphis Phonk Radio")]
        public async Task Phonk(InteractionContext msg)
        {
            await Play(msg, "https://www.youtube.com/watch?v=3lwdObInlqU&ab_channel=Memphis66.6");
        }
    }
}