using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Attributes;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("clear", "Clear amount messages in a current channel"),
         RequireAdmin]
        public async Task ClearCommand(InteractionContext msg, [Option("amount", "Amount messages to delete")] long amount)
        {
            if (amount > 100 || amount < 0)
            {
                await msg.CreateResponseAsync(new DiscordEmbedBuilder
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
                await msg.Channel.DeleteMessagesAsync(await msg.Channel.GetMessagesAsync((int)amount + 1));

                string messageOrMessages;
                if (amount.ToString()[amount.ToString().Length - 1] == '1' && amount != 11)
                    messageOrMessages = "message";
                else
                    messageOrMessages = "messages";
                
                await msg.CreateResponseAsync(new DiscordEmbedBuilder
                {
                    Title = "Deleted messages report", 
                    Description = $"I have deleted {amount} {messageOrMessages}",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                }, true);
            }
        }
    }
}