using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.VisualBasic;

namespace jus1dBot
{
    public partial class Commands
    {
        [Command("join")]
        public async Task Join(CommandContext msg, DiscordChannel channel)
        {
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await msg.RespondAsync("Connection is not established");
                return;
            }
            
            var node = lava.ConnectedNodes.Values.First();
            
            if (channel.Type != ChannelType.Voice)
            {
                await msg.RespondAsync("Not a valid voice channel.");
                return;
            }
            
            await node.ConnectAsync(channel);
            await msg.Channel.SendMessageAsync($"Joined {channel.Name}!");
        }

        [Command("quit")]
        public async Task Quit(CommandContext msg, DiscordChannel channel)
        {
            var lava = msg.Client.GetLavalink();
            if (!lava.ConnectedNodes.Any())
            {
                await msg.RespondAsync("Connection is not established");
                return;
            }

            var node = lava.ConnectedNodes.Values.First();

            if (channel.Type != ChannelType.Voice)
            {
                await msg.RespondAsync("Not a valid voice channel.");
                return;
            }

            var conn = node.GetGuildConnection(channel.Guild);

            if (conn == null)
            {
                await msg.RespondAsync("I'm is not connected.");
                return;
            }

            await conn.DisconnectAsync();
            await msg.RespondAsync($"Left {channel.Name}!");
        }
    }
}