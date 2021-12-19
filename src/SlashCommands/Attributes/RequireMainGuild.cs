using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.Attributes
{
    /// <summary>
    /// Check whether the command is executed from hell server(main bot guild)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireMainGuild : SlashCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(InteractionContext msg)
        {
            return msg.Guild.Id == Bot.MainGuildId;
        }
    }
}