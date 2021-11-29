using System;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DiscordColour = DSharpPlus.Entities.DiscordColor;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Net.Models;
using DSharpPlus.SlashCommands;
using SpotifyAPI.Web;

namespace duckerBot
{
    public partial class Commands : BaseCommandModule
    {
        // -ban
        [Command("ban"),
         RequirePermissions(Permissions.BanMembers)]
        public async Task Ban(CommandContext msg, DiscordMember user, params string[] reasonInput)
        {
            string reason = "";
            for (int i = 0; i < reasonInput.Length; i++)
            {
                reason += reasonInput[i] + " ";
            }
            
            var banCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "User banned",
                Description = $":)",
                ImageUrl = "https://static.wikia.nocookie.net/angrybirds-fiction/images/b/b7/%D0%91%D0%B0%D0%BD%D1%85%D0%B0%D0%BC%D0%BC%D0%B5%D1%80.png/revision/latest?cb=20190731080031&path-prefix=ru",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            try
            {
                await user.Guild.BanMemberAsync(user, 0, reason);
            }
            catch (Exception e)
            {
                var incorrectBanCommandEmbed = new DiscordEmbedBuilder
                {
                    Description = $":x: You can't ban this user",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
                throw;
            }
            DiscordMessage message = msg.Channel.SendMessageAsync(banCommandEmbed).Result;
            Thread.Sleep(3000);
            await msg.Channel.DeleteMessageAsync(message);
        }
        

        [Command("ban"),
         RequirePermissions(Permissions.BanMembers)]
        public async Task Ban(CommandContext msg, params string[] text)
        {
            var incorrectBanCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-ban <member>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
        }
        
        
        // -kick 
        [Command("kick"), 
         RequirePermissions(Permissions.KickMembers)]
        public async Task Kick(CommandContext msg, DiscordMember user, params string[] reasonInput)
        {
            string reason = "";
            for (int i = 0; i < reasonInput.Length; i++)
            {
                reason += reasonInput[i] + " ";
            }
            
            try
            {
                await user.RemoveAsync(reason);
            }
            catch (Exception e)
            {
                var incorrectKickEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: You can't kick this member",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectKickEmbed);
                throw;
            }
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":white_check_mark:"));
        }

