using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using ducker.DiscordData;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.MusicModule;

public partial class MusicSlashCommands
{
    [SlashCommand("repeat", "Repeat current track")]
    [RequireMusicChannel]
    public async Task RepeatCommand(InteractionContext msg)
    {
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
        {
            await msg.Channel.SendMessageAsync(Embed.NotInVoiceChannelEmbed(msg));
            return;
        }

        var lava = msg.Client.GetLavalink();
        var node = lava.ConnectedNodes.Values.First();
        var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
        if (connection == null)
        {
            await msg.Channel.SendMessageAsync(Embed.NoConnectionEmbed(msg));
            return;
        }

        if (connection.CurrentState == null || connection.CurrentState.CurrentTrack == null)
        {
            await msg.Channel.SendMessageAsync(Embed.NoTracksPlayingEmbed(msg));
            return;
        }

        Bot.Queue.Insert(0, connection.CurrentState.CurrentTrack);
        await msg.Channel.SendMessageAsync(Embed.TrackRepeatEmbed(msg.User));
    }
}