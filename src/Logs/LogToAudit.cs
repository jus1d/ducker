using DSharpPlus.Entities;

namespace ducker.Logs
{
    public partial class Log
    {
        public static async Task LogToAudit(DiscordGuild guild, string logText)
        {
            ulong auditChannelId = Database.GetLogsChannel(guild.Id);
            DiscordChannel auditChannel = guild.GetChannel(auditChannelId);
            await auditChannel.SendMessageAsync(logText);
        }
    }
}