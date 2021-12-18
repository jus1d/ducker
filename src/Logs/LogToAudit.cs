using DSharpPlus.Entities;
using ducker.Database;

namespace ducker.Logs
{
    public partial class Log
    {
        public static async Task LogToAudit(DiscordGuild guild, string logText)
        {
            ulong auditChannelId = DB.GetLogsChannel(guild.Id);
            DiscordChannel auditChannel = guild.GetChannel(auditChannelId);
            await auditChannel.SendMessageAsync(logText);
        }
    }
}