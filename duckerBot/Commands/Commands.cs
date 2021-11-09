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

namespace duckerBot
{
    public partial class Commands : BaseCommandModule
    {
        // -avatar
        [Command("avatar"), 
         Description("bot will send you URL of tagged user's avatar")]
        public async Task Avatar(CommandContext msg, [Description("user, whose avatar URL will send bot")] DiscordMember user = null)
        {
            if (user == null)
            {
                var incorrectAvatarCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-avatar <user>`",
                    Color = incorrectEmbedColor
                };
                incorrectAvatarCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(incorrectAvatarCommandEmbed);
            }
            else
            {
                var userAvatarEmbed = new DiscordEmbedBuilder
                {
                    Title = "User's avatar",
                    Description = $"**{user.Mention}'s avatar**",
                    ImageUrl = user.AvatarUrl,
                    Url = user.AvatarUrl,
                    Color = mainEmbedColor
                };
                userAvatarEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(userAvatarEmbed);
            }
        }
        
        [Command("avatar"), Description("bot will send you URL of tagged user's avatar")]
        public async Task Avatar(CommandContext msg, params string[] text)
        {
            var incorrectAvatarCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-avatar <user>`",
                Color = incorrectEmbedColor
            };
            incorrectAvatarCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(incorrectAvatarCommandEmbed);
        }
        
        
        // -invitelink
        [Command("invitelink"), Description("send you bot's invite link")]
        public async Task InviteLink(CommandContext msg)
        {
            var inviteLinkEmbed = new DiscordEmbedBuilder
            {
                Title = "Invite Link",
                Description = $"[for {msg.Member.Mention}]",
                Url = "https://discord.com/api/oauth2/authorize?client_id=906179696516026419&permissions=8&scope=bot",
                Color = mainEmbedColor
            };
            inviteLinkEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(inviteLinkEmbed);
        }

        [Command("invitelink"), Description("send you bot's invite link")]
        public async Task InviteLink(CommandContext msg, params string[] text)
        {
            var incorrectInviteLinkCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-invitelink`",
                Color = incorrectEmbedColor
            };
            incorrectInviteLinkCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(incorrectInviteLinkCommandEmbed);
        }
        
        
        // -random <min> <max>
        [Command("random"), Description("send you random value in your range")]
        public async Task Random(CommandContext msg, [Description("min value")] int minValue, [Description("max value")] int maxValue)
        {
            var rnd = new Random();
            if (minValue > maxValue)
            {
                var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-random <min value> <max value>`",
                    Color = incorrectEmbedColor
                };
                incorrectRandomCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
                await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
                return;
            }
            var randomEmbed = new DiscordEmbedBuilder
            {
                Title = "Random number",
                Description = $"Your random number is: **{rnd.Next(minValue, maxValue + 1)}**",
                Color = mainEmbedColor
            };
            randomEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(randomEmbed);
        }

        [Command("random"), Description("send you random value in your range")]
        public async Task Random(CommandContext msg)
        {
            var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-random <min value> <max value>`",
                Color = incorrectEmbedColor
            };
            incorrectRandomCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
        }

        [Command("random"), Description("send you random value in your range")]
        public async Task Random(CommandContext msg, params string[] text)
        {
            var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-random <min value> <max value>`\n [for {msg.Member.Mention}]",
                Color = incorrectEmbedColor
            };
            incorrectRandomCommandEmbed.WithFooter(msg.User.Username, msg.User.AvatarUrl);
            await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
        }
    }
}