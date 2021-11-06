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

namespace duckerBot
{
    public partial class Commands : BaseCommandModule
    {
        private DiscordColor mainEmbedColor = DiscordColor.Azure;
        private DiscordColor incorrectEmbedColor = DiscordColor.Red;
        
        
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
                    Title = $"{msg.User.Username}'s information",
                    Description = $"User ID: {msg.User.Id}\nDate account created: {userCreatedDate}\nUser's avatar:",
                    ImageUrl = msg.User.AvatarUrl,
                    Color = mainEmbedColor
                };
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
                Description = $"**Usage:** ```-userinfo <user>(optional)```\n [for {msg.Member.Mention}]",
                Color = incorrectEmbedColor
            };
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
                    Description = $"**Usage:** -ban <member>\n [for {msg.Member.Mention}]",
                    Color = incorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
            }
            else
            {
                var banCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = "User banned",
                    Description = $":)\n[for {msg.Member.Mention}]",
                    ImageUrl = "https://static.wikia.nocookie.net/angrybirds-fiction/images/b/b7/%D0%91%D0%B0%D0%BD%D1%85%D0%B0%D0%BC%D0%BC%D0%B5%D1%80.png/revision/latest?cb=20190731080031&path-prefix=ru",
                    Color = mainEmbedColor
                };
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
                Description = $"**Usage:** -ban <member>\n [for {msg.Member.Mention}]",
                Color = incorrectEmbedColor
            };
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
                    Description = $"**Usage:** -clear <amount> (amount must be less than 100)\n [for {msg.Member.Mention}]",
                    Color = incorrectEmbedColor
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
                    Color = mainEmbedColor
                };

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
                Description = $"**Usage:** -clear <amount>\n [for {msg.Member.Mention}]",
                Color = incorrectEmbedColor
            };
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
                Description = $"**Usage:** -clear <amount>\n [for {msg.Member.Mention}]",
                Color = incorrectEmbedColor
            };
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
                Description = $"**Usage:** ```-embed -t <embed's title> -d <embed's description> \n-image <embed's image> -titlelink <link for embed's title>```\n [for {msg.Member.Mention}]",
                Color = incorrectEmbedColor
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
                    for (int j = i + 1; j < embedConfig.Length && embedConfig[j] != "-d" && embedConfig[j] != "-image" && embedConfig[j] != "-titlelink"; j++)
                    {
                        embedTitle += embedConfig[j] + " ";
                    }
                }
                else if (embedConfig[i] == "-d")
                {
                    for (int j = i + 1; j < embedConfig.Length && embedConfig[j] != "-t" && embedConfig[j] != "-image" && embedConfig[j] != "-titlelink"; j++)
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
            }
            var userCreatedEmbed = new DiscordEmbedBuilder
            {
                Title = embedTitle,
                Description = embedDescription,
                ImageUrl = embedImageLink,
                Url = embedTitleLink,
                Color = mainEmbedColor
            };
            await msg.Channel.SendMessageAsync(userCreatedEmbed);
        }
        
        
        // -t
        [Command("t"), RequirePermissions(Permissions.Administrator)]
        public async Task Test(CommandContext msg)
        {
            var tEmbed = new DiscordEmbedBuilder
            {
                Title = "title",
                Description = "description",
                Color = mainEmbedColor
            };
            await msg.Channel.SendMessageAsync(tEmbed);
        }
    }
}