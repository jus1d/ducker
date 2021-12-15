using System.Runtime.CompilerServices;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ducker.Attributes;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("phonk"), 
         Description("Start playing 24/7 Memphis Phonk Radio"),
         Aliases("ph"),
         RequireMusicChannel]
        public async Task Phonk(CommandContext msg)
        {
            await PlayCommand(msg, "https://www.youtube.com/watch?v=3lwdObInlqU&ab_channel=Memphis66.6");
        } 
    }
}