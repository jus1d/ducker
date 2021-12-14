using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        [Command("reaction-role"), 
         Description("Send an embed with buttons, by press there you will granted a role"),
         RequireAdmin]
        public async Task ReactionRolesEmbed(CommandContext msg, [RemainingText] string text)
        {
            await msg.Message.DeleteAsync();

            DiscordEmoji twitchRgbEmoji = DiscordEmoji.FromName(msg.Client, ":twitchrgb:");
            DiscordEmoji chelEmoji = DiscordEmoji.FromName(msg.Client, ":chel:");
            var followButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_follow_role", $"", false, new DiscordComponentEmoji(twitchRgbEmoji));
            var chelButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_chel_role", "", false, new DiscordComponentEmoji(chelEmoji));
            
            await msg.Channel.SendMessageAsync(new DiscordMessageBuilder()
                .AddEmbed(ducker.Embed.ReactionRolesEmbed(msg.Client, msg.Guild))
                .AddComponents(followButton, chelButton));
        }
    }
}