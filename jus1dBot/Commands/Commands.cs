using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.VisualBasic;

namespace jus1dBot
{
    public class Commands : BaseCommandModule
    {
        // pinging
        [Command("ping")]
        [Description("returns pong")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task Ping(CommandContext msg)
        {
            msg.Channel.SendMessageAsync("pong");
        }

        // response
        [Command("response")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task Resonse(CommandContext msg)
        {
            var interactivity = msg.Client.GetInteractivity();

            var message = await interactivity.WaitForMessageAsync(x => x.Channel == msg.Channel).ConfigureAwait(false);

            await msg.Channel.SendMessageAsync(message.Result.Content);
        }

        // -channelid
        [Command("channelid")]
        [Description("Returns current channel ID")]
        public async Task ChannelID(CommandContext msg)
        {
            if(msg.Channel.Name != "bot-commands")
                return;
            
            msg.Channel.SendMessageAsync(msg.Channel.Id.ToString()).ConfigureAwait(false);
        }
        [Command("channelid")]
        [Description("Returns current channel ID")]
        public async Task ChannelID(CommandContext msg, DiscordChannel channel)
        {
            if(msg.Channel.Name != "bot-commands")
                return;
            
            msg.Channel.SendMessageAsync($"{channel.Mention} channel ID: {channel.Id}").ConfigureAwait(false);
        }
        [Command("channelid")]
        [Description("Returns current channel ID")]
        public async Task ChannelID(CommandContext msg, params string[] parametres)
        {
            if(msg.Channel.Name != "bot-commands")
                return;

            var templateEmbed = new DiscordEmbedBuilder
            {
                Title = "Template -channelid:",
                Description = "-channelid <channel>",
                Color = DiscordColor.Azure
                
            };
            
            msg.Channel.SendMessageAsync(templateEmbed).ConfigureAwait(false);
        }
        
        // -invitelink
        [Command("invitelink")]
        public async Task InviteLink(CommandContext msg)
        {
            var message = msg.Channel.SendMessageAsync($"Here your link, {msg.User.Mention}\n " +
                                                       $"https://discord.com/api/oauth2/authorize?client_id=849009875031687208&permissions=8&scope=bot");
        }
        
        // -userinfo
        [Command("userinfo")]
        public async Task UserInfo(CommandContext msg)
        {
            var user = msg.User;
            
            string userCreatedDate = "";
            
            for (int i = 0; i < user.CreationTimestamp.ToString().Length - 7; i++)
            {
                userCreatedDate = userCreatedDate + user.CreationTimestamp.ToString()[i];
            }

            msg.Channel.SendMessageAsync($"{user.Mention}'s Info:\n" +
                                         $"User ID: {user.Id}\n" +
                                         $"Date account created: {userCreatedDate}\n" +
                                         $"User's avatar URL: {user.AvatarUrl}");
        }
        [Command("userinfo")]
        public async Task UserInfo(CommandContext msg, DiscordMember user)
        {
            string userCreatedDate = "";
            
            for (int i = 0; i < user.CreationTimestamp.ToString().Length - 7; i++)
            {
                userCreatedDate = userCreatedDate + user.CreationTimestamp.ToString()[i];
            }
            
            msg.Channel.SendMessageAsync($"{user.Mention}'s Info:\n" +
                                         $"User ID: {user.Id}\n" +
                                         $"Date account created: {userCreatedDate}\n" +
                                         $"User's avatar URL: {user.AvatarUrl}");
        }
        
        // -useravatar <@user>
        [Command("useravatar")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task UserAvatar(CommandContext msg, DiscordMember user)
        {
            msg.Channel.SendMessageAsync($"{user.Mention}'s avatar URL: {user.AvatarUrl}").ConfigureAwait(false);
        }
        
        // -clearallchannels
        /*[Command("clearallchannels")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task ClearAllChannels(CommandContext msg)
        {
            msg.Guild.DeleteAllChannelsAsync().ConfigureAwait(false);
        }*/
        
        
    }
}