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
using DiscordColour = DSharpPlus.Entities.DiscordColor;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Net.Models;
using DSharpPlus.SlashCommands;

namespace duckerBot
{
    public partial class Commands : BaseCommandModule
    {
        // -ban
        [Command("ban"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Ban(CommandContext msg, DiscordMember user = null)
        {
            if (user == null)
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
            else
            {
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
                await user.Guild.BanMemberAsync(user);
                DiscordMessage message = msg.Channel.SendMessageAsync(banCommandEmbed).Result;
                Thread.Sleep(3000);
                await msg.Channel.DeleteMessageAsync(message);
            }
        }

        [Command("ban"),
         RequirePermissions(Permissions.Administrator)]
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
         RequirePermissions(Permissions.Administrator)]
        public async Task Kick(CommandContext msg, DiscordMember user = null)
        {
            if (user == null)
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
                return;
            }

            try
            {
                await user.RemoveAsync();
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
         RequirePermissions(Permissions.Administrator)]
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
        

        [Command("addreactions")]
        public async Task AddReaction(CommandContext msg, ulong msgId, params string[] emoji)
        {
            DiscordMessage message = msg.Channel.GetMessageAsync(msgId).Result;
            try
            {
                string s = emoji[0];
            }
            catch (Exception e)
            {
                var incorrectEmojisEmbed = new DiscordEmbedBuilder
                {
                    Description = "Enter emojis for set reactions",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectEmojisEmbed);
                throw;
            }

            for (int i = 0; i < emoji.Length; i++)
            {
                await message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, emoji[i]));
            }
        }
        
        [Command("reaction")]
        public async Task Reaction(CommandContext msg, ulong messageId, DiscordEmoji emoji)
        {
            var message = msg.Channel.GetMessageAsync(messageId);
            await message.Result.CreateReactionAsync(emoji);
            Thread.Sleep(5000);
            await msg.Message.DeleteAsync();
        }
    }
}