using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace ducker.Attributes
{
    /// <summary>
    /// Check whether the command is executed from hell server(main bot guild)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireMainGuild : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext msg, bool help)
        {
            return Task.FromResult(msg.Guild.Id == Bot.MainGuildId);
        }

    }
}