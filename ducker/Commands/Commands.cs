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

namespace ducker
{
    public partial class Commands : BaseCommandModule
    {
        // -avatar 
        [Command("avatar"), 
         Description("Send user's avatar and it's link to current channel"), 
         Aliases("ava")]
        public async Task Avatar(CommandContext msg, DiscordMember user)
        {
            var userAvatarEmbed = new DiscordEmbedBuilder
            {
                Title = "User's avatar",
                Description = $"**{user.Mention}'s avatar**",
                ImageUrl = user.AvatarUrl,
                Url = user.AvatarUrl,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            await msg.Channel.SendMessageAsync(userAvatarEmbed);
        }
        
        [Command("avatar")]
        public async Task Avatar(CommandContext msg, params string[] text)
        {
            var incorrectAvatarCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-avatar <user>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectAvatarCommandEmbed);
        }
        
        
        // -invite-link
        [Command("invite-link"), 
         Description("Send invite link for this bot to current channel"),
         Aliases("invite")]
        public async Task InviteLink(CommandContext msg)
        {
            var inviteLinkEmbed = new DiscordEmbedBuilder
            {
                Title = "Invite Link",
                Url = Bot.InviteLink,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            await msg.Channel.SendMessageAsync(inviteLinkEmbed);
        }

        [Command("invite-link")]
        public async Task InviteLink(CommandContext msg, params string[] text)
        {
            var incorrectInviteLinkCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-invite-link`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectInviteLinkCommandEmbed);
        }
        
        
        // -random <min> <max>
        [Command("random"), 
         Description("Send random value in your range from min to max value to current channel"),
         Aliases("rnd")]
        public async Task Random(CommandContext msg, [Description("min value")] int minValue, [Description("max value")] int maxValue)
        {
            var rnd = new Random();
            if (minValue > maxValue)
            {
                var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
                {
                    Title = $"Missing argument",
                    Description = $"**Usage:** `-random <min value> <max value>`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
                return;
            }
            var randomEmbed = new DiscordEmbedBuilder
            {
                Title = "Random number",
                Description = $"Your random number is: **{rnd.Next(minValue, maxValue + 1)}**",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            };
            await msg.Channel.SendMessageAsync(randomEmbed);
        }

        [Command("random")]
        public async Task Random(CommandContext msg)
        {
            var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-random <min value> <max value>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
        }

        [Command("random")]
        public async Task Random(CommandContext msg, params string[] text)
        {
            var incorrectRandomCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-random <min value> <max value>`\n [for {msg.Member.Mention}]",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectRandomCommandEmbed);
        }
    }
}