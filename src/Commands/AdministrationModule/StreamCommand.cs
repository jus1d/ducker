using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Commands.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationCommands
    {
        [Command("stream"), 
         Description("Send stream announcement"),
         RequireAdmin]
        public async Task StreamCommand(CommandContext msg, [RemainingText] string description)
        {
            await msg.Message.DeleteAsync();
            await (await msg.Channel.SendMessageAsync(msg.Guild.GetRole(Role.TwitchFollowerRoleId).Mention)).DeleteAsync();
            await (await msg.Channel.SendMessageAsync(Embed.StreamAnnouncementEmbed(msg, description))).CreateReactionAsync(DiscordEmoji.FromName(msg.Client, ":twitch:"));
        }
    }
}