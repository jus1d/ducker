using DSharpPlus.SlashCommands;
using ducker.Database;

namespace ducker.SlashCommands.Attributes
{
    /// <summary>
    /// Check whether the command is executed from music channel
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireMusicChannel : SlashCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(InteractionContext msg)
        {
            ulong musicChannelId = DB.GetId(msg.Guild.Id, "musicChannelId");
            ulong cmdChannelId = DB.GetId(msg.Guild.Id, "cmdChannelId");
            bool correctChannel = msg.Channel.Id == musicChannelId || msg.Channel.Id == cmdChannelId;
            if (!correctChannel)
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelId));
            if (musicChannelId == 0)
                await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));
            
            return correctChannel;
        }
    }
}