using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace ducker
{
    public class BaseHelpFormatter 
    {
        public BaseHelpFormatter(CommandContext ctx)
        {
            
        }

        public CommandHelpMessage Build()
        {
            return new CommandHelpMessage(null, new DiscordEmbedBuilder
            {
                Title = "help"
            });
        }
    }
}