using DSharpPlus.Entities;
using ducker.Database;

namespace ducker.Logs
{
    public partial class Log
    {
        public static async Task Audit(DiscordGuild guild, string logText, string reason = "noneReason")
        {
            ulong auditChannelId = DB.GetId(guild.Id, "logsChannelId");
            DiscordChannel auditChannel = guild.GetChannel(auditChannelId);

            if (reason == "noneReason")
            {
                await auditChannel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Title = "Audit log",
                    Description = logText,
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        Text = "UTC time"
                    },
                    Timestamp = DateTimeOffset.Now.ToUniversalTime(),
                    Color = Bot.LogColor
                });
            }
            else
            {
                await auditChannel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Title = "Audit log",
                    Description = $"{logText}\n**Reason: **{reason}",
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
}