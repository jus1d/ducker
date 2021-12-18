using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.MiscModule
{
    public partial class MiscSlashCommands
    {
        [SlashCommand("random", "Send random value in your range from min to max value to current channel")]
        public async Task Random(InteractionContext msg, 
            [Option("min", "Minimal value in your range")] long minValue, 
            [Option("max", "Maximal value in your range")] long maxValue) 
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            var rnd = new Random();
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = "Random number",
                Description = $"Your random number is: **{rnd.Next((int)minValue, (int)maxValue + 1)}**",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.MainEmbedColor
            });
        }
    }
}