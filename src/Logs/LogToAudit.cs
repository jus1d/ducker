using DSharpPlus.Entities;
using ducker.Database;
using Microsoft.VisualBasic;

namespace ducker.Logs
{
    public partial class Log
    {
        public static async Task LogToAudit(DiscordGuild guild, string logText)
        {
            ulong auditChannelId = DB.GetId(guild.Id, "logsChannelId");
            DiscordChannel auditChannel = guild.GetChannel(auditChannelId);
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
    }
}