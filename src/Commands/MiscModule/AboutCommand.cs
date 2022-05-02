using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.DiscordData;

namespace ducker.Commands.MiscModule;

public partial class MiscCommands
{
    [Command("about")]
    public async Task AboutCommand(CommandContext msg, DiscordMember mbr = null)
    {
        DiscordMember member = mbr ?? msg.Member;
        await msg.Channel.SendMessageAsync(Embed.AboutMemberEmbed(member));
    }
}