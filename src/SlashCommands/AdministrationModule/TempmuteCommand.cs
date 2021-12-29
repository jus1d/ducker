using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Database;
using ducker.Logs;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("tempmute", "Temporarily mute mentioned member"), RequireAdmin]
        public async Task TempmuteCommand(InteractionContext msg, [Option("member", "Member to mute")] DiscordUser user,
            [Option("duration", "Mute duration in hours")] long hoursDuration,
            [Option("reason", "Reason to mute")] string reason = "noneReason")
        {
            DiscordMember member = (DiscordMember)user;
            ulong muteRoleId = DB.GetId(msg.Guild.Id, "muteRoleId");
            if (muteRoleId == 0)
            {
                DiscordRole muteRole = await msg.Guild.CreateRoleAsync("Muted", Permissions.None, DiscordColor.DarkGray, false, false);

                var channels = await msg.Guild.GetChannelsAsync();

                foreach (var channel in channels)
                {
                    await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);
                }

                DB.Update(msg.Guild.Id, "muteRoleId", muteRole.Id.ToString());
                
                await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
                await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
                await Log.Audit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention} for {hoursDuration} hours.", reason);
            }
            else
            {
                try
                {
                    await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
                }
                catch
                {
                    DiscordRole muteRole = await msg.Guild.CreateRoleAsync("Muted", Permissions.None, DiscordColor.DarkGray, false, false);

                    var channels = await msg.Guild.GetChannelsAsync();

                    foreach (var channel in channels)
                    {
                        await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);
                    }

                    DB.Update(msg.Guild.Id, "muteRoleId", muteRole.Id.ToString());
                
                    await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
                }
                await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
                await Log.Audit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention} for {hoursDuration} hours.", reason);
            }
            
            Thread.Sleep((int)hoursDuration * 3600000);
            await member.RevokeRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
            await Log.Audit(msg.Guild, $"{member.Mention} unmuted", "Mute time expired");
        }
    }
}