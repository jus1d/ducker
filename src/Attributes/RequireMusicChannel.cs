using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace ducker.Attributes
{
    /// <summary>
    /// Check whether the command is executed from hell server(main bot guild)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireMusicChannel : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext msg, bool help)
        {
            ulong musicChannelId = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelId = Database.GetCmdChannel(msg.Guild.Id);
            bool correctChannel = msg.Channel.Id == musicChannelId || msg.Channel.Id == cmdChannelId;
            if (!correctChannel)
                msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelId));
            
            return Task.FromResult(correctChannel);
        }

    }
}