using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace ducker.Commands.Attributes
{
    /// <summary>
    /// Check whether the command is executed from hell server(main bot guild)
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequireMainGuild : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext msg, bool help)
        {
            if (msg.Guild.Id != Bot.MainGuildId)
            {
                await msg.Channel.SendMessageAsync(new DiscordEmbedBuilder
                {
                    Description = $"This command can be used only in `{(await msg.Client.GetGuildAsync(Bot.MainGuildId)).Name}` discord server",
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = msg.User.AvatarUrl,
                        Text = msg.User.Username
                    },
                    Color = Bot.MainEmbedColor
                });
            }
            return msg.Guild.Id == Bot.MainGuildId;
        }

    }
}