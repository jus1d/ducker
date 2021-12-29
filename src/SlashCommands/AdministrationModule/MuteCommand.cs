using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.SlashCommands.Attributes;
using ducker.Database;
using ducker.Logs;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("mute", "Mute mentioned member"), RequireAdmin]
        public async Task MuteCommand(InteractionContext msg, [Option("member", "Member to mute")] DiscordUser user, [Option("reason", "Reason for mute")] string reason = "noneReason")
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            DiscordMember member = (DiscordMember) user;
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
                    DiscordRole muteRole = await msg.Guild.CreateRoleAsync("Muted", Permissions.None, DiscordColor.DarkGray, false, false);

                    var channels = await msg.Guild.GetChannelsAsync();

                    foreach (var channel in channels)
                    {
                        await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);
                    }

                    DB.Update(msg.Guild.Id, "muteRoleId", muteRole.Id.ToString());
                
                    await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
                }
                await Log.Audit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention}.", reason);
            }
        }
    }
}