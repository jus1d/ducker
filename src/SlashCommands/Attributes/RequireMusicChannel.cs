using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;
using ducker.Database;

namespace ducker.SlashCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireMusicChannel : SlashCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(InteractionContext msg)
        {
            ulong musicChannelId = DB.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelId = DB.GetCmdChannel(msg.Guild.Id);
            bool correctChannel = msg.Channel.Id == musicChannelId || msg.Channel.Id == cmdChannelId;
            if (!correctChannel)
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelId));
            if (musicChannelId == 0)
                await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));
            
            return correctChannel;
        }
    }
}