        [Command("kick"),  
         RequirePermissions(Permissions.KickMembers)]
        public async Task Kick(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -kick <member>",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }


        // -clear
        [Command("clear"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg, int amount)
        {
            if (amount > 100 || amount < 0)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-clear <amount> (amount must be less than 100 and bigger than 0)`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
            }
            else
            {
                await msg.Channel.DeleteMessagesAsync(await msg.Channel.GetMessagesAsync(amount + 1));

                string messageOrMessages;
                if (amount.ToString()[amount.ToString().Length - 1] == '1' && amount != 11)
                {
                    messageOrMessages = "message";
                }
                else
                {
                    messageOrMessages = "messages";
                }
            
                var deletedMessagesReport = new DiscordEmbedBuilder
                {
                    Title = $"Deleted messages report", 
                    Description = $"I have deleted {amount} {messageOrMessages}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                DiscordMessage message = msg.Channel.SendMessageAsync(deletedMessagesReport).Result;
                Thread.Sleep(3000);
                await msg.Channel.DeleteMessageAsync(message);
            }
        }
        
        [Command("clear"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-clear <amount>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        [Command("clear"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg,  string text = null)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-clear <amount>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }


        [Command("addrole"), RequirePermissions(Permissions.ManageRoles)]
        public async Task AddRoleCommand(CommandContext msg, DiscordMember member, DiscordRole role)
        {
            await msg.Message.DeleteAsync();
            if (member.Roles.ToArray().Contains(role))
            {
                var memberHasRoleEmbed = new DiscordEmbedBuilder
                {
                    Description = $"This member currently has this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(memberHasRoleEmbed);
                return;
            }
            
            try
            {
                await member.GrantRoleAsync(role);
                var completeEmbed = new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} added to {member.Mention}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Channel.SendMessageAsync(completeEmbed);
            }
            catch (Exception e)
            {
                var incorrectEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: You can't add this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectEmbed);
            }
        }
        
        [Command("addrole"), RequirePermissions(Permissions.ManageRoles)]
        public async Task AddRoleCommand(CommandContext msg, DiscordRole role, DiscordMember member)
        {
            await msg.Message.DeleteAsync();
            if (member.Roles.ToArray().Contains(role))
            {
                var memberHasRoleEmbed = new DiscordEmbedBuilder
                {
                    Description = $"This member currently has this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(memberHasRoleEmbed);
                return;
            }
            
            try
            {
                await member.GrantRoleAsync(role);
                var completeEmbed = new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} added to {member.Mention}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Channel.SendMessageAsync(completeEmbed);
            }
            catch (Exception e)
            {
                var incorrectEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: You can't add this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectEmbed);
            }
        }
        
        [Command("addrole"), RequirePermissions(Permissions.ManageRoles)]
        public async Task AddRoleCommand(CommandContext msg, params string[] text)
        {
            var incorrectAddRoleCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-addrole <member> <role>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectAddRoleCommandEmbed);
        }


        [Command("removerole"), RequirePermissions(Permissions.ManageRoles)]
        public async Task RemoveRole(CommandContext msg, DiscordMember member, DiscordRole role)
        {
            await msg.Message.DeleteAsync();
            if (!member.Roles.ToArray().Contains(role))
            {
                var memberHasRoleEmbed = new DiscordEmbedBuilder
                {
                    Description = $"This member doesn't have this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(memberHasRoleEmbed);
                return;
            }

            try
            {
                await member.RevokeRoleAsync(role);
                var completeEmbed = new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} removed from {member.Mention}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Channel.SendMessageAsync(completeEmbed);
            }
            catch (Exception e)
            {
                var incorrectEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: You can't remove this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectEmbed);
            }
        }
        
        [Command("removerole"), RequirePermissions(Permissions.ManageRoles)]
        public async Task RemoveRole(CommandContext msg, DiscordRole role, DiscordMember member)
        {
            await msg.Message.DeleteAsync();
            if (!member.Roles.ToArray().Contains(role))
            {
                var memberHasRoleEmbed = new DiscordEmbedBuilder
                {
                    Description = $"This member doesn't have this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(memberHasRoleEmbed);
                return;
            }

            try
            {
                await member.RevokeRoleAsync(role);
                var completeEmbed = new DiscordEmbedBuilder
                {
                    Description = $"Complete, {role.Name} removed from {member.Mention}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Channel.SendMessageAsync(completeEmbed);
            }
            catch (Exception e)
            {
                var incorrectEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: You can't remove this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectEmbed);
            }
        }
        
        [Command("removerole"), RequirePermissions(Permissions.ManageRoles)]
        public async Task RemoveRoleCommand(CommandContext msg, params string[] text)
        {
            var incorrectRemoveRoleCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-remove <member> <role>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectRemoveRoleCommandEmbed);
        }
        
        
        // -mute
        [Command("mute"), RequirePermissions(Permissions.Administrator)]
        public async Task Mute(CommandContext msg, DiscordMember member)
        {
            DiscordRole muteRole = msg.Guild.GetRole(911479294209970187);
            await member.GrantRoleAsync(muteRole);
        }
        
        // -mute
        [Command("mute"), RequirePermissions(Permissions.Administrator)]
        public async Task Mute(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-mute <member>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        // -unmute
        [Command("unmute"), RequirePermissions(Permissions.Administrator)]
        public async Task Unmute(CommandContext msg, DiscordMember member)
        {
            DiscordRole muteRole = msg.Guild.GetRole(911479294209970187);
            await member.RevokeRoleAsync(muteRole);
        }
        
        // -unmute
        [Command("unmute"), RequirePermissions(Permissions.Administrator)]
        public async Task Unmute(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-unmute <member>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }



        // -embed 
        [Command("embed"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Embed(CommandContext msg, params string[] embedConfig)
        {
            var color = Bot.MainEmbedColor;
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-embed <embed config>`\n\n`config template: -t <title> -d <description> -image <URL> \n-titlelink <URL> -color <#color> -del`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            
            try
            {
                Console.WriteLine(embedConfig[0]); // catch exception by appeal to some array element
            }
            catch (Exception e)
            {
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
            }

            if (embedConfig[0] == "-titlelink" && embedConfig.Length == 2)
            {
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
            }

            string embedTitle = "";
            string embedDescription = "";
            string embedTitleLink = "";
            string embedImageLink = "";

            for (int i = 0; i < embedConfig.Length; i++)
            {
                if (embedConfig[i] == "-t")
                {
                    for (int j = i + 1; j < embedConfig.Length && embedConfig[j] != "-d" && embedConfig[j] != "-image" && embedConfig[j] != "-titlelink" && embedConfig[j] != "-del" && embedConfig[j] != "-color"; j++)
                    {
                        embedTitle += embedConfig[j] + " ";
                    }
                }
                else if (embedConfig[i] == "-d")
                {
                    for (int j = i + 1; j < embedConfig.Length && embedConfig[j] != "-t" && embedConfig[j] != "-image" && embedConfig[j] != "-titlelink" && embedConfig[j] != "-del" && embedConfig[j] != "-color"; j++)
                    {
                        embedDescription += embedConfig[j] + " ";
                    }
                }
                else if (embedConfig[i] == "-image")
                {
                    embedImageLink = embedConfig[i + 1];
                }
                else if (embedConfig[i] == "-titlelink")
                {
                    embedTitleLink = embedConfig[i + 1];
                }
                else if (embedConfig[i] == "-del")
                {
                    await msg.Message.DeleteAsync();
                }
                else if (embedConfig[i] == "-color")
                {
                    try
                    {
                        color = new DiscordColor(embedConfig[i + 1]);
                    }
                    catch (Exception e)
                    {
                        var incorrectColorFlag = new DiscordEmbedBuilder
                        {
                            Title = $"Missing argument",
                            Description = $"Incorrect `-color` flag\n" +
                                          $"**Usage:** `-color <#color> (HEX)`",
                            Footer = new DiscordEmbedBuilder.EmbedFooter
                            {
                                IconUrl = msg.User.AvatarUrl,
                                Text = msg.User.Username
                            },
                            Color = Bot.IncorrectEmbedColor
                        };
                        await msg.Channel.SendMessageAsync(incorrectColorFlag);
                        throw;
                    }
                }
            }
            var userCreatedEmbed = new DiscordEmbedBuilder
            {
                Title = embedTitle,
                Description = embedDescription,
                ImageUrl = embedImageLink,
                Url = embedTitleLink,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = color
            };
            await msg.Channel.SendMessageAsync(userCreatedEmbed);
        }
        
        
        // -poll
        [Command("poll")]
        public async Task Poll(CommandContext msg, params string[] pollConfig)
        {
            try
            {
                bool b = pollConfig[0] == "s";
            }
            catch (Exception e)
            {
                var incorrectPollCommand = new DiscordEmbedBuilder
                {
                    Title = "Missing argument",
                    Description = "**Usage:** `-poll <poll description>`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectPollCommand);
                throw;
            }
            
            string pollDescription = "";
            for (int i = 0; i < pollConfig.Length; i++)
            {
                Console.WriteLine(pollConfig[0]);
            }
            var pollEmbed = new DiscordEmbedBuilder
            {
                Title = "Poll",
                Description = pollDescription,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            var pollMessage = msg.Channel.SendMessageAsync(pollEmbed);
            await pollMessage.Result.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":white_check_mark:"));
            await pollMessage.Result.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":x:"));
        }
        
        
        [Command("reaction")]
        public async Task Reaction(CommandContext msg, ulong messageId, DiscordEmoji emoji)
        {
            var message = msg.Channel.GetMessageAsync(messageId);
            await message.Result.CreateReactionAsync(emoji);
            Thread.Sleep(5000);
            await msg.Message.DeleteAsync();
        }

