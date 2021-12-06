using System;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using SpotifyAPI.Web;

namespace duckerBot
{
    public partial class SlashCommands
    {
        [SlashCommand("join", "Join your voice channel")]
        public async Task Join(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            if (msg.Member.VoiceState == null || msg.Member.VoiceState.Channel == null)
            {
                await duckerBot.Embed.NotInVoiceChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            await node.ConnectAsync(msg.Member.VoiceState.Channel);
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }

        [SlashCommand("quit", "Quit voice channel"), RequirePermissions(Permissions.Administrator)]
        public async Task Quit(InteractionContext msg)
        {
            if (msg.Channel.Id != Bot.MusicChannelId && msg.Channel.Id != Bot.CmdChannelId)
            {
                await duckerBot.Embed.IncorrectMusicChannel(msg).SendAsync(msg.Channel);
                return;
            }
            var lava = msg.Client.GetLavalink();
            var node = lava.ConnectedNodes.Values.First();
            var connection = node.GetGuildConnection(msg.Member.VoiceState.Guild);
            if (connection == null)
            {
                await duckerBot.Embed.NoConnection(msg).SendAsync(msg.Channel);
                return;
            }
            await connection.DisconnectAsync();
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, ":success:"));
        }
    }
}