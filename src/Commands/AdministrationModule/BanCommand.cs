using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using ducker.Attributes;

namespace ducker.Commands.AdministrationModule
{
    public partial class AdministrationModule : BaseCommandModule
    {
        [Command("ban"),
         Description("Ban mentioned user in current server"),
         RequireAdmin]
        public async Task BanCommand(CommandContext msg, DiscordMember member, [RemainingText] string reason = "Reason does not given")
        {
            try
            {
                await member.Guild.BanMemberAsync(member, 0, reason);
                await msg.Message.CreateReactionAsync(DiscordEmoji.FromName(msg.Client, Bot.RespondEmojiName));
            }
            catch
            {
                var incorrectBanCommandEmbed = new DiscordEmbedBuilder
                {
                    Description = "You can't ban this user",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.IncorrectEmbedColor
                };
                await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
            }
        }

        [Command("ban")]
        public async Task BanCommand(CommandContext msg, [RemainingText] string text)
        {
            var incorrectBanCommandEmbed = new DiscordEmbedBuilder
            {
                Title = $"Missing argument",
                Description = $"**Usage:** `-ban <member>`",
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = msg.User.AvatarUrl,
                    Text = msg.User.Username
                },
                Color = Bot.IncorrectEmbedColor
            };
            await msg.Channel.SendMessageAsync(incorrectBanCommandEmbed);
        }
    }
}