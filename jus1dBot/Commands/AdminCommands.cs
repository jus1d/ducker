using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace jus1dBot
{
    public partial class Commands : BaseCommandModule
    {
        // pinging
        [Command("ping")]
        [Description("returns pong")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task Ping(CommandContext msg)
        {
            await msg.Channel.SendMessageAsync("pong");
        }
        
        // -userinfo
        [Command("userinfo")]
        [RequireRoles(RoleCheckMode.All, "admin")]
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
        
        // -useravatar
        [Command("useravatar")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        [Description("Bot will send you URL of tagged user's avatar")]
        public async Task UserAvatar(CommandContext msg, [Description("user, whose avatar URL will send bot")] DiscordMember user)
        {
            await msg.Channel.SendMessageAsync($"{user.Mention}'s avatar URL: {user.AvatarUrl}").ConfigureAwait(false);
        }
        
        
    }
}