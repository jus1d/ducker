﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using DSharpPlus.Lavalink;
using DSharpPlus.Net.Models;
using DSharpPlus.SlashCommands;
using MySqlConnector;
using SpotifyAPI.Web;

namespace ducker
{
    public class AdminCommands : BaseCommandModule
    {
        // -clear
        [Command("clear"),
         Description("Clear `amount` messages from current channel"),
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
        public async Task Clear(CommandContext msg, params string[] txt)
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


        [Command("add-role"), 
         Description("Add a role to mentioned user"),
         RequirePermissions(Permissions.ManageRoles)]
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
        
        [Command("add-role"), RequirePermissions(Permissions.ManageRoles)]
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
        
        [Command("add-role"), RequirePermissions(Permissions.ManageRoles)]
        public async Task AddRoleCommand(CommandContext msg, params string[] txt)
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


        [Command("remove-role"), 
         Description("Remove role from mentioned user"),
         RequirePermissions(Permissions.ManageRoles)]
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
        
        [Command("remove-role"), RequirePermissions(Permissions.ManageRoles)]
        public async Task RemoveRole(CommandContext msg, DiscordRole role, DiscordMember member)
        {
            await msg.Message.DeleteAsync();
            if (!member.Roles.ToArray().Contains(role))
            {
                var memberHasntRoleEmbed = new DiscordEmbedBuilder
                {
                    Description = $"This member doesn't have this role",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(memberHasntRoleEmbed);
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
        
        [Command("remove-role"), RequirePermissions(Permissions.ManageRoles)]
        public async Task RemoveRoleCommand(CommandContext msg, params string[] txt)
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
        [Command("mute"), 
         Description("Mute mentioned member"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Mute(CommandContext msg, DiscordMember member)
        {
            ulong muteRoleId = Database.GetMuteRoleId(msg.Guild.Id);
            if (muteRoleId == 0)
            {
                DiscordRole muteRole = await msg.Guild.CreateRoleAsync("Muted", Permissions.None, DiscordColor.DarkGray, false, false);

                var channels = await msg.Guild.GetChannelsAsync();

                foreach (var channel in channels)
                {
                    await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);
                }

                muteRoleId = muteRole.Id;

                Database database = new Database();
                DataTable table = new DataTable();
                DataTable findGuildTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand findGuildCommand = new MySqlCommand($"SELECT * FROM `ducker` WHERE `guildId` = '{msg.Guild.Id}'", 
                    database.GetConnection());
                adapter.SelectCommand = findGuildCommand;
                adapter.Fill(findGuildTable);
                
                MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `muteRoleId` = {muteRoleId} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                    database.GetConnection());
            
                adapter.SelectCommand = command;
                adapter.Fill(table);
                await member.GrantRoleAsync(msg.Guild.GetRole(Database.GetMuteRoleId(msg.Guild.Id)));
            }
            else
            {
                await member.GrantRoleAsync(msg.Guild.GetRole(Database.GetMuteRoleId(msg.Guild.Id)));
            }
        }
        
        // -mute
        [Command("mute"), RequirePermissions(Permissions.Administrator)]
        public async Task Mute(CommandContext msg, [RemainingText] string txt)
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
        [Command("unmute"), 
         Description("Unmute mentioned member"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Unmute(CommandContext msg, DiscordMember member)
        {
            await member.RevokeRoleAsync(msg.Guild.GetRole(Role.MutedRoleId));
        }
        
        // -unmute
        [Command("unmute"), RequirePermissions(Permissions.Administrator)]
        public async Task Unmute(CommandContext msg, params string[] txt)
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
         Description("Create, and sends an embed with your title, description, title URL, image (All optional, but title or description must be. If you use -del flag, message with config will be deleted)"),
         Aliases("e"),
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


        [Command("activity"),
         Description("Change bot activity"),
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
        public async Task ActivityChanger(CommandContext msg, params string[] txt)
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

        [Command("reaction-role-embed"), 
         Description("Send an embed with buttons, by press there you will granted a role"),
         RequirePermissions(Permissions.Administrator)]
        public async Task ReactionRolesEmbed(CommandContext msg)
        {
            await msg.Message.DeleteAsync();

            DiscordEmoji twitchRgbEmoji = DiscordEmoji.FromName(msg.Client, ":twitchrgb:");
            DiscordEmoji chelEmoji = DiscordEmoji.FromName(msg.Client, ":chel:");
            var followButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_follow_role", $"", false, new DiscordComponentEmoji(twitchRgbEmoji));
            var chelButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_chel_role", "", false, new DiscordComponentEmoji(chelEmoji));
            var builder = new DiscordMessageBuilder()
                .AddEmbed(ducker.Embed.ReactionRolesEmbed(msg.Client, msg.Guild))
                .AddComponents(followButton, chelButton);
            await msg.Channel.SendMessageAsync(builder);
        }

        [Command("stream"), 
         Description("Send stream announcement"),
         RequirePermissions(Permissions.Administrator)]
        public async Task StreamAnnouncement(CommandContext msg, params string[] text)
        {
            await msg.Message.DeleteAsync();
            
            string description = "";
            for (int i = 0; i < text.Length; i++)
            {
                description += text[i] + " ";
            }
            
            await (await msg.Channel.SendMessageAsync(msg.Guild.GetRole(Role.TwitchFollowerRoleId).Mention)).DeleteAsync();
            await (await msg.Channel.SendMessageAsync(ducker.Embed.StreamAnnouncementEmbed(msg, description))).CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":twitch:"));
        }
        
        [Command("set-channel"), 
         Description("Set music channel for this guild"),
         Aliases("sc"), 
         RequirePermissions(Permissions.Administrator)]
        public async Task SetMusicChannel(CommandContext msg, string channelType, DiscordChannel channel)
        {
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            Database database = new Database();
            DataTable table = new DataTable();
            DataTable findGuildTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand findGuildCommand = new MySqlCommand($"SELECT * FROM `ducker` WHERE `guildId` = '{msg.Guild.Id}'", 
                database.GetConnection());
            adapter.SelectCommand = findGuildCommand;
            adapter.Fill(findGuildTable);

            switch (channelType)
            {
                case "music":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `musicChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            database.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, {channel.Id}, NULL, NULL)", 
                            database.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
                case "cmd":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `cmdChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            database.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, NULL, {channel.Id}, NULL)", 
                            database.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
                case "logs":
                    if (findGuildTable.Rows.Count > 0)
                    {
                        MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `logsChannelId` = {channel.Id} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                            database.GetConnection());
            
                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    else
                    {
                        MySqlCommand command = new MySqlCommand($"INSERT INTO `ducker` (`guildId`, `musicChannelId`, `cmdChannelId`, `logsChannelId`) VALUES ({msg.Guild.Id}, NULL, NULL, {channel.Id})", 
                            database.GetConnection());

                        adapter.SelectCommand = command;
                        adapter.Fill(table);
                    }
                    break;
            }
            await msg.Channel.SendMessageAsync(ducker.Embed.ChannelConfiguredEmbed(msg.User, channelType, channel));
        }
    }
}