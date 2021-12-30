using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule;

public partial class AdministrationSlashCommands
{
    [SlashCommand("uptime", "Send time that bot started at")]
    [RequireAdmin]
    [Hidden]
    public async Task UptimeCommand(InteractionContext msg)
    {
        var uptime = DateTime.Now - Bot.Uptime;
        var text = string.Empty;

        if (uptime.Days == 0)
            text = $"Bot started **{uptime.Hours} hours, {uptime.Minutes} minutes, {uptime.Seconds} seconds ago**";
        if (uptime.Hours == 0)
            text = $"Bot started **{uptime.Minutes} minutes, {uptime.Seconds} seconds ago**";
        if (uptime.Minutes == 0)
            text = $"Bot started **{uptime.Seconds} seconds ago**";
        if (uptime.Seconds == 0)
            text = "Bot started right now";

        await msg.CreateResponseAsync(new DiscordEmbedBuilder
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