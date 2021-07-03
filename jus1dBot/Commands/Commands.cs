using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.VisualBasic;

namespace jus1dBot
{
    public partial class Commands : BaseCommandModule
    {
        // -useravatar
        [Command("useravatar")]
        [Description("Bot will send you URL of tagged user's avatar")]
        public async Task UserAvatar(CommandContext msg, [Description("user, whose avatar URL will send bot")] DiscordMember user)
        {
            var Embed = new DiscordEmbedBuilder
            {
                Title = "User avatar",
                Description = $"{user.Mention}'s avatar: {user.AvatarUrl}",
                Color = DiscordColor.Azure
            };
            
            await msg.Channel.SendMessageAsync(Embed).ConfigureAwait(false);
        }
        
        // -invitelink
        [Command("invitelink")]
        [Description("Send you bot's invite link")]
        public async Task InviteLink(CommandContext msg)
        {
            var Embed = new DiscordEmbedBuilder
            {
                Title = "Invite Link",
                Description = $"https://discord.com/api/oauth2/authorize?client_id=849009875031687208&permissions=8&scope=bot \n [for {msg.Member.Mention}]",
                Color = DiscordColor.Azure
            };
            
            msg.Channel.SendMessageAsync(Embed);
        }

        // -writeme <text>
        [Command("writeme")]
        [Description("Bot will type to you your text")]
        public async Task WriteMe(CommandContext msg, [Description("your text")] params string[] text)
        {
            string textForSend = "";
            
            for (int i = 0; i < text.Length; i++)
            {
                textForSend = textForSend + " " + text[i];
            }
            await msg.Member.SendMessageAsync(textForSend);
        }
        
        // -random <min> <max>
        [Command("random")]
        [Description("Send you randon value in your tange")]
        public async Task Random(CommandContext msg, [Description("minimal value")] int minValue, [Description("maximum value")]int maxValue)
        {
            var rnd = new Random();
            var Embed = new DiscordEmbedBuilder
            {
                Title = "Random number",
                Description = $"Random number: **{rnd.Next(minValue, maxValue + 1)}** [for {msg.Member.Mention}]",
                Color = DiscordColor.Azure
            };
            await msg.Channel.SendMessageAsync(Embed);
        }
    }
}