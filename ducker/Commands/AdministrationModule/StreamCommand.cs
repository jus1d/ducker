using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        [Command("stream"), 
         Description("Send stream announcement"),
         RequirePermissions(Permissions.Administrator)]
        public async Task StreamAnnouncement(CommandContext msg, [RemainingText] string description)
        {
            await msg.Message.DeleteAsync();
            await (await msg.Channel.SendMessageAsync(msg.Guild.GetRole(Role.TwitchFollowerRoleId).Mention)).DeleteAsync();
            await (await msg.Channel.SendMessageAsync(ducker.Embed.StreamAnnouncementEmbed(msg, description))).CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":twitch:"));
        }
    }
}