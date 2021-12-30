﻿using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.DiscordData;
using ducker.SlashCommands.Attributes;

namespace ducker.SlashCommands.AdministrationModule;

public partial class AdministrationSlashCommands
{
    [SlashCommand("reaction-role", "Send embed with reactions, press them to get role")]
    [RequireAdmin]
    [RequireMainGuild]
    public async Task ReactionRoleCommand(InteractionContext msg)
    {
        var twitchRgbEmoji = DiscordEmoji.FromName(msg.Client, ":twitchrgb:");
        var chelEmoji = DiscordEmoji.FromName(msg.Client, ":chel:");
        var followButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_follow_role", "", false,
            new DiscordComponentEmoji(twitchRgbEmoji));
        var chelButton = new DiscordButtonComponent(ButtonStyle.Secondary, "get_chel_role", "", false,
            new DiscordComponentEmoji(chelEmoji));

        await msg.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
            new DiscordInteractionResponseBuilder()
                .AddEmbed(Embed.ReactionRolesEmbed(msg.Client, msg.Guild))
                .AddComponents(followButton, chelButton));
    }
}