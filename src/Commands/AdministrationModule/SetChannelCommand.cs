using System.Data;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Commands.Attributes;
using ducker.Database;
using ducker.Logs;
using MySqlConnector;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationCommands
    {
        [Command("set-channel"), 
         Description("Set music channel for this guild"),
         Aliases("sc"), 
         RequireAdmin]
        public async Task SetChannelCommand(CommandContext msg, string channelType, DiscordChannel channel)
        {
            await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            
            switch (channelType)
            {
                case "music":
                    DB.Update(msg.Guild.Id, "musicChannelId", channel.Id.ToString());
                    await Log.Audit(msg.Guild, $"Music channel for this server is set to {channel.Mention} by {msg.Member.Mention}");
                    break;
                case "cmd":
                    DB.Update(msg.Guild.Id, "cmdChannelId", channel.Id.ToString());
                    await Log.Audit(msg.Guild, $"Commands channel for this server is set to {channel.Mention} by {msg.Member.Mention}");
                    break;
                case "logs":
                    DB.Update(msg.Guild.Id, "logsChannelId", channel.Id.ToString());
                    await Log.Audit(msg.Guild, $"Logs channel for this server is set to {channel.Mention} by {msg.Member.Mention}");
                    break;
            }
            await msg.Channel.SendMessageAsync(Embed.ChannelConfiguredEmbed(msg.User, channelType, channel));
        }
    }
}