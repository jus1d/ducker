using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ducker.Database;
using ducker.DiscordData;

namespace ducker.Commands.Attributes;

/// <summary>
///     Check whether the command is executed from music channel
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireMusicChannel : CheckBaseAttribute
{
    public override async Task<bool> ExecuteCheckAsync(CommandContext msg, bool help)
    {
        var musicChannelId = DB.GetId(msg.Guild.Id, "musicChannelId");
        var cmdChannelId = DB.GetId(msg.Guild.Id, "cmdChannelId");
        var correctChannel = msg.Channel.Id == musicChannelId || msg.Channel.Id == cmdChannelId;
        if (!correctChannel)
            await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelId));
        if (musicChannelId == 0)
            await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));

        return correctChannel;
    }
}