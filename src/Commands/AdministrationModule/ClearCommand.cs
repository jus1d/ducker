﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        [Command("clear"),
         Description("Clear `amount` messages from current channel"),
         RequireAdmin]
        public async Task ClearCommand(CommandContext msg, int amount, [RemainingText] string reason = "No reason given")
        {
            if (amount > 100 || amount < 0)
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Title = "Missing argument",
                    Description = "**Usage:** `-clear <amount> (amount must be less than 100 and bigger than 0)`",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                });
            }
            else
            {
                await msg.Channel.DeleteMessagesAsync(await msg.Channel.GetMessagesAsync(amount + 1));

                string messageOrMessages;
                if (amount.ToString()[amount.ToString().Length - 1] == '1' && amount != 11)
                    messageOrMessages = "message";
                else
                    messageOrMessages = "messages";
                
                DiscordMessage message = await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Title = "Deleted messages report", 
                    Description = $"I have deleted {amount} {messageOrMessages}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                });
                Thread.Sleep(3000);
                await msg.Channel.DeleteMessageAsync(message);
                
                // TODO: logs(@jus1d cleared amount messages from #channel, message list:)
            }
        }
        
        [Command("clear")]
        public async Task ClearCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** `-clear <amount>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            });
        }
    }
}