using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace ducker.SlashCommands.AdministrationModule
{
    public partial class AdministrationSlashCommands
    {
        [SlashCommand("activity", "Changes activity to the bot")]
        public async Task ActivityCommand(InteractionContext msg,
            [Choice("playing", "playing")] 
            [Choice("streaming", "streaming")] 
            [Option("type", "Type for activity")] string activityType)
        {
            await msg.CreateResponseAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            if (activityType == "playing")
            {
                await msg.Client.UpdateStatusAsync(new DiscordActivity
                {
                    ActivityType = ActivityType.Playing,
                    Name = "with ducks | -help"
                }, UserStatus.Idle);
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "Activity changed to playing type",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                });
            }
            else if (activityType == "streaming")
            {
                await msg.Client.UpdateStatusAsync(new DiscordActivity
                {
                    ActivityType = ActivityType.Streaming,
                    Name = "with ducks |  -help",
                    StreamUrl = "https://www.twitch.tv/itakash1"
                }, UserStatus.Idle);
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = "Activity changed to streaming type",
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
}