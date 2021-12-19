using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace ducker.Commands.Attributes
{
    /// <summary>
    /// Check whether the command is executed from hell server(main bot guild)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireMainGuild : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext msg, bool help)
        {
            return msg.Guild.Id == Bot.MainGuildId;
        }

    }
}