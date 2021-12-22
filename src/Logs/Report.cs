using DSharpPlus.Entities;
using ducker.Database;

namespace ducker.Logs
{
    public partial class Log
    {
        public static async Task Report(DiscordGuild guild, string logText)
        {
            ulong auditChannelId = DB.GetId(guild.Id, "logsChannelId");
            DiscordChannel auditChannel = guild.GetChannel(auditChannelId);
            await auditChannel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Report log",
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