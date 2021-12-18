﻿using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("stream", "Send stream announcement"), RequirePermissions(Permissions.Administrator)]
        public async Task StreamCommand(InteractionContext msg, [Option("description", "Stream description")] string description = "")
        {
            await msg.CreateResponseAsync(Embed.StreamAnnouncementEmbed(msg, description));
            await (await msg.Channel.SendMessageAsync(msg.Guild.GetRole(Role.TwitchFollowerRoleId).Mention)).DeleteAsync();
        }
    }
}