﻿using DSharpPlus;
using DSharpPlus.EventArgs;
using ducker.Logs;

namespace ducker.Events
{
    public partial class EventHandler
    {
        public static async Task OnMemberAdded(DiscordClient client, GuildMemberAddEventArgs e)
        {
            await e.Member.GrantRoleAsync(e.Guild.GetRole(Role.ChelRoleId));
            await Log.LogToAudit(e.Guild, $"{e.Member.Mention}, just landed on the `{e.Guild.Name}`");
        }
    }
}