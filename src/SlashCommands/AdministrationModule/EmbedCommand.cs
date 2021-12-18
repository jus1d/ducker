using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using ducker.Attributes;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("embed", "Sends to current channel embed with your title, description and other settings"),
         RequireAdmin]
        public async Task EmbedCommand(InteractionContext msg,
            [Option("description", "Set description tp your embed")] string description = null,
            [Option("title", "Set title for your embed")] string title = null,
            [Option("color", "Set color to your embed")] string colorHexCode = null,
            [Option("image", "Add image to your embed")] string imageUrl = null,
            [Option("titleURL", "Set title clickable to your URL")] string titleUrl = null)
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
            // TODO: check it!
        }
    }
}