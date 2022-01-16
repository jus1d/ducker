using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Lavalink;
using ducker.DiscordData;
using ducker.Logs;

namespace ducker.Events;

public partial class EventHandler
{
    public static async Task OnComponentInteractionCreated(DiscordClient client, InteractionCreateEventArgs e)
    {
        var member = (DiscordMember) e.Interaction.User;
        DiscordEmbedBuilder grantedEmbed;
        if (e.Interaction.Data.CustomId == "get_follow_role")
        {
            if (member.Roles.Contains(e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId)))
            {
                await member.RevokeRoleAsync(e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId));
                await Log.Audit(e.Interaction.Guild,
                    $"{e.Interaction.User.Mention} remove his {e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId).Name} role");
                grantedEmbed = new DiscordEmbedBuilder
                {
                    Description =
                        $"You removed your `{e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId).Name}` role",
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
                await Log.Audit(e.Interaction.Guild,
                    $"{e.Interaction.User.Mention} get a {e.Interaction.Guild.GetRole(Role.TwitchFollowerRoleId).Name} role");
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

            var message = e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().AddEmbed(grantedEmbed).AsEphemeral(true));
        }
        else if (e.Interaction.Data.CustomId == "get_chel_role")
        {
            if (member.Roles.Contains(e.Interaction.Guild.GetRole(Role.ChelRoleId)))
            {
                await member.RevokeRoleAsync(e.Interaction.Guild.GetRole(Role.ChelRoleId));
                await Log.Audit(e.Interaction.Guild,
                    $"{e.Interaction.User.Mention} get a {e.Interaction.Guild.GetRole(Role.ChelRoleId).Name} role");
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
                await Log.Audit(e.Interaction.Guild,
                    $"{e.Interaction.User.Mention} get a {e.Interaction.Guild.GetRole(Role.ChelRoleId).Name} role");
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

            await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
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
                var lavalinkTrack = Bot.Queue[0]; // try use list's element to catch exception
            }
            catch
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
}