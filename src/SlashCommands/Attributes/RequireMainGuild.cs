using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.Attributes;

/// <summary>
///     Check whether the command is executed from hell server(main bot guild)
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireMainGuild : SlashCheckBaseAttribute
{
    public override async Task<bool> ExecuteChecksAsync(InteractionContext msg)
    {
        if (msg.Guild.Id != Bot.MainGuildId)
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Description =
                    $"This command can be used only in `{(await msg.Client.GetGuildAsync(Bot.MainGuildId)).Name}` discord server",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            });
        return msg.Guild.Id == Bot.MainGuildId;
    }
}