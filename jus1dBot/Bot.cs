using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace jus1dBot
{
    public class Bot
    {
        public DiscordClient client { get; private set; }
        
        public InteractivityExtension interactivity { get; private set; }
        
        public CommandsNextExtension commands { get; private set; }
        
        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            
            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };
            
            client = new DiscordClient(config);

            client.Ready += OnClientReady;
            client.UpdateStatusAsync();
            client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(2)
            });
            client.GuildMemberAdded += async (args, member) =>
            {
                await member.Member.SendMessageAsync($"Hello");
            };
            
            client.MessageCreated += async (args, msg ) =>
            {
                DiscordMember member = (DiscordMember)msg.Author;
                
                if (msg.Message.MentionEveryone)
                {
                    if (msg.Message.Author.IsBot) // ignore bots
                        return;
                    
                    if (member.IsOwner) // ignore owner
                        return;
                    
                    if (member.Id == 857687574281453598) // ignore itakashi
                        return;
                    
                    await msg.Message.DeleteAsync();
                    await msg.Message.Channel.SendMessageAsync($"не тегай");
                }
            };

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = true
            };
            
            var endpoint = new ConnectionEndpoint
            {
                Hostname = "127.0.0.1",
                Port = 2333
            };
            
            var lavalinkConfig = new LavalinkConfiguration
            {
                Password = "11111111",
                RestEndpoint = endpoint,
                SocketEndpoint = endpoint
            };
            
            var lavalink = client.UseLavalink();


            commands = client.UseCommandsNext(commandsConfig);
            commands.RegisterCommands<Commands>();

            await client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient c, ReadyEventArgs e)
        {
            var activity = new DiscordActivity
            {
                ActivityType = ActivityType.Playing,
                Name = "with ducks | -help",
            };
            client.UpdateStatusAsync(activity);
            return Task.CompletedTask;
        }
    }
}