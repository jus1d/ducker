using System.Linq;
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
        [Command("ping"), Description("returns pong"), RequirePermissions(Permissions.Administrator)]
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
        [Command("userinfo"), Description("Bot will send you information about tagged user, or you"), RequirePermissions(Permissions.Administrator)]
        public async Task UserInfo(CommandContext msg, [Description("optional user, whose information will send bot")] DiscordMember user = null)
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

        [Command("userinfo"), Description("Bot will send you information about tagged user, or you"), RequirePermissions(Permissions.Administrator)]
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
        
        
        // -ban
        [Command("ban"), Description("banned mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Ban(CommandContext msg, [Description("user")] DiscordMember user = null)
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

        [Command("ban"), Description("banned mentioned user"), RequirePermissions(Permissions.Administrator)]
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
        [Command("kick"), Description("kick mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Kick(CommandContext msg, DiscordMember user = null)
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

        [Command("kick"), Description("kick mentioned user"), RequirePermissions(Permissions.Administrator)]
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
        public async Task ChannelID(CommandContext msg, [Description(" optional channel (for voice channels with emoji - use template: **-channelid <#id>**)")] DiscordChannel channel = null)
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
        public async Task ChannelID(CommandContext msg, [Description("if you misuse the command")] params string[] parametres)
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
    }
}