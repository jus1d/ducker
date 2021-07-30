using System;
using System.Collections;
using System.Collections.Generic;
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
        [Command("useravatar"), Description("Bot will send you URL of tagged user's avatar")]
        public async Task UserAvatar(CommandContext msg, [Description("user, whose avatar URL will send bot")] DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -useravatar <member>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed).ConfigureAwait(false);
            }
            else
            {
                var embed = new DiscordEmbedBuilder
                {
                    Title = "User avatar",
                    Description = $"{user.Mention}'s avatar: {user.AvatarUrl}\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Azure
                };
                await msg.Channel.SendMessageAsync(embed).ConfigureAwait(false);
            }
        }
        
        [Command("useravatar"), Description("Bot will send you URL of tagged user's avatar")]
        public async Task UserAvatar(CommandContext msg, params string[] text)
        {
            var incorrectCommandsEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -useravatar <member>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandsEmbed);
        }
        
        
        // -invitelink
        [Command("invitelink"), Description("Send you bot's invite link")]
        public async Task InviteLink(CommandContext msg)
        {
            var embed = new DiscordEmbedBuilder
            {
                Title = "Invite Link",
                Description = $"https://discord.com/api/oauth2/authorize?client_id=849009875031687208&permissions=8&scope=bot\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Azure
            };
            
            await msg.Channel.SendMessageAsync(embed);
        }

        [Command("invitelink"), Description("Send you bot's invite link")]
        public async Task InviteLink(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -invitelink\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        // -random <min> <max>
        [Command("random"), Description("Send you random value in your range")]
        public async Task Random(CommandContext msg, [Description("minimal value")] int minValue, [Description("maximum value")] int maxValue)
        {
            var rnd = new Random();
            if (minValue > maxValue)
            {
                var incorrectCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** -random <minimal value> <maximal value>\n [for {msg.Member.Mention}]",
                    Color = DiscordColor.Red
                };
                await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
                return;
            }
            var embed = new DiscordEmbedBuilder
            {
                Title = "Random number",
                Description = $"Random number: **{rnd.Next(minValue, maxValue + 1)}** [for {msg.Member.Mention}]",
                Color = DiscordColor.Azure
            };
            await msg.Channel.SendMessageAsync(embed);
        }

        [Command("random"), Description("Send you random value in your range")]
        public async Task Random(CommandContext msg)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -random <minimal value> <maximal value>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }

        [Command("random"), Description("Send you random value in your range")]
        public async Task Random(CommandContext msg, params string[] text)
        {
            var incorrectCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -random <minimal value> <maximal value>\n [for {msg.Member.Mention}]",
                Color = DiscordColor.Red
            };
            await msg.Channel.SendMessageAsync(incorrectCommandEmbed);
        }
        
        
        /*// -bj
        [Command("bj"), Description("play blackjack with bot")]
        public async Task Blackjack(CommandContext msg)
        {
            var bjEmbed = new DiscordEmbedBuilder
            {
                
            };
        }*/
    }
}