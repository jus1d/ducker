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
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using TwitchCSharp;
using TwitchCSharp.Clients;

namespace duckerBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        
        public static DiscordColor MainEmbedColor = DiscordColor.Aquamarine;
        public static DiscordColor IncorrectEmbedColor = DiscordColor.Red;
        public static DiscordColor WarningColor = DiscordColor.Orange;
        public static ulong MusicChannelId = ConfigJson.GetConfigField().MusicChannelId;
        public static ulong ServerLogsChannelId = ConfigJson.GetConfigField().ServerLogsChannelId;
        public static ulong CmdChannelId = ConfigJson.GetConfigField().CmdChannelId;
        public static ulong ReactionRolesMessageId = ConfigJson.GetConfigField().ReactionRolesMessageId;
        
        public async Task RunAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = ConfigJson.GetConfigField().Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.All
            };
            
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = PollBehaviour.DeleteEmojis,
                Timeout = TimeSpan.FromHours(12)
            });

            Client.MessageCreated += EventHandler.OnMessageCreated;
            Client.GuildMemberAdded += EventHandler.OnMemberAdded;
            Client.GuildMemberRemoved += EventHandler.OnMemberRemoved;
            Client.MessageReactionAdded += EventHandler.OnReactionAdded;
            Client.MessageReactionRemoved += EventHandler.OnReactionRemoved;

            var commandsConfig = new CommandsNextConfiguration 
            {
                StringPrefixes = new string[] { ConfigJson.GetConfigField().Prefix },
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
            var lavalink = Client.UseLavalink();
            var slash = Client.UseSlashCommands();
            
            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<Commands>();
            slash.RegisterCommands<SlashCommands>(696496218934608004);
            await Client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient c, ReadyEventArgs e)
        {
            var activity = new DiscordActivity
            {
                ActivityType = ActivityType.Playing,
                Name = "with ducks |  -help"
            };
            Client.UpdateStatusAsync(activity);
            return Task.CompletedTask;
        }
    }
}