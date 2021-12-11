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
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;

namespace ducker
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        private LiveStreamMonitorService Monitor;
        private TwitchAPI API;

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
            API = new TwitchAPI();
            API.Settings.ClientId = "2zij38j2vmug5ictoalp9nxttl7s9w";
            API.Settings.Secret = "av494afabwc27lb6yv49qdqkii3jkb";
            API.Settings.AccessToken = "19tc6p6bpqt1teiypn9a17r7ts484a";  
            
            Monitor = new LiveStreamMonitorService(API, 10);
            List<string> idList = new List<string>{ "itakash1", "jus1d", "cheatbanned", "pate1k", "bratishkinoff", "evelone192" };
            Monitor.SetChannelsById(idList);
            Monitor.Start();
            Monitor.OnStreamOnline += OnStreamOnline;
            Monitor.OnStreamOffline += OnStreamOffline;
            
            // Monitor.Start();
            
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
            //Monitor.Start();
            await Task.Delay(-1);
        }

        public async void OnStreamOffline(object? sender, OnStreamOfflineArgs e)
        {
            Console.WriteLine($"stream offline");
            var guild = await Client.GetGuildAsync(696496218934608004);
            await guild.GetChannel(CmdChannelId).SendMessageAsync($"stream offline! title: {e.Stream.Title} channel: {e.Channel}");
        }

        public async void OnStreamOnline(object? sender, OnStreamOnlineArgs e)
        {
            Console.WriteLine($"stream online! title: {e.Stream.Title} channel: {e.Channel}");
            var guild = await Client.GetGuildAsync(696496218934608004);
            await guild.GetChannel(CmdChannelId).SendMessageAsync($"stream online! title: {e.Stream.Title} channel: {e.Channel}");
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