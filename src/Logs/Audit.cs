using DSharpPlus.Entities;
using ducker.Database;

namespace ducker.Logs;

public class Log
{
    public static async Task Audit(DiscordGuild guild, string logText, string reason = "noneReason",
        LogType logType = LogType.Audit)
    {
        var auditChannelId = DB.GetId(guild.Id, "logsChannelId");
        var auditChannel = guild.GetChannel(auditChannelId);
        var title = string.Empty;
        var description = string.Empty;

        if (logType == LogType.Audit)
        {
            title = "Audit log";
            if (reason == "noneReason")
                description = logText;
            else
                description = $"{logText}\n**Reason: **{reason}";
        }
        else if (logType == LogType.Report)
        {
            title = "Report log";
            if (reason == "noneReason")
                description = logText;
            else
                description = $"{logText}\n**Reason: **{reason}";
        }

        await auditChannel.SendMessageAsync(new DiscordEmbedBuilder
        {
            Title = title,
            Description = description,
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = "UTC time"
            },
            Timestamp = DateTimeOffset.Now.ToUniversalTime(),
            Color = Bot.LogColor
        });
    }
}