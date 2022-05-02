using DSharpPlus.SlashCommands;
using ducker.Database;
using ducker.DiscordData;

namespace ducker.SlashCommands.Attributes;

/// <summary>
///     Check whether the command is executed from music channel
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireMusicChannel : SlashCheckBaseAttribute
{
    public override async Task<bool> ExecuteChecksAsync(InteractionContext msg)
    {
        /*var musicChannelId = DB.GetId(msg.Guild.Id, "musicChannelId");
        var cmdChannelId = DB.GetId(msg.Guild.Id, "cmdChannelId");
        var correctChannel = msg.Channel.Id == musicChannelId || msg.Channel.Id == cmdChannelId;
        if (!correctChannel)
            await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelId));
        if (musicChannelId == 0)
            await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));

        return correctChannel;*/
        return true;
    }
}