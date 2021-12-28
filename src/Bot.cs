using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Lavalink;
using DSharpPlus.Net;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using ducker.Commands;
using ducker.Commands.MiscModule;
using ducker.Commands.AdministrationModule;
using ducker.Commands.MusicModule;
using ducker.Config;
using ducker.SlashCommands.AdministrationModule;
using ducker.SlashCommands.MiscModule;
using ducker.SlashCommands.MusicModule;
using EventHandler = ducker.Events.EventHandler;

namespace ducker
{
    public class Bot
    {
        public DiscordClient Client { get; private set; } = new (new DiscordConfiguration
        {
            Token = ConfigJson.GetConfigField().Token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Debug,
            LogTimestampFormat = "dd.MM.yyyy - hh:mm:ss",
            Intents = DiscordIntents.All
        });

        public CommandsNextExtension Commands { get; private set; }

        public static string RespondEmojiName = ":verify:";
        public static DiscordColor MainEmbedColor = new ("#9b73ff");
        public static DiscordColor IncorrectEmbedColor = new ("#ff0000");
        public static DiscordColor WarningColor = new ("#ff9f30");
        public static DiscordColor LogColor = new("#4D4D4D");
        public static readonly ulong Id = ConfigJson.GetConfigField().Id;
        public static readonly string InviteLink = "https://discord.com/api/oauth2/authorize?client_id=921896450915434537&permissions=8&scope=bot%20applications.commands";
        public static readonly ulong MainGuildId = 696496218934608004;
        public static readonly ulong DevGuildId = 906326660796801085;
        public static DateTime Uptime;

        public static List<LavalinkTrack> Queue = new ();

        public async Task RunAsync()
        {
            Client.Ready += OnClientReady;
            Client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = PollBehaviour.DeleteEmojis,
                Timeout = TimeSpan.FromHours(1)
            });

            Client.ComponentInteractionCreated += EventHandler.OnComponentInteractionCreated;
            Client.GuildMemberAdded += EventHandler.OnMemberAdded;
            Client.MessageCreated += EventHandler.OnMessageCreated;
            Client.GuildMemberRemoved += EventHandler.OnMemberRemoved;
            Client.MessageUpdated += EventHandler.OnMessageUpdated;
            Client.ChannelCreated += EventHandler.OnChannelCreated;

            var commandsConfig = new CommandsNextConfiguration 
            {
                StringPrefixes = new [] { ConfigJson.GetConfigField().Prefix, "." },
                EnableDms = true,
                EnableMentionPrefix = true,
                EnableDefaultHelp = true,
                IgnoreExtraArguments = true
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
            Commands.RegisterCommands<AdministrationCommands>();
            Commands.RegisterCommands<MiscCommands>();
            Commands.RegisterCommands<MusicCommands>();
            Commands.SetHelpFormatter<DefaultHelpFormatter>();
            slash.RegisterCommands<AdministrationSlashCommands>(MainGuildId);
            slash.RegisterCommands<AdministrationSlashCommands>(DevGuildId);
            slash.RegisterCommands<MiscSlashCommands>(MainGuildId);
            slash.RegisterCommands<MiscSlashCommands>(DevGuildId);
            slash.RegisterCommands<MusicSlashCommands>(MainGuildId);
            slash.RegisterCommands<MusicSlashCommands>(DevGuildId);

            await Client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            (await lavalink.ConnectAsync(lavalinkConfig)).PlaybackFinished += Events.EventHandler.OnPlaybackFinished;
            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            Uptime = DateTime.Now;
            Client.UpdateStatusAsync(new DiscordActivity
            {
                ActivityType = ActivityType.Playing,
                Name = "with ducks | -help"
            }, UserStatus.Idle);
            return Task.CompletedTask;
        }
    }
}