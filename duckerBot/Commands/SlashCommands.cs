using System;
using System.Threading.Tasks;
using DSharpPlus.SlashCommands;

namespace duckerBot
{
    public class SlashCommands : ApplicationCommandModule
    {
        [SlashCommand("test", "A slash command made to test the DSharpPlusSlashCommands library!")]
        public async Task TestCommand(InteractionContext ctx)
        {
            Console.Write("123");
        }
    }
}