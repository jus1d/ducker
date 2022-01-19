using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.DiscordData;

namespace ducker.SlashCommands.MiscModule;

public partial class MiscSlashCommands
{
    [SlashCommand("about", "Send information about mentioned member or you")]
    public async Task AboutCommand(InteractionContext msg, DiscordUser user = null)
    {
        DiscordMember member = (DiscordMember) user ?? msg.Member;
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        await msg.Channel.SendMessageAsync(Embed.AboutMemberEmbed(member));
    }
}