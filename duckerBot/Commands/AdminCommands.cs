using System;
using System.Linq;
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
        [Command("ban")]
        [RequirePermissions(Permissions.Administrator)]
        [Description("banned mentioned user")]
        public async Task Ban(CommandContext msg, [Description("user")] DiscordMember user)
        {
            await user.Guild.BanMemberAsync(user);
            
        }
        
        // -channelid
        [Command("channelid")]
        [RequirePermissions(Permissions.Administrator)]
        [Description("Send you tagged (or bot-commands) channel ID")]
        public async Task ChannelID(CommandContext msg, [Description(" optional channel (for voice channels with emoji - use template: **-channelid <#id>**)")] DiscordChannel channel = null)
        {
            if (channel == null)
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Channel ID",
                    Description = $"{msg.Channel.Mention} channel ID: {msg.Channel.Id}",
                    Color = DiscordColor.Azure
                };
                
                await msg.Channel.SendMessageAsync(embed).ConfigureAwait(false);
            }
            else
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Channel ID",
                    Description = $"{channel.Mention} channel ID: {channel.Id}",
                    Color = DiscordColor.Azure
                };
                
                await msg.Channel.SendMessageAsync(embed).ConfigureAwait(false);
            }
        }
        
        // -channelid <text>
        [Command("channelid")]
        [RequirePermissions(Permissions.Administrator)]
        [Description("Send you tagged (or bot-commands) channel ID")]
        
        public async Task ChannelID(CommandContext msg, [Description("if you misuse the command")] params string[] parametres)
        {
            if(msg.Channel.Name != "bot-commands")
                return;

            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = "Template -channelid:",
                Description = "-channelid <channel>\n" +
                              "for voice channels with emoji - use template: **-channelid <#id>**",
                Color = DiscordColor.Azure
                
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        // -clear
        [Command("clear"), Description("delete messages"), RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg, int amount)
        {
            if (amount > 100)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -clear <amount> (amount must be less than 100)\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
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
                    Color = DiscordColor.Azure
                };

                DiscordMessage message = msg.Channel.SendMessageAsync(deletedMessagesReport).Result;
                Thread.Sleep(1500);
                await msg.Channel.DeleteMessageAsync(message);
            }
        }
        
        [Command("clear"), Description("delete messages"), RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -clear <amount>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        [Command("clear"), Description("delete messages"), RequirePermissions(Permissions.Administrator)]
        public async Task Clear(CommandContext msg,  string text = null)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -clear <amount>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
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
                Color = DiscordColor.Red
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
                Color = DiscordColor.Azure
            };
            await msg.Channel.SendMessageAsync(userCreatedEmbed);
        }
        
        
        // -t
        [Command("t"), RequirePermissions(Permissions.Administrator)]
        public async Task Test(CommandContext msg, DiscordMember user)
        {
            
        }
    }
}