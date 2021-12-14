using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule
    {
        [Command("kick"),
         Description("Kick mentioned user from current server"),
         RequireAdmin]
        public async Task KickCommand(CommandContext msg, DiscordMember member, [RemainingText] string reason = "Reason does not given")
        {
            try
            {
                await member.RemoveAsync(reason);
                await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            }
            catch
            {
                var incorrectKickEmbed = new DiscordEmbedBuilder
                {
                    Description = ":x: You can't kick this member",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectKickEmbed);
                throw;
            }
        }

        [Command("kick")]
        public async Task KickCommand(CommandContext msg, [RemainingText] string text)
        {
            await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** -kick <member> <reason>",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            });
        }
    }
}