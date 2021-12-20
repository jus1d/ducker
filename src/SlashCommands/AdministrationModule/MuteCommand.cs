using System.Data;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.SlashCommands.Attributes;
using ducker.Database;
using ducker.Logs;
using MySqlConnector;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("mute", "Mute mentioned member"), RequireAdmin]
        public async Task MuteCommand(InteractionContext msg, [Option("member", "Member to mute")] DiscordUser user, [Option("reason", "Reason for mute")] string reason = "No reason given")
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

                muteRoleId = muteRole.Id;

                DB db = new DB();
                DataTable table = new DataTable();
                DataTable findGuildTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand findGuildCommand = new MySqlCommand($"SELECT * FROM `ducker` WHERE `guildId` = '{msg.Guild.Id}'", 
                    db.GetConnection());
                adapter.SelectCommand = findGuildCommand;
                adapter.Fill(findGuildTable);
                
                MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `muteRoleId` = {muteRoleId} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                    db.GetConnection());
            
                adapter.SelectCommand = command;
                adapter.Fill(table);
                await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
                await Log.LogToAudit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention}. Reason: {reason}");
            }
            else
            {
                await member.GrantRoleAsync(msg.Guild.GetRole(DB.GetId(msg.Guild.Id, "muteRoleId")));
                await Log.LogToAudit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention}. Reason: {reason}");
            }
        }
    }
}