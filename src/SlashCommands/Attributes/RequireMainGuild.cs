using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.Attributes
{
    public class RequireMainGuild : SlashCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(InteractionContext msg)
        {
            return msg.Guild.Id == Bot.MainGuildId;
        }
    }
}