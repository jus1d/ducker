﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Database;
using ducker.DiscordData;
using ducker.Logs;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule;

public partial class AdministrationSlashCommands
{
    [SlashCommand("set-channel", "Set music channel for this server")]
    [RequireAdmin]
    public async Task SetChannelCommand(InteractionContext msg,
        [Option("channelType", "Channel to set")]
        [Choice("Command channel", "cmd")]
        [Choice("Logs channel", "logs")]
        [Choice("Music channel", "music")]
        string channelType,
        [Option("channel", "Music channel")] DiscordChannel channel)
    {
        await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
        switch (channelType)
        {
            case "music":
                DB.Update(msg.Guild.Id, "musicChannelId", channel.Id.ToString());
                await Log.Audit(msg.Guild,
                    $"Music channel for this server is set to {channel.Mention} by {msg.Member.Mention}");
                break;
            case "cmd":
                DB.Update(msg.Guild.Id, "cmdChannelId", channel.Id.ToString());
                await Log.Audit(msg.Guild,
                    $"Commands channel for this server is set to {channel.Mention} by {msg.Member.Mention}");
                break;
            case "logs":
                DB.Update(msg.Guild.Id, "logsChannelId", channel.Id.ToString());
                await Log.Audit(msg.Guild,
                    $"Logs channel for this server is set to {channel.Mention} by {msg.Member.Mention}");
                break;
        }

        await msg.Channel.SendMessageAsync(Embed.ChannelConfiguredEmbed(msg.User, channelType, channel));
    }
}