        [Command("reaction")]
        public async Task Reaction(CommandContext msg, params string[] text)
        {
            try
            {
                string s = text[0];
            }
            catch (Exception e)
            {
                var incorrectReactionCommand = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-reaction <message id> <emoji>`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectReactionCommand);
            }
        }

        [Command("t")]
        public async Task TestCommand(CommandContext msg, Uri url)
        {
            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest("2e0c2440f50140cf9f2254ed41bb1f37", "0f48eeafd9584ff08d42cce68c49782c");
            var response = await new OAuthClient(config).RequestToken(request);
            var spotify = new SpotifyClient(config.WithToken(response.AccessToken));
            
            if (url.LocalPath[Range.EndAt(7)] == "/track/")
            {
                string trackId = url.ToString()[Range.StartAt(31)][Range.EndAt(22)];
                var track = spotify.Tracks.Get(trackId).Result;
                await msg.Channel.SendMessageAsync(track.Name);
            }
            else if (url.LocalPath[Range.EndAt(10)] == "/playlist/")
            {
                string playlistId = url.ToString()[Range.StartAt(34)][Range.EndAt(22)];
                var playlist = spotify.Playlists.Get(playlistId).Result;
                await msg.Channel.SendMessageAsync(playlist.Name);
            }
            else if (url.LocalPath[Range.EndAt(9)] == "/episode/")
            {
                await msg.Channel.SendMessageAsync("1");
                string episodeId = url.ToString()[Range.StartAt(33)][Range.EndAt(22)];
                await msg.Channel.SendMessageAsync(episodeId);
                var episode = spotify.Episodes.Get(episodeId);
                await msg.Channel.SendMessageAsync("1");
                msg.Channel.SendMessageAsync(episode.Result.Name);
            }
        }

        [Command("activity"),
         RequirePermissions(Permissions.Administrator)]
        public async Task ActivityChanger(CommandContext msg, string activityType)
        {
            if (activityType == "streaming")
            {
                var activity = new DiscordActivity
                {
                    ActivityType = ActivityType.Streaming,
                    Name = "with ducks |  -help",
                    StreamUrl = "https://www.twitch.tv/itakash1"
                };
                var activityChanedEmbed = new DiscordEmbedBuilder
                {
                    Description = "Activity changed to streaming type",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Client.UpdateStatusAsync(activity);
                await msg.Channel.SendMessageAsync(activityChanedEmbed);
            }
            else if (activityType == "playing")
            {
                var activity = new DiscordActivity
                {
                    ActivityType = ActivityType.Playing,
                    Name = "with ducks | -help"
                };
                var activityChanedEmbed = new DiscordEmbedBuilder
                {
                    Description = "Activity changed to playing type",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                };
                await msg.Client.UpdateStatusAsync(activity);
                await msg.Channel.SendMessageAsync(activityChanedEmbed);
            }
            else
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-activity <type>`\nPossible types: `stream`, `def`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
            }
        }

