using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using SpotifyAPI.Web;

namespace ducker
{
    //[Group("MusicCommands")]
    public class MusicCommands : BaseCommandModule
    {
        // -join
        [Command("join")]
        public async Task Join(CommandContext msg, DiscordChannel channel = null)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (channel == null)
            {
                channel = msg.Member.VoiceState.Channel;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            await node.ConnectAsync(channel);
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }
        

        // -quit
        [Command("quit"), Aliases("leave", "q"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Quit(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.DisconnectAsync();
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }


        // -play
        [Command("play"), Aliases("p")]
        public async Task Play(CommandContext msg, params string[] input) 
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            await Join(msg, msg.Member.VoiceState.Channel);
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
                if (Uri.TryCreate(input[0], UriKind.Absolute, out url)) // by url
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
                    string search = "";
                    for (int i = 0; i < input.Length; i++)
                        search += input[i] + " ";

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
                if (Uri.TryCreate(input[0], UriKind.Absolute, out url)) // by url
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
                        await Embed.TrackQueued(msg).SendAsync(msg.Channel);
                    }
                    else 
                    {
                        var loadResult = await node.Rest.GetTracksAsync(url);
                        var track = loadResult.Tracks.First();
                        Bot.Queue.Add(track);
                        await Embed.TrackQueued(msg).SendAsync(msg.Channel);
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
                        await Embed.SearchFailed(msg, search).SendAsync(msg.Channel);
                        return;
                    }

                    var track = loadResult.Tracks.First();
                    Bot.Queue.Add(track);
                    await Embed.TrackQueued(msg).SendAsync(msg.Channel);
                }
            }
        }


        // -pause
        [Command("pause")]
        public async Task Pause(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection == null)
            {
                await Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            if (connection.CurrentState.CurrentTrack == null)
            {
                await Embed.NoTracksPlaying(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.PauseAsync();
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }

        [Command("pause")]
        public async Task Pause(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            await Embed.IncorrectCommand(msg, "-pause").SendAsync(msg.Channel);
        }
        
        
        // -resume
        [Command("resume")]
        public async Task Resume(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            
            if (connection.CurrentState.CurrentTrack == null)
            {
                await Embed.NoTracksPlaying(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.ResumeAsync();
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }
        
        
        // -np
        [Command("np")]
        public async Task NowPlaying(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoTracksPlayingEmbed(msg));
                return;
            }

            await msg.Channel.SendMessageAsync(Embed.NowPlayingEmbed(connection.CurrentState.CurrentTrack,
                msg.User));
        }
        
        
        // -repeat
        [Command("repeat")]
        public async Task Repeat(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg));
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NotInVoiceChannelEmbed(msg));
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoConnectionEmbed(msg));
                return;
            }
            if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
            {
                await msg.Channel.SendMessageAsync(Embed.NoTracksPlayingEmbed(msg));
                return;
            }
            
            Bot.Queue.Insert(0, connection.CurrentState.CurrentTrack);
            await msg.Channel.SendMessageAsync(Embed.TrackRepeatEmbed(msg.User));
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
                await msg.Channel.SendMessageAsync(Embed.ClearQueueEmbed(msg.User));
                return;
            }
            
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            await connection.StopAsync();
            await msg.Channel.SendMessageAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }
        
        [Command("skip")]
        public async Task Skip(CommandContext msg, params string[] text)
        {
            await Embed.IncorrectCommand(msg, "-skip").SendAsync(msg.Channel);
        }
        
        
        // -queue
        [Command("queue")]
        public async Task Queue(CommandContext msg)
        {
            await msg.Channel.SendMessageAsync(Embed.Queue(msg.User));
        }

        [Command("clear-queue")]
        public async Task ClearQueue(CommandContext msg)
        {
            Bot.Queue.Clear();
            await msg.Channel.SendMessageAsync(Embed.ClearQueueEmbed(msg.User));
        }
        
        
        // -stop
        [Command("stop"), Aliases("s")]
        public async Task Stop(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Channel.Guild);

            if (connection == null)
            {
                await Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.DisconnectAsync();
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }

        [Command("stop")]
        public async Task Stop(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            await Embed.IncorrectCommand(msg, "-stop").SendAsync(msg.Channel);
        }


        [Command("phonk"), Aliases("ph")]
        public async Task Phonk(CommandContext msg)
        {
            await Play(msg, "https://www.youtube.com/watch?v=3lwdObInlqU&ab_channel=Memphis66.6");
        }
    }
}