using System.Data;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.SlashCommands.Attributes;
using ducker.Database;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("set-channel", "Set music channel for this server"), RequireAdmin]
        public async Task SetChannelCommand(InteractionContext msg,
            [Option("channelType", "Channel to set")] 
            [Choice("Command channel", "cmd")] 
            [Choice("Logs channel", "logs")] 
            [Choice("Music channel", "music")] string channelType,
            [Option("channel", "Music channel")] DiscordChannel channel)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            switch (channelType)
            {
                case "music":
                    DB.Update(msg.Guild.Id, "musicChannelId", channel.Id.ToString());
                    break;
                case "cmd":
                    DB.Update(msg.Guild.Id, "cmdChannelId", channel.Id.ToString());
                    break;
                case "logs":
                    DB.Update(msg.Guild.Id, "logsChannelId", channel.Id.ToString());
                    break;
            }
            await msg.Channel.SendMessageAsync(Embed.ChannelConfiguredEmbed(msg.User, channelType, channel));
        }
    }
}