        [Command("activity"),
         RequirePermissions(Permissions.Administrator)]
        public async Task ActivityChanger(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-activity <type>`\nPossible types: `stream`, `def`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }

        [Command("rrembed"), RequirePermissions(Permissions.Administrator)]
        public async Task ServerNewsRole(CommandContext msg)
        {
            await msg.Message.DeleteAsync();

            DiscordEmoji vibeEmoji = DiscordEmoji.FromName(msg.Client, ":vibe:");
            DiscordEmoji twitchRgbEmoji = DiscordEmoji.FromName(msg.Client, ":twitchrgb:");
            DiscordEmoji chelEmoji = DiscordEmoji.FromName(msg.Client, ":chel:");
            var rolesEmbed = new DiscordEmbedBuilder
            {
                Title = $"{vibeEmoji} Welcome, tap reactions to get roles {vibeEmoji}",
                Description = $"Reaction roles list:\n{twitchRgbEmoji} - twitch follower(by having this reaction, u will get stream notifications)\n{chelEmoji} - default role for this server\n\nGL",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = msg.Guild.GetMemberAsync(906179696516026419).Result.DisplayName,
                    IconUrl = msg.Guild.GetMemberAsync(906179696516026419).Result.AvatarUrl
                },
                Color = Bot.MainEmbedColor
            };
            var message = msg.Channel.SendMessageAsync(rolesEmbed);
            await message.Result.CreateReactionAsync(twitchRgbEmoji);
            await message.Result.CreateReactionAsync(chelEmoji);
        }

        [Command("stream"), RequirePermissions(Permissions.Administrator)]
        public async Task StreamAnnouncement(CommandContext msg, params string[] text)
        {
            string description = "";
            for (int i = 0; i < text.Length; i++)
            {
                description += text[i] + " ";
            }
            
            await msg.Message.DeleteAsync();
            var streamAnnouncementembed = new DiscordEmbedBuilder
            {
                Title = "Stream online!",
                Description = $"{description} https://www.twitch.tv/itakash1",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = msg.Guild.GetMemberAsync(Bot.Id).Result.DisplayName,
                    IconUrl = msg.Guild.GetMemberAsync(Bot.Id).Result.AvatarUrl
                },
                ImageUrl = "https://psv4.userapi.com/c537232/u171567304/docs/d41/9e121b6e7f1c/main.gif?extra=SfCKm9eq4uM0yCcBmuf5rJjueEM6DOAANvGaAiRiW6zkUIXXYCvEsqOkK-qcl29K4iurvQTK8I4THZUYDTUxAfIs7FTi1lWlVmzgrO14Ou7ETGyGA66hgs6FU4TXqj9HZtWrmxu_vzPqO2TmKJ1i0sOlog",
                Color = Bot.MainEmbedColor
            };
            var followerTag = msg.Channel.SendMessageAsync(msg.Guild.GetRole(914921577634754600).Mention).Result.DeleteAsync();
            await msg.Channel.SendMessageAsync(streamAnnouncementembed).Result.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":twitch:"));
        } 
    }
}