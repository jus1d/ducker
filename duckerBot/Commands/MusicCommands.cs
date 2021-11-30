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
using SpotifyAPI.Web;
using Newtonsoft.Json;
using System.IO;
using System.Text;

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
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }
            
            if (channel == null)
                channel = msg.Member.VoiceState.Channel;

            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                var noConnectionEmbed = new DiscordEmbedBuilder
                {
                    Description = "Connection is not established",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
                return;
            }
            
            var node = lava.ConnectedNodes.Values.First();
            
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                var noInVoice = new DiscordEmbedBuilder
                {
                    Description = "You are not in a voice channel",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(noInVoice);
                return;
            }
            
            await node.ConnectAsync(channel);
        }
        

        // -quit
        [Command("quit"), Aliases("leave", "q"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Quit(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }
            
            DiscordChannel channel = msg.Member.VoiceState.Channel;
            
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                var noConnectionEmbed = new DiscordEmbedBuilder
                {
                    Description = "Connection is not established",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
                return;
            }
            var node = lava.ConnectedNodes.Values.First();
            if (channel.Type != ChannelType.Voice)
            {
                var invalidChannel = new DiscordEmbedBuilder
                {
                    Description = "Not a valid voice channel",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(invalidChannel);
                return;
            }
            var connection = node.GetGuildConnection(channel.Guild);
            if (connection == null)
            {
                var noConnectionEmbed = new DiscordEmbedBuilder
                {
                    Description = "I'm is not connected",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
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
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                var incorrectMusicCommand = new DiscordEmbedBuilder
                {
                    Title = "You are not in a voice channel",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                    IconUrl = msg.User.AvatarUrl,
                    Text = "Ordered by " + msg.User.Username
                },
                };
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
                    Title = "There are no tracks loaded",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                };
                await msg.Channel.SendMessageAsync(incorrectMusicCommand);
                return;
            }
            await connection.ResumeAsync();
        }
        
        
        // -play url
        [Command("play"), Aliases("p")]
        public async Task Play(CommandContext msg, Uri url)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
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
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(noConnectionEmbed);
                return;
            }

            if (url.Authority == "open.spotify.com")
            {
                var config = SpotifyClientConfig.CreateDefault();
                var request = new ClientCredentialsRequest(ConfigJson.GetConfigField().SpotifyId, ConfigJson.GetConfigField().SpotifySecret);
                var response = await new OAuthClient(config).RequestToken(request);
                var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
                var trackSpotify = await spotify.Tracks.Get(GetTrackId(url.ToString()));
            
                string authors = "";
                for (int i = 0; i < trackSpotify.Artists.Count; i++)
                {
                    if (trackSpotify.Artists.Count == 1 || (trackSpotify.Artists.Count == i + 1))
                    {
                        authors += trackSpotify.Artists[i].Name;
                    }
                    else
                    {
                        authors += trackSpotify.Artists[i].Name + ", ";
                    }
                }

                string search = trackSpotify.Name + authors;
                
                await Join(msg);
                var loadResult = await node.Rest.GetTracksAsync(search);
                var track = loadResult.Tracks.First();
                await connection.PlayAsync(track);
            
                var playEmbed = new DiscordEmbedBuilder
                {
                    Title = "Now playing",
                    Description = $"[{track.Title} - {authors}]({url})",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = "Ordered by " + msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                DiscordMessage message = msg.Channel.SendMessageAsync(playEmbed).Result;
                await message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":pause_button:"));
                await message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":arrow_forward:"));
            }
            else
            {
                var loadResult = await node.Rest.GetTracksAsync(url);
                var track = loadResult.Tracks.First();
                await connection.PlayAsync(track);
                var playEmbed = new DiscordEmbedBuilder
                {
                    Title = "Now playing",
                    Description = $"[{track.Title}]({url})",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = "Ordered by " + msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                DiscordMessage message = msg.Channel.SendMessageAsync(playEmbed).Result;
                await message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":pause_button:"));
                await message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":arrow_forward:"));
            }
        }
        
        public static string GetTrackId(string trackUrl)
        {
            string trackId = "";

            int i = 31;
            while (trackUrl[i] != '?')
            {
                trackId += trackUrl[i];
                i++;
            }
            return trackId;
        }
        
        // -play search
        [Command("play")]
        public async Task Play(CommandContext msg, params string[] searchInput)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            await Join(msg);

            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                var noVoice = new DiscordEmbedBuilder
                {
                    Description = "You are not in a voice channel",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
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
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
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
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Channel.SendMessageAsync(searchFaled);
                return;
            }

            var track = loadResult.Tracks.First();

            await connection.PlayAsync(track);
            
            var playEmbed = new DiscordEmbedBuilder
            {
                Title = "Now playing",
                Description = $"[{track.Title}]({track.Uri})",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = "Ordered by " + msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            DiscordMessage message = msg.Channel.SendMessageAsync(playEmbed).Result;
            await message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":pause_button:"));
            await message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":arrow_forward:"));
        }
        
        
        // -pause
        [Command("pause")]
        public async Task Pause(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                var noVoice = new DiscordEmbedBuilder
                {
                    Description = "You are not in a voice channel",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
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
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(noVoice);
                return;
            }

            if (connection.CurrentState.CurrentTrack == null)
            {
                var noTracksLoaded = new DiscordEmbedBuilder
                {
                    Description = "There are no tracks loaded",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(noTracksLoaded);
                return;
            }
            await connection.PauseAsync();
        }

        [Command("pause")]
        public async Task Pause(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            var incorrectPauseCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** -pause",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectPauseCommandEmbed);
            var emoji = DiscordEmoji.FromName(msg.Client, ":pause_button:");
            await msg.Message.CreateReactionAsync(emoji);
        }
        
        
        // -stop
        [Command("stop"), Aliases("s")]
        public async Task Stop(CommandContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }

            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                var noTracksLoaded = new DiscordEmbedBuilder
                {
                    Description = "Connection is not established",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
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
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(noTracksLoaded);
                return;
            }
            await connection.DisconnectAsync();
        }

        [Command("stop")]
        public async Task Stop(CommandContext msg, params string[] text)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                var incorrectChannel = new DiscordEmbedBuilder
                {
                    Title = "Incorrect channel for music commands",
                    Description = $"This command can be used only in <#{msg.Guild.GetChannel(Bot.MusicChannelId).Id}>",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.WarningColor
                };
                await msg.Channel.SendMessageAsync(incorrectChannel);
                return;
            }
            await Quit(msg);
        }


        [Command("phonk"), Aliases("ph")]
        public async Task Phonk(CommandContext msg)
        {
            Uri url = new Uri("https://www.youtube.com/watch?v=3lwdObInlqU&ab_channel=Memphis66.6");
            await Play(msg, url);
        }
    }
}