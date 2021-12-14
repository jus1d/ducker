using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        /// <summary>
        /// Command to send announcement about stream
        /// </summary>
        /// <param name="msg">The context that command belongs to</param>
        /// <param name="description">Description for announcement. Optional</param>
        [Command("stream"), 
         Description("Send stream announcement"),
         RequireAdmin]
        public async Task StreamAnnouncement(CommandContext msg, [RemainingText] string description)
        {
            await msg.Message.DeleteAsync();
            await (await msg.Channel.SendMessageAsync(msg.Guild.GetRole(Role.TwitchFollowerRoleId).Mention)).DeleteAsync();
            await (await msg.Channel.SendMessageAsync(ducker.Embed.StreamAnnouncementEmbed(msg, description))).CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":twitch:"));
        }
    }
}