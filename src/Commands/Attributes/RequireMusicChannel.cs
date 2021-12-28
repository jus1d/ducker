﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using ducker.DiscordData;
using ducker.Database;

namespace ducker.Commands.Attributes
{
    /// <summary>
    /// Check whether the command is executed from music channel
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireMusicChannel : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext msg, bool help)
        {
            ulong musicChannelId = DB.GetId(msg.Guild.Id, "musicChannelId");
            ulong cmdChannelId = DB.GetId(msg.Guild.Id, "cmdChannelId");
            bool correctChannel = msg.Channel.Id == musicChannelId || msg.Channel.Id == cmdChannelId;
            if (!correctChannel)
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelId));
            if (musicChannelId == 0)
                await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));
            
            return correctChannel;
        }

    }
}