using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;

namespace ducker
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
            var channel = e.Guild.GetChannel(Database.GetLogsChannel(e.Guild.Id));
            await e.Member.GrantRoleAsync(e.Guild.GetRole(Role.ChelRoleId));
            await channel.SendMessageAsync($"{e.Member.Mention}, just landed on the `{e.Guild.Name}`");
        }

        public static async Task OnMemberRemoved(DiscordClient client, GuildMemberRemoveEventArgs e)
        {
            var channel = e.Guild.GetChannel(Database.GetLogsChannel(e.Guild.Id));
            await channel.SendMessageAsync($"{e.Member.Mention}. On siebalsya ksta");
        }

        public static async Task OnReactionAdded(DiscordClient client, MessageReactionAddEventArgs e)
        {
            if (e.User.IsBot)
                return;
        }
        
        public static async Task OnReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs e)
        {
            
        }

        public static async Task OnComponentInteractionCreated(DiscordClient client, InteractionCreateEventArgs e)
        {
            DiscordMember member = (DiscordMember) e.Interaction.User;
            DiscordEmbedBuilder grantedEmbed;
            if (e.Interaction.Data.CustomId == "get_follow_role")
            {
                if (member.Roles.Contains(e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId)))
                {
                    await member.RevokeRoleAsync(e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId));
                    grantedEmbed = new DiscordEmbedBuilder
                    {
                        Description = $"You removed your `{e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId).Name}` role",
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = e.Interaction.User.AvatarUrl,
                            Text = e.Interaction.User.Username
                        },
                        Color = Bot.MainEmbedColor
                    };
                }
                else
                {
                    await member.GrantRoleAsync(e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId));
                    grantedEmbed = new DiscordEmbedBuilder
                    {
                        Description = $"You got the `{e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId).Name}` role",
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = e.Interaction.User.AvatarUrl,
                            Text = e.Interaction.User.Username
                        },
                        Color = Bot.MainEmbedColor
                    };
                }
                
                var message =  e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(grantedEmbed).AsEphemeral(true));
            }
            else if (e.Interaction.Data.CustomId == "get_chel_role")
            {
                if (member.Roles.Contains(e.Interaction.Guild.GetRole(Role.ChelRoleId)))
                {
                    await member.RevokeRoleAsync(e.Interaction.Guild.GetRole(Role.ChelRoleId));
                    grantedEmbed = new DiscordEmbedBuilder
                    {
                        Description = $"You removed your `{e.Interaction.Guild.GetRole(Role.ChelRoleId).Name}` role",
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = e.Interaction.User.AvatarUrl,
                            Text = e.Interaction.User.Username
                        },
                        Color = Bot.MainEmbedColor
                    };
                }
                else
                {
                    await member.GrantRoleAsync(e.Interaction.Guild.GetRole(Role.ChelRoleId));
                    grantedEmbed = new DiscordEmbedBuilder
                    {
                        Description = $"You got the `{e.Interaction.Guild.GetRole(Role.ChelRoleId).Name}` role",
                        Footer = new DiscordEmbedBuilder.EmbedFooter
                        {
                            IconUrl = e.Interaction.User.AvatarUrl,
                            Text = e.Interaction.User.Username
                        },
                        Color = Bot.MainEmbedColor
                    };
                }
                
                var message = e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(grantedEmbed).AsEphemeral(true));
            }
            else if (e.Interaction.Data.CustomId == "play_button")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                var lava = client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                LavalinkGuildConnection connection;
                try
                {
                    connection = node.GetGuildConnection(member.VoiceState.Guild);
                }
                catch
                {
                    return;
                }

                if (member.VoiceState.Channel != connection.Channel)
                    return;
                await connection.ResumeAsync();
            }
            else if (e.Interaction.Data.CustomId == "pause_button")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                var lava = client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                LavalinkGuildConnection connection;
                try
                {
                    connection = node.GetGuildConnection(member.VoiceState.Guild);
                }
                catch
                {
                    return;
                }

                if (member.VoiceState.Channel != connection.Channel)
                    return;
                await connection.PauseAsync();
            }
            else if (e.Interaction.Data.CustomId == "next_button")
            {
                try
                {
                    LavalinkTrack lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
                }
                catch (Exception exception)
                {
                    await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().AddEmbed(Embed.ClearQueueEmbed(e.Interaction.User)));
                    return;
                }
                
                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                var lava = client.GetLavalink();
                var node = lava.ConnectedNodes.Values.First();
                var connection = node.GetGuildConnection(member.VoiceState.Guild);
                await connection.StopAsync();
            }
            else if (e.Interaction.Data.CustomId == "queue_button")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
                await e.Interaction.Channel.SendMessageAsync(Embed.Queue(e.Interaction.User));
            }
        }

        public static async Task OnPlaybackFinished(LavalinkGuildConnection sender, TrackFinishEventArgs e)
        {
            try
            {
                LavalinkTrack lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
            }
            catch (Exception exception)
            {
                return;
            }

            ulong musicChannelIdFromDB = Database.GetMusicChannel(sender.Guild.Id);

            await sender.PlayAsync(Bot.Queue[0]);
            await Embed.NowPlaying(sender.Node.Discord, Bot.Queue[0], await sender.Node.Discord.GetGuildAsync(696496218934608004).Result.GetMemberAsync(Bot.Id)).SendAsync(sender.Guild.GetChannel(musicChannelIdFromDB));
            Bot.Queue.Remove(Bot.Queue[0]);
        }
    }
}