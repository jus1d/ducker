using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.Net.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using DSharpPlus.SlashCommands;

namespace duckerBot
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
            client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = PollBehaviour.DeleteEmojis,
                Timeout = TimeSpan.FromMinutes(3)
            });

            client.MessageCreated += EventHandler.OnMessageCreated;
            client.GuildMemberAdded += EventHandler.OnMemberAdded;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = true,
                EnableMentionPrefix = true,
                EnableDefaultHelp = false
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
                Name = "with ducks | -help"
            };
            client.UpdateStatusAsync(activity);
            return Task.CompletedTask;
        }
    }
}