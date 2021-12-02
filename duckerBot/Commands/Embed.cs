using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;

namespace duckerBot
{
    public class Embed
    {
        public static DiscordMessageBuilder NowPlaying(DiscordClient client, DiscordUser user, LavalinkTrack track)
        {
            var playEmbed = new DiscordEmbedBuilder
            {
                Title = "Now playing",
                Description = $"[{track.Title}]({track.Uri})",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = user.AvatarUrl,
                    Text = "Ordered by " + user.Username
                },
                Color = Bot.MainEmbedColor
            };
            var playButton = new DiscordButtonComponent(ButtonStyle.Secondary, "play_button", $"Play", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":arrow_forward:")));
            var pauseButton = new DiscordButtonComponent(ButtonStyle.Secondary, "pause_button", $"Pause", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":pause_button:")));
            var nextButton = new DiscordButtonComponent(ButtonStyle.Secondary, "next_button", $"Next", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":track_next:")));
            var queueButton = new DiscordButtonComponent(ButtonStyle.Secondary, "queue_button", $"Queue", false, new DiscordComponentEmoji(DiscordEmoji.FromName(client,":page_facing_up:")));
            
            return new DiscordMessageBuilder()
                .AddEmbed(playEmbed)
                .AddComponents(pauseButton, playButton, nextButton, queueButton);
        }
    }
}