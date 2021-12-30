using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Commands.Attributes;
using ducker.Database;
using ducker.Logs;

namespace ducker.Commands.AdministrationModule;

public partial class AdministrationCommands
{
    [Command("tempmute")]
    [Description("Mute member for some duration")]
    [RequireAdmin]
    public async Task TempmuteCommand(CommandContext msg, DiscordMember member, int hoursDuration,
        [RemainingText] string reason = "noneReason")
    {
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
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            await Log.Audit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention} for {hoursDuration} hours.",
                reason);
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

            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            await Log.Audit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention} for {hoursDuration} hours.",
                reason);
        }

        Thread.Sleep(hoursDuration * 3600000);
        await member.RevokeRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
        await Log.Audit(msg.Guild, $"{member.Mention} unmuted", "Mute time expired");
    }
}