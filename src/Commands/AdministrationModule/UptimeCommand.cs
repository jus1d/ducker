using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Commands.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationCommands
    {
        [Command("uptime"), 
         Description("Send time that bot started at"),
         Aliases("up"), RequireAdmin, Hidden]
        public async Task UptimeCommand(CommandContext msg)
        {
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            TimeSpan uptime = DateTime.Now - Bot.Uptime;
            string text = String.Empty;
            
            if (uptime.Days == 0)
                text = $"Bot started **{uptime.Hours} hours, {uptime.Minutes} minutes, {uptime.Seconds} seconds ago**";
            if (uptime.Hours == 0)
                text = $"Bot started **{uptime.Minutes} minutes, {uptime.Seconds} seconds ago**";
            if (uptime.Minutes == 0)
                text = $"Bot started **{uptime.Seconds} seconds ago**";
            if (uptime.Seconds == 0)
                text = "Bot started right now";

            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Uptime",
                Description = text,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl, Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            });
        }
    }
}