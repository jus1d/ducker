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
                var embed = new DiscordEmbedBuilder
                {
                    Title = $"Don't tag everyone!",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.Author.AvatarUrl,
                        Text = msg.Author.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Message.Channel.SendMessageAsync(embed);
            }
        }
        
        public static async Task OnMemberAdded(DiscordClient client, GuildMemberAddEventArgs e)
        {
            var channel = e.Guild.GetChannel(Bot.ServerLogsChannelId);
            await e.Member.GrantRoleAsync(e.Guild.GetRole(816666984745140254));
            await channel.SendMessageAsync($"{e.Member.Mention}, just landed on the `{e.Guild.Name}`");
        }

        public static async Task OnMemberRemoved(DiscordClient client, GuildMemberRemoveEventArgs e)
        {
            var channel = e.Guild.GetChannel(Bot.ServerLogsChannelId);
            await channel.SendMessageAsync($"{e.Member.Mention}. On siebalsya ksta");
        }

        public static async Task OnReactionAdded(DiscordClient client, MessageReactionAddEventArgs e)
        {
            if (e.User.IsBot)
                return;

            if (e.Emoji == DiscordEmoji.FromName(client, ":play_pause:") && e.Channel.Id == Bot.MusicChannelId)
            {
                await e.Message.DeleteReactionAsync(e.Emoji, e.User);
                var lava = client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                DiscordMember member = (DiscordMember) e.User;
                var connection = node.GetGuildConnection(member.VoiceState.Guild);
                // here must be connection playing check
            }
            else if (e.Emoji == DiscordEmoji.FromName(client, ":arrow_forward:") && e.Channel.Id == Bot.MusicChannelId)
            {
                await e.Message.DeleteReactionAsync(e.Emoji, e.User);
                var lava = client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                DiscordMember member = (DiscordMember) e.User;
                var connection = node.GetGuildConnection(member.VoiceState.Guild);

                if (((DiscordMember) e.User).VoiceState.Channel != connection.Channel)
                    return;
                await connection.ResumeAsync();
            }
            else if (e.Emoji == DiscordEmoji.FromName(client, ":pause_button:") && e.Channel.Id == Bot.MusicChannelId)
            {
                await e.Message.DeleteReactionAsync(e.Emoji, e.User);
                var lava = client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                DiscordMember member = (DiscordMember) e.User;
                var connection = node.GetGuildConnection(member.VoiceState.Guild);

                if (((DiscordMember) e.User).VoiceState.Channel != connection.Channel)
                    return;
                await connection.PauseAsync();
            }
            else if (e.Emoji == DiscordEmoji.FromName(client, ":twitchrgb:") && e.Message.Id == Bot.ReactionRolesMessageId)
            {
                await e.Message.DeleteReactionAsync(e.Emoji, e.User);
                DiscordMember member = (DiscordMember) e.User;
                
                if (member.Roles.Contains(e.Guild.GetRole(914921577634754600)))
                    await member.RevokeRoleAsync(e.Guild.GetRole(914921577634754600));
                else
                    await member.GrantRoleAsync(e.Guild.GetRole(914921577634754600));
            }
            else if (e.Emoji == DiscordEmoji.FromName(client, ":chel:") && e.Message.Id == Bot.ReactionRolesMessageId)
            {
                await e.Message.DeleteReactionAsync(e.Emoji, e.User);
                DiscordMember member = (DiscordMember) e.User;
                
                if (member.Roles.Contains(e.Guild.GetRole(816666984745140254)))
                    await member.RevokeRoleAsync(e.Guild.GetRole(816666984745140254));
                else
                    await member.GrantRoleAsync(e.Guild.GetRole(816666984745140254));
            }
        }
        
        public static async Task OnReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs e)
        {
            
        }
    }
}