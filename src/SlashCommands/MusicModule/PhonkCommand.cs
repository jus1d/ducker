using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.MusicModule;

public partial class MusicSlashCommands
{
    [SlashCommand("phonk", "Start playing 24/7 Memphis Phonk Radio")]
    public async Task PhonkCommand(InteractionContext msg)
    {
        await PlayCommand(msg, "https://www.youtube.com/watch?v=3lwdObInlqU&ab_channel=Memphis66.6");
    }
}