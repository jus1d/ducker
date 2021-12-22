using DSharpPlus.Entities;
using ducker.Database;
using Microsoft.VisualBasic;

namespace ducker.Logs
{
    public partial class Log
    {
        public static async Task Audit(DiscordGuild guild, string logText, string reason = "noneReason", LogType logType = LogType.Audit)
        {
            ulong auditChannelId = DB.GetId(guild.Id, "logsChannelId");
            DiscordChannel auditChannel = guild.GetChannel(auditChannelId);
            string title = String.Empty;
            string description = String.Empty;

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
}