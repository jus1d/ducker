using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.VisualBasic;

namespace jus1dBot
{
    public partial class Commands : BaseCommandModule
    {
        // -channelid
        [Command("channelid")]
        [Description("Send you tagged (or bot-commands) channel ID")]
        public async Task ChannelID(CommandContext msg, [Description(" optional channel (for voice channels with emoji - use template: **-channelid <#id>**)")] DiscordChannel channel = null)
        {
            if(msg.Channel.Name != "bot-commands")
                return;

            if (channel == null)
            {
                await msg.Channel.SendMessageAsync(msg.Channel.Id.ToString()).ConfigureAwait(false);
            }
            else
            {
                await msg.Channel.SendMessageAsync($"{channel.Mention} channel ID: {channel.Id}").ConfigureAwait(false);
            }
        }
        
        // -channelid <text>
        [Command("channelid")]
        [Description("Send you tagged (or bot-commands) channel ID")]
        
        public async Task ChannelID(CommandContext msg, [Description("if you misuse the command")] params string[] parametres)
        {
            if(msg.Channel.Name != "bot-commands")
                return;

            var templateEmbed = new DiscordEmbedBuilder
            {
                Title = "Template -channelid:",
                Description = "-channelid <channel>\n" +
                              "for voice channels with emoji - use template: **-channelid <#id>**",
                Color = DiscordColor.Azure
                
            };
            
            await msg.Channel.SendMessageAsync(templateEmbed).ConfigureAwait(false);
        }
        
        // -invitelink
        [Command("invitelink")]
        [Description("Send you bot's invite link")]
        public async Task InviteLink(CommandContext msg)
        {
            var message = msg.Channel.SendMessageAsync($"Here your link, {msg.User.Mention}\n " +
                                                       $"https://discord.com/api/oauth2/authorize?client_id=849009875031687208&permissions=8&scope=bot");
        }

        // -writeme <text>
        [Command("writeme")]
        [Description("Bot will type to you your text")]
        public async Task WriteMe(CommandContext msg, [Description("your text")] params string[] text)
        {
            string textForSend = "";
            
            for (int i = 0; i < text.Length; i++)
            {
                textForSend = textForSend + " " + text[i];
            }
            await msg.Member.SendMessageAsync(textForSend);
        }
        
        // -random <min> <max>
        [Command("random")]
        [Description("Send you randon value in your tange")]
        public async Task Random(CommandContext msg, [Description("minimal value")] int minValue, [Description("maximum value")]int maxValue)
        {
            var rnd = new Random();
            await msg.Channel.SendMessageAsync($"Random number: {rnd.Next(minValue, maxValue + 1)}");
        }
        
        // -dice <dices>
        [Command("dice")]
        [Description("Send you random value, possible on dices")]
        public async Task Dice(CommandContext msg, [Description("optinal dices amount")] int dices = 1)
        {
            int result = 0;

            int maxPossibleresult = dices * 6;
            
        }
        
        
        
        // -clearallchannels
        /*[Command("clearallchannels")]
        [RequireRoles(RoleCheckMode.All, "admin")]
        public async Task ClearAllChannels(CommandContext msg)
        {
            msg.Guild.DeleteAllChannelsAsync().ConfigureAwait(false);
        }*/
    }
}