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

namespace jus1dBot
{
    public partial class Commands : BaseCommandModule
    {
        // pinging
        [Command("ping")]
        [Description("returns pong")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Ping(CommandContext msg)
        {
            var pingEmbed = new DiscordEmbedBuilder
            {
                Description = msg.Client.Ping.ToString() + "ms",
                Color = DiscordColor.Azure
            };

            await msg.Channel.SendMessageAsync(pingEmbed);
        }
        
        // -userinfo
        [Command("userinfo")]
        [RequirePermissions(Permissions.Administrator)]
        [Description("Bot will send you information about tagged user, or you")]
        public async Task UserInfo(CommandContext msg, [Description("optional user, whose information will send bot")] DiscordMember user = null)
        {
            if (user == null)
            {
                var userSended = msg.User;
            
                string userCreatedDate = "";
            
                for (int i = 0; i < userSended.CreationTimestamp.ToString().Length - 7; i++)
                {
                    userCreatedDate = userCreatedDate + userSended.CreationTimestamp.ToString()[i];
                }

                await msg.Channel.SendMessageAsync($"{userSended.Mention}'s Info:\n" +
                                                   $"User ID: {userSended.Id}\n" +
                                                   $"Date account created: {userCreatedDate}\n" +
                                                   $"User's avatar URL: {userSended.AvatarUrl}");
            }
            else
            {
                string userCreatedDate = "";
            
                for (int i = 0; i < user.CreationTimestamp.ToString().Length - 7; i++)
                {
                    userCreatedDate = userCreatedDate + user.CreationTimestamp.ToString()[i];
                }
                
                await msg.Channel.SendMessageAsync($"{user.Mention}'s Info:\n" +
                                                   $"User ID: {user.Id}\n" +
                                                   $"Date account created: {userCreatedDate}\n" +
                                                   $"User's avatar URL: {user.AvatarUrl}");
            }
        }

        // -voicemute
        [Command("voicemute")]
        [RequirePermissions(Permissions.Administrator)]
        [Description("Mute(voice) tagged user")]
        public async Task VoiceMute(CommandContext msg, [Description("User, for mute")] DiscordMember user = null)
        {
            if (user == null)
            {
                var templateEmbed = new DiscordEmbedBuilder
                {
                    Title = "Template -voicemute:",
                    Description = "-voicemute <user>\n",
                    Color = DiscordColor.Azure
                
                };

                await msg.Channel.SendMessageAsync(templateEmbed);
                return;
            }
            
            await user.SetMuteAsync(true);
        }
        
        // -voiceunmute
        [Command("voiceunmute")]
        [RequirePermissions(Permissions.Administrator)]
        [Description("Unmute(voice) tagged user")]
        public async Task VoiceUnmute(CommandContext msg, [Description("User, for unmute")] DiscordMember user = null)
        {
            if (user == null)
            {
                var templateEmbed = new DiscordEmbedBuilder
                {
                    Title = "Template -voiceunmute:",
                    Description = "-voiceunmute <user>\n",
                    Color = DiscordColor.Azure
                
                };

                await msg.Channel.SendMessageAsync(templateEmbed);
                return;
            }
            
            await user.SetMuteAsync(false);
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
        
        
        // -rules (command off)
        // [Command("rules"), Description("send rules to channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Rules(CommandContext msg)
        {
            var rulesEmbed = new DiscordEmbedBuilder
            {
                Title = "Server rules",
                Description = "",
                Color = DiscordColor.Azure
            };
            await msg.Channel.SendMessageAsync(rulesEmbed);
            await msg.Channel.DeleteMessageAsync(msg.Message);
        }
        
        
        // -embed 
        [Command("embed"),
         Description("send embed to current discord channel with your title, description & photos (may be)"),
         RequirePermissions(Permissions.Administrator)]
        public async Task Embed(CommandContext msg, params string[] embedConfig)
        {
            try
            {
                Console.WriteLine(embedConfig[3]); // catch exeption by appeal to some array element
            }
            catch (Exception e)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -embed -t <embed's title> -d <embed's description>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                msg.Channel.SendMessageAsync(incorrectCommandEmbed);
            }
            
            string embedTitle = "";
            string embedDescription = "";
            
            for (int i = 0; i < embedConfig.Length; i++)
            {
                if (embedConfig[i] == "-t")
                {
                    for (int j = i + 1;  embedConfig[j] != "-d"; j++)
                    {
                        embedTitle += embedConfig[j] + " ";
                    }
                }
                else if (embedConfig[i] == "-d")
                {
                    for (int j = i + 1; j < embedConfig.Length; j++)
                    {
                        embedDescription += embedConfig[j] + " ";
                    }
                }
            }
            var userCreatedEmbed = new DiscordEmbedBuilder
            {
                Title = embedTitle,
                Description = embedDescription,
                Color = DiscordColor.Azure
            };
            msg.Channel.SendMessageAsync(userCreatedEmbed);
        }
        
        
        // -test
        [Command("test"), RequirePermissions(Permissions.Administrator)]
        public async Task Test(CommandContext msg, params string[] text)
        {
            
        }
    }
}