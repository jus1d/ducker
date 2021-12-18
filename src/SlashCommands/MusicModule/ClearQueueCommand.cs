﻿using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Database;

namespace ducker.SlashCommands.MusicModule
{
    public partial class MusicSlashCommands
    {
        [SlashCommand("clear-queue", "Clear queue list")]
        public async Task ClearQueueCommand(InteractionContext msg)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            ulong musicChannelIdFromDb = DB.GetMusicChannel(msg.Guild.Id);
            ulong cmdChannelIdFromDb = DB.GetCmdChannel(msg.Guild.Id);
            
            if (musicChannelIdFromDb == 0)
            {
                await msg.Channel.SendMessageAsync(Embed.NoMusicChannelConfigured(msg.User));
                return;
            }
            if (msg.Channel.Id != musicChannelIdFromDb && msg.Channel.Id != cmdChannelIdFromDb)
            {
                await msg.Channel.SendMessageAsync(Embed.IncorrectMusicChannelEmbed(msg, musicChannelIdFromDb));
                return;
            }
            Bot.Queue.Clear();
            await msg.Channel.SendMessageAsync(Embed.Queue(msg.User));
        }
    }
}