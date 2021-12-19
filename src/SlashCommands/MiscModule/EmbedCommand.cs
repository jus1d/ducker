using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.MiscModule
{
    public partial class MiscSlashCommands
    {
        [SlashCommand("embed", "Sends to current channel embed with your title, description and other settings")]
        public async Task EmbedCommand(InteractionContext msg,
            [Option("description", "Set description tp your embed")] string description = "",
            [Option("title", "Set title for your embed")] string title = "",
            [Option("color", "Set color to your embed")] string colorHexCode = "9b73ff",
            [Option("image", "Add image to your embed")] string imageUrl = "",
            [Option("titleURL", "Set title clickable to your URL")] string titleUrl = "")
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            var color = new DiscordColor(colorHexCode);
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
                Url = titleUrl,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = color
            });
        }
    }
}