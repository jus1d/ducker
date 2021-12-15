using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ducker.Attributes;

namespace ducker.Commands.MusicModule
{
    public partial class MusicCommands
    {
        [Command("queue"),
         Description("Send queue list"),
         RequireMusicChannel]
        public async Task QueueCommand(CommandContext msg)
        {
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
            if (musicChannelIdFromDb == 0)
            {
                await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));
                return;
            }
            if (msg.Channel.Id != musicChannelIdFromDb && msg.Channel.Id != cmdChannelIdFromDb)
            {
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelIdFromDb));
                return;
            }
            await msg.Channel.SendMessageAsync(Embed.Queue(msg.User));
        }

        [Command("queue")]
        public async Task QueueCommand(CommandContext msg, [RemainingText] string text)
        {
            ulong musicChannelIdFromDb = Database.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = Database.GetCmdChannel(msg.Guild.Id);
            
            if (musicChannelIdFromDb == 0)
            {
                await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));
                return;
            }
            if (msg.Channel.Id != musicChannelIdFromDb && msg.Channel.Id != cmdChannelIdFromDb)
            {
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelIdFromDb));
                return;
            }
            await msg.Channel.SendMessageAsync(Embed.Queue(msg.User));
        }
    }
}