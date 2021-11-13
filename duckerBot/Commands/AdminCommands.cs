using System;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Net.Models;
using DSharpPlus.SlashCommands.Attributes;

namespace duckerBot
{
    public partial class Commands : BaseCommandModule
    {
        private DiscordColor mainEmbedColor = DiscordColor.Aquamarine;
        private DiscordColor incorrectEmbedColor = DiscordColor.Red;
        private DiscordColor warningColor = DiscordColor.Orange;
        
        
        // -userinfo
        [Command("userinfo"), 
         RequirePermissions(Permissions.Administrator), 
         Description("bot will send you information about tagged user, or you")]
        public async Task UserInfo(CommandContext msg, [Description("optional user, whose information will send bot")] DiscordMember user = null)
        {
            string userCreatedDate = "";
            if (user == null)
            {
                for (int i = 0; i < msg.User.CreationTimestamp.ToString().Length - 7; i++)
                {
                    userCreatedDate = userCreatedDate + msg.User.CreationTimestamp.ToString()[i];
                }
                
                var userInfoEmbed = new DiscordEmbedBuilder
                {
                    Title = $"{msg.User.Username}'s information",
                    Description = $"User ID: {msg.User.Id}\nDate account created: {userCreatedDate}\nUser's avatar:",
                    ImageUrl = msg.User.AvatarUrl,
                    Color = mainEmbedColor
                };
                userInfoEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(userInfoEmbed);
            }
            else
            {
                for (int i = 0; i < user.CreationTimestamp.ToString().Length - 7; i++)
                {
                    userCreatedDate = userCreatedDate + user.CreationTimestamp.ToString()[i];
                }
                
                var userInfoEmbed = new DiscordEmbedBuilder
                {
                    Title = $"{user.Username}'s information",
                    Description = $"User ID: {user.Id}\nDate account created: {userCreatedDate}\nUser's avatar:",
                    ImageUrl = user.AvatarUrl,
                    Color = mainEmbedColor
                };
                userInfoEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(userInfoEmbed);
            }
        }

        [Command("userinfo"), 
         Description("Bot will send you information about tagged user, or you"),
         RequirePermissions(Permissions.Administrator)]
        public async Task UserInfo(CommandContext msg, params string[] text)
        {
            var incorrectUserInfoCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-userinfo <user>(optional)`",
                Color = incorrectEmbedColor
            };
            incorrectUserInfoCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(incorrectUserInfoCommandEmbed);
        }
        
        
        // -ban
        [Command("ban"),
         RequirePermissions(Permissions.Administrator),
         Description("ban mentioned user")]
        public async Task Ban(CommandContext msg, DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectBanCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-ban <member>`",
                    Color = incorrectEmbedColor
                };
                incorrectBanCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
            }
            else
            {
                var banCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = "User banned",
                    Description = $":)",
                    ImageUrl = "https://static.wikia.nocookie.net/angrybirds-fiction/images/b/b7/%D0%91%D0%B0%D0%BD%D1%85%D0%B0%D0%BC%D0%BC%D0%B5%D1%80.png/revision/latest?cb=20190731080031&path-prefix=ru",
                    Color = mainEmbedColor
                };
                banCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await user.Guild.BanMemberAsync(user);
                DiscordMessage message = msg.Channel.SendMessageAsync(banCommandEmbed).Result;
                Thread.Sleep(3000);
                await msg.Channel.DeleteMessageAsync(message);
            }
        }

        [Command("ban"),
         RequirePermissions(Permissions.Administrator),
         Description("ban mentioned user")]
        public async Task Ban(CommandContext msg, params string[] text)
        {
            var incorrectBanCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-ban <member>`",
                Color = incorrectEmbedColor
            };
            incorrectBanCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
        }


        // -clear
        [Command("clear"), 
         Description("delete messages"), 
         RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg, int amount)
        {
            if (amount > 100)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-clear <amount> (amount must be less than 100)`",
                    Color = incorrectEmbedColor
                };
                incorrectCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
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
                    Color = mainEmbedColor
                };
                deletedMessagesReport.WithFooter(msg.User.Username, msg.User.AvatarUrl);

                DiscordMessage message = msg.Channel.SendMessageAsync(deletedMessagesReport).Result;
                Thread.Sleep(3000);
                await msg.Channel.DeleteMessageAsync(message);
            }
        }
        
        [Command("clear"), 
         Description("delete messages"), 
         RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-clear <amount>`",
                Color = incorrectEmbedColor
            };
            incorrectCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        [Command("clear"), 
         Description("delete messages"), 
         RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg,  string text = null)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-clear <amount>`",
                Color = incorrectEmbedColor
            };
            incorrectCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        // -embed 
        [Command("embed"),
         Description("send embed to current discord channel with your title, description & photos (may be)"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Embed(CommandContext msg, params string[] embedConfig)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-embed <embed config>`\n\n`config template: -t <title> -d <description> -image <URL> \n-titlelink <URL> -del`",
                Color = incorrectEmbedColor
            };
            incorrectCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            
            try
            {
                Console.WriteLine(embedConfig[0]); // catch exeption by appeal to some array element
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
                        switch (embedConfig[i + 1])
                        {
                            case "red":
                                mainEmbedColor = DiscordColor.Red;
                                break;
                            case "green":
                                mainEmbedColor = DiscordColor.Green;
                                break;
                            case "blue":
                                mainEmbedColor = DiscordColor.Azure;
                                break;
                            case "black":
                                mainEmbedColor = DiscordColor.Black;
                                break;
                            case "white":
                                mainEmbedColor = DiscordColor.White;
                                break;
                            default:
                                var incorrectColorFlag = new DiscordEmbedBuilder
                                {
                                    Title = "Missing argument",
                                    Description = "Incorrect -color flag usage\n**Usage:** `-color <color>`\nPossible colors now: `red, green, blue, white, black`",
                                    Color = incorrectEmbedColor
                                };
                                incorrectColorFlag.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                                msg.Channel.SendMessageAsync(incorrectColorFlag);
                                return;
                        }
                    }
                    catch (Exception e)
                    {
                        var incorrectColorFlag = new DiscordEmbedBuilder
                        {
                            Title = $"Missing argument",
                            Description = $"Incorrect `-color` flag\n" +
                                          $"**Usage:** `-color <color>`",
                            Color = incorrectEmbedColor
                        };
                        incorrectColorFlag.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                        msg.Channel.SendMessageAsync(incorrectColorFlag);
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
                Color = mainEmbedColor
            };
            userCreatedEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(userCreatedEmbed);
        }

        [Command("t")]
        public async Task T(CommandContext msg)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "title",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl, 
                    Text = msg.User.Username + "#" + msg.User.Discriminator
                }
            };
            msg.Channel.SendMessageAsync(embed);
        } 
    }
}