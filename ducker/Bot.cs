using System;
using System.Collections.Generic;
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

namespace ducker
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public static string RespondEmojiName = ":tick:";
        public static DiscordColor MainEmbedColor = new DiscordColor("#9b73ff");
        public static DiscordColor IncorrectEmbedColor = new DiscordColor("#ff0000");
        public static DiscordColor WarningColor = new DiscordColor("#ff9f30");
        public static readonly ulong Id = ConfigJson.GetConfigField().Id;
        public static readonly ulong MusicChannelId = ConfigJson.GetConfigField().MusicChannelId;
        public static readonly ulong ServerLogsChannelId = ConfigJson.GetConfigField().ServerLogsChannelId;
        public static readonly ulong CmdChannelId = ConfigJson.GetConfigField().CmdChannelId;
        public static readonly string InviteLink = "https://discord.com/api/oauth2/authorize?client_id=918248095869968434&permissions=8&scope=bot%20applications.commands";

        public static List<LavalinkTrack> Queue = new List<LavalinkTrack>();

        public async Task RunAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = ConfigJson.GetConfigField().Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                LogTimestampFormat = "dd.MM.yyyy - hh:mm:ss tt",
                Intents = DiscordIntents.All
            };
            
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = PollBehaviour.DeleteEmojis,
                Timeout = TimeSpan.FromHours(12)
            });

            Client.ComponentInteractionCreated += EventHandler.OnComponentInteractionCreated;
            Client.GuildMemberAdded += EventHandler.OnMemberAdded;
            Client.MessageCreated += EventHandler.OnMessageCreated;
            Client.GuildMemberRemoved += EventHandler.OnMemberRemoved;
            Client.MessageReactionAdded += EventHandler.OnReactionAdded;
            Client.MessageReactionRemoved += EventHandler.OnReactionRemoved;

            var commandsConfig = new CommandsNextConfiguration 
            {
                StringPrefixes = new string[] { ConfigJson.GetConfigField().Prefix },
                EnableDms = true,
                EnableMentionPrefix = true,
                EnableDefaultHelp = true
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
            Commands.RegisterCommands<MusicCommands>();
            Commands.SetHelpFormatter<DefaultHelpFormatter>();
            slash.RegisterCommands<SlashCommands>(696496218934608004);
            await Client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            (await lavalink.ConnectAsync(lavalinkConfig)).PlaybackFinished += EventHandler.OnPlaybackFinished;
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