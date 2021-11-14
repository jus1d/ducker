using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.Net.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace duckerBot
{
    public class EventHandler
    {
        public static async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs msg)
        {
            if (msg.Author.IsBot)
                return;

            DiscordMember member = (DiscordMember)msg.Author; // mows
                
            if (msg.Message.MentionEveryone)
            {
                if (member.Guild.Permissions == Permissions.Administrator) // ignore owner
                    return;
                    
                await msg.Message.DeleteAsync();
                var embed = new DiscordEmbedBuilder();
                embed
                    .WithTitle($"Don't tag everyone!")
                    .WithColor(DiscordColor.Red)
                    .WithFooter(msg.Author.Username, msg.Author.AvatarUrl);
                await msg.Message.Channel.SendMessageAsync(embed);
            }
        }
        
        internal static async Task OnMemberAdded(DiscordClient client, GuildMemberAddEventArgs member)
        {
            DiscordChannel channel = await client.GetChannelAsync(787190218221944862);
            if (channel?.Guild.Id == member.Guild.Id)
            {
                await channel.AddOverwriteAsync(member.Member, Permissions.AccessChannels, Permissions.None);
                DiscordEmbed message = new DiscordEmbedBuilder
                {
                    Description = "User '" + member.Member.Username + "#" + member.Member.Discriminator + "' has left the server.",
                    Color = DiscordColor.Green
                };
                await channel.SendMessageAsync(message);
            }
        }
    }
}