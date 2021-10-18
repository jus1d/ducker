using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace jus1dBot
{
    public partial class Commands : BaseCommandModule
    {
        // pinging
        [Command("ping"), Description("Returns client's ping"), RequirePermissions(Permissions.Administrator)]
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
        [Command("userinfo"), Description("Bot will send you information about tagged user"), RequirePermissions(Permissions.Administrator)]
        public async Task UserInfo(CommandContext msg, [Description("User, whose information will send bot")] DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -userinfo <member>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
                return;
            }
            
            string userCreatedDate = "";
            string userJoinedDate = "";
            
            for (int i = 0; i < user.CreationTimestamp.ToString().Length - 7; i++)
            {
                userCreatedDate = userCreatedDate + user.CreationTimestamp.ToString()[i];
                userJoinedDate = userJoinedDate + user.JoinedAt.ToString()[i];
            }

            var embed = new DiscordEmbedBuilder 
            {
                Description = $"**{user.Mention}'s information**\n" +
                              $"\n" + 
                              $"User ID: {user.Id}\n" + 
                              $"Date account created: {userCreatedDate}\n" +
                              $"Date joined to server: {userJoinedDate}\n" + 
                              $"User avatar URL: {user.AvatarUrl}\n" + 
                              $"[for {msg.User.Mention}]",
                Color = DiscordColor.Azure
            };
            
            await msg.Channel.SendMessageAsync(embed);
        }

        [Command("userinfo"), Description("Bot will send you information about tagged user"), RequirePermissions(Permissions.Administrator)]
        public async Task UserInfo(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -userinfo <member>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        // -voicemute
        [Command("voicemute"), Description("Mute(voice) tagged user"), RequirePermissions(Permissions.Administrator)]
        public async Task VoiceMute(CommandContext msg, [Description("User, for mute")] DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -voicemute <member>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
                return;
            }
            
            await user.SetMuteAsync(true);
        }

        [Command("voicemute"), Description("Mute(voice) tagged user"), RequirePermissions(Permissions.Administrator)]
        public async Task VoiceMute(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -voicemute <member>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        // -voiceunmute
        [Command("voiceunmute"), Description("Unmute(voice) tagged user"), RequirePermissions(Permissions.Administrator)]
        public async Task VoiceUnmute(CommandContext msg, [Description("User, for unmute")] DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -voiceunmute <member>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
                return;
            }
            
            await user.SetMuteAsync(false);
        }

        [Command("voiceunmute"), Description("Unmute(voice) tagged user"), RequirePermissions(Permissions.Administrator)]
        public async Task VoiceUnmute(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -voiceunmute <member>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }


        // -ban
        [Command("ban"), Description("Banned mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Ban(CommandContext msg, [Description("User, for ban")] DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -ban <member>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
                return;
            }
            await user.Guild.BanMemberAsync(user);
        }

        [Command("ban"), Description("Banned mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Ban(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -ban <member>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        // -kick 
        [Command("kick"), Description("Kick mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Kick(CommandContext msg, [Description("Member:")] DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -kick <member>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
                return;
            }
            
            await user.RemoveAsync();
        }

        [Command("kick"), Description("Kick mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Kick(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -kick <member>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
            return;
        }
        

        // -channelid
        [Command("channelid"), Description("Send you tagged (or bot-commands) channel ID"), RequirePermissions(Permissions.Administrator)]
        public async Task ChannelId(CommandContext msg, [Description(" optional channel (for voice channels with emoji - use template: **-channelid <#id>**)")] DiscordChannel channel = null)
        {
            if (channel == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -channelid <channel>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed).ConfigureAwait(false);
                return;
            }
            
            var embed = new DiscordEmbedBuilder 
            {
                Title = "Channel ID", 
                Description = $"{channel.Mention} channel ID: {channel.Id}", 
                Color = DiscordColor.Azure
            };
            await msg.Channel.SendMessageAsync(embed).ConfigureAwait(false);
        }

        
        [Command("channelid"), Description("Send you tagged (or bot-commands) channel ID"), RequirePermissions(Permissions.Administrator)]
        public async Task ChannelId(CommandContext msg, [Description("if you misuse the command")] params string[] parametres)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -channelid <channel>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed).ConfigureAwait(false);
        }
        
        
        // -clone
        [Command("clone"), Description("clone channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Clone(CommandContext msg, [Description("channel to clone")] DiscordChannel channel = null)
        {
            if (channel == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -clone <channel>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
                return;
            }
            await channel.CloneAsync();
        }

        [Command("clone"), Description("clone channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Clone(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -clone <channel>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        // -mute
        [Command("mute"), Description("grant mute role to mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Mute(CommandContext msg, DiscordMember member = null)
        {
            if (member == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -mute <member>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
                return;
            }
            
            await member.GrantRoleAsync(await msg.Guild.CreateRoleAsync("Muted", Permissions.None, DiscordColor.Gray));
        }

        [Command("mute"), Description("grant mute role to mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Mute(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -mute <member>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
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
        }
    }
}