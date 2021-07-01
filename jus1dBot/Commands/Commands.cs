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
        [Description("Returns tagged channel ID")]
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
        [Description("Returns current channel ID")]
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
        public async Task InviteLink(CommandContext msg)
        {
            var message = msg.Channel.SendMessageAsync($"Here your link, {msg.User.Mention}\n " +
                                                       $"https://discord.com/api/oauth2/authorize?client_id=849009875031687208&permissions=8&scope=bot");
        }

        // -writeme <text>
        [Command("writeme")]
        public async Task WriteMe(CommandContext msg, params string[] text)
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
        public async Task Random(CommandContext msg, int minValue, int maxValue)
        {
            var rnd = new Random();
            await msg.Channel.SendMessageAsync($"Random number: {rnd.Next(minValue, maxValue + 1)}");
        }
        
        // -dice <dices>
        [Command("dice")]
        public async Task Dice(CommandContext msg, int dices = 1)
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