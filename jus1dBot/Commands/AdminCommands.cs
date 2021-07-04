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
        [Command("voicemute"), Description("Mute(voice) tagged user"), RequirePermissions(Permissions.Administrator)]
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

                msg.Channel.SendMessageAsync(templateEmbed);
                return;
            }
            
            user.SetMuteAsync(true);
        }
        
        
        // -voiceunmute
        [Command("voiceunmute"), Description("Unmute(voice) tagged user"), RequirePermissions(Permissions.Administrator)]
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

                msg.Channel.SendMessageAsync(templateEmbed);
                return;
            }
            
            user.SetMuteAsync(false);
        }
        
        
        // -ban
        [Command("ban"), Description("banned mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Ban(CommandContext msg, [Description("user")] DiscordMember user)
        {
            user.Guild.BanMemberAsync(user);
            
        }
        
        
        // -kick 
        [Command("kick"), Description("kick mentioned user"), RequirePermissions(Permissions.Administrator)]
        public async Task Kick(CommandContext msg, DiscordMember user)
        {
            await user.RemoveAsync();
        }
        
        
        // -mute
        [Command("mute"), Description("give mute role"), RequirePermissions(Permissions.Administrator)]
        public async Task Mute(CommandContext msg, [Description("user to mute")] DiscordMember user, [Description("role")] DiscordRole role)
        {
            user.GrantRoleAsync(role);
        }


        // -channelid
        [Command("channelid"), Description("Send you tagged (or bot-commands) channel ID"), RequirePermissions(Permissions.Administrator)]
        public async Task ChannelID(CommandContext msg, [Description(" optional channel (for voice channels with emoji - use template: **-channelid <#id>**)")] DiscordChannel channel = null)
        {
            if (channel == null)
            {
                var Embed = new DiscordEmbedBuilder
                {
                    Title = "Channel ID",
                    Description = $"{msg.Channel.Mention} channel ID: {msg.Channel.Id}",
                    Color = DiscordColor.Azure
                };
                
                await msg.Channel.SendMessageAsync(Embed).ConfigureAwait(false);
            }
            else
            {
                var Embed = new DiscordEmbedBuilder
                {
                    Title = "Channel ID",
                    Description = $"{channel.Mention} channel ID: {channel.Id}",
                    Color = DiscordColor.Azure
                };
                
                await msg.Channel.SendMessageAsync(Embed).ConfigureAwait(false);
            }
        }
        
        
        // -channelid <text>
        [Command("channelid"), Description("Send you tagged (or bot-commands) channel ID"), RequirePermissions(Permissions.Administrator)]
        public async Task ChannelID(CommandContext msg, [Description("if you misuse the command")] params string[] parametres)
        {
            if(msg.Channel.Name != "bot-commands")
                return;

            var templateEmbed = new DiscordEmbedBuilder
            {
                Title = "Template -channelid:",
                Description = "-channelid <channel>\n" +
                              "for voice channels with emoji - use template: **-channelid <#id>**",
                Color = DiscordColor.Azure
                
            };
            
            await msg.Channel.SendMessageAsync(templateEmbed).ConfigureAwait(false);
        }
        
        
        // -clone
        [Command("clone"), Description("clone channel"), RequirePermissions(Permissions.Administrator)]
        public async Task TestCommand(CommandContext msg, [Description("channel to clone")] DiscordChannel channel = null)
        {
            if (channel == null)
            {
                msg.Channel.CloneAsync();
            }
            else
            {
                channel.CloneAsync();
            }
        }
    }
}