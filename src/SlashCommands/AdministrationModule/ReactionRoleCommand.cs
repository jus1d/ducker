using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Attributes;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("reaction-role-embed", "Send embed with reactions, press them to get role"), RequireAdmin]
        public async Task ReactionRoleCommand(InteractionContext msg)
        {
            DiscordEmoji twitchRgbEmoji = DiscordEmoji.FromName(msg.Client, ":twitchrgb:");
            DiscordEmoji chelEmoji = DiscordEmoji.FromName(msg.Client, ":chel:");
            var followButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_follow_role", "", false, new DiscordComponentEmoji(twitchRgbEmoji));
            var chelButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_chel_role", "", false, new DiscordComponentEmoji(chelEmoji));
            
            await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder()
                    .AddEmbed(ducker.Embed.ReactionRolesEmbed(msg.Client, msg.Guild))
                    .AddComponents(followButton, chelButton));
        }
    }
}