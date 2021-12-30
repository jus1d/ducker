using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Database;
using ducker.Logs;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule;

public partial class AdministrationSlashCommands
{
    [SlashCommand("mute", "Mute mentioned member")]
    [RequireAdmin]
    public async Task MuteCommand(InteractionContext msg,
        [Option("member", "Member to mute")] DiscordUser user,
        [Option("reason", "Reason for mute")] string reason = "noneReason")
    {
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        var member = (DiscordMember) user;
        var muteRoleId = DB.GetId(msg.Guild.Id, "muteRoleId");
        if (muteRoleId == 0)
        {
            var muteRole =
                await msg.Guild.CreateRoleAsync("Muted", Permissions.None, DiscordColor.DarkGray, false, false);

            var channels = await msg.Guild.GetChannelsAsync();

            foreach (var channel in channels)
                await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);

            DB.Update(msg.Guild.Id, "muteRoleId", muteRole.Id.ToString());
            await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
            await Log.Audit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention}.", reason);
        }
        else
        {
            try
            {
                await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
            }
            catch
            {
                var muteRole =
                    await msg.Guild.CreateRoleAsync("Muted", Permissions.None, DiscordColor.DarkGray, false, false);

                var channels = await msg.Guild.GetChannelsAsync();

                foreach (var channel in channels)
                    await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);

                DB.Update(msg.Guild.Id, "muteRoleId", muteRole.Id.ToString());

                await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
            }

            await Log.Audit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention}.", reason);
        }
    }
}