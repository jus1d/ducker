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
using ducker.Events;
using ducker.SlashCommands.AdministrationModule;
using ducker.SlashCommands.MiscModule;
using ducker.SlashCommands.MusicModule;

namespace ducker
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public static string RespondEmojiName = ":verify:";
        public static DiscordColor MainEmbedColor = new ("#9b73ff");
        public static DiscordColor IncorrectEmbedColor = new ("#ff0000");
        public static DiscordColor WarningColor = new ("#ff9f30");
        public static DiscordColor LogColor = new("#4D4D4D");
        public static readonly ulong Id = ConfigJson.GetConfigField().Id;
        public static readonly string InviteLink = "https://discord.com/api/oauth2/authorize?client_id=921896450915434537&permissions=8&scope=bot%20applications.commands";
        public static readonly ulong MainGuildId = 696496218934608004;

        public static List<LavalinkTrack> Queue = new ();

        public async Task RunAsync()
        {
            var config = new DiscordConfiguration
            {
                Token = ConfigJson.GetConfigField().Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
                LogTimestampFormat = "dd.MM.yyyy - hh:mm:ss",
                Intents = DiscordIntents.All
            };
            
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = PollBehaviour.DeleteEmojis,
                Timeout = TimeSpan.FromHours(1)
            });

            Client.ComponentInteractionCreated += Events.EventHandler.OnComponentInteractionCreated;
            Client.GuildMemberAdded += Events.EventHandler.OnMemberAdded;
            Client.MessageCreated += Events.EventHandler.OnMessageCreated;
            Client.GuildMemberRemoved += Events.EventHandler.OnMemberRemoved;
            Client.MessageUpdated += Events.EventHandler.OnMessageUpdated;
            Client.ChannelCreated += Events.EventHandler.OnChannelCreated;

            var commandsConfig = new CommandsNextConfiguration 
            {
                StringPrefixes = new [] { ConfigJson.GetConfigField().Prefix },
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
            slash.RegisterCommands<AdministrationSlashCommands>(696496218934608004);
            slash.RegisterCommands<MiscSlashCommands>(696496218934608004);
            slash.RegisterCommands<MusicSlashCommands>(696496218934608004);
            
            await Client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            (await lavalink.ConnectAsync(lavalinkConfig)).PlaybackFinished += Events.EventHandler.OnPlaybackFinished;
            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            Client.UpdateStatusAsync(new DiscordActivity
            {
                ActivityType = ActivityType.Playing,
                Name = "with ducks | -help"
            }, UserStatus.Idle);
            return Task.CompletedTask;
        }
    }
}