using System.Data;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;
using ducker.Logs;
using MySqlConnector;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        [Command("mute"),
         Description("Mute mentioned member"),
         RequireAdmin]
        public async Task MuteCommand(CommandContext msg, DiscordMember member, [RemainingText] string reason = "No reason given")
        {
            ulong muteRoleId = Database.GetMuteRoleId(msg.Guild.Id);
            if (muteRoleId == 0)
            {
                DiscordRole muteRole = await msg.Guild.CreateRoleAsync("Muted", Permissions.None, DiscordColor.DarkGray, false, false);

                var channels = await msg.Guild.GetChannelsAsync();

                foreach (var channel in channels)
                {
                    await channel.AddOverwriteAsync(muteRole, Permissions.None, Permissions.SendMessages);
                }

                muteRoleId = muteRole.Id;

                Database database = new Database();
                DataTable table = new DataTable();
                DataTable findGuildTable = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand findGuildCommand = new MySqlCommand($"SELECT * FROM `ducker` WHERE `guildId` = '{msg.Guild.Id}'", 
                    database.GetConnection());
                adapter.SelectCommand = findGuildCommand;
                adapter.Fill(findGuildTable);
                
                MySqlCommand command = new MySqlCommand($"UPDATE `ducker` SET `muteRoleId` = {muteRoleId} WHERE `ducker`.`guildId` = {msg.Guild.Id}",
                    database.GetConnection());
            
                adapter.SelectCommand = command;
                adapter.Fill(table);
                await member.GrantRoleAsync(msg.Guild.GetRole(Database.GetMuteRoleId(msg.Guild.Id)));
                await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            }
            else
            {
                await member.GrantRoleAsync(msg.Guild.GetRole(Database.GetMuteRoleId(msg.Guild.Id)));
                await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
                await Log.LogToAudit(msg.Guild, $"{msg.Member.Mention} muted {member.Mention}. Reason: {reason}");
            }
        }
        
        [Command("mute")]
        public async Task Mute(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Missing argument",
                Description = "**Usage:** `-mute <member>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            });
        }
    }
}