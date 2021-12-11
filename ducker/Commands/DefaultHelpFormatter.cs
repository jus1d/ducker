using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace ducker;

public class DefaultHelpFormatter : BaseHelpFormatter
    {
        public DiscordEmbedBuilder EmbedBuilder { get; }
        private Command Command { get; set; }

        public DefaultHelpFormatter(CommandContext ctx)
            : base(ctx)
        {
            this.EmbedBuilder = new DiscordEmbedBuilder()
                .WithTitle("Help list")
                .WithColor(Bot.MainEmbedColor);
        }

        public override BaseHelpFormatter WithCommand(Command command)
        {
            this.Command = command;

            this.EmbedBuilder.WithDescription($"{Formatter.InlineCode(command.Name)}: {command.Description ?? "No description provided."}");

            if (command is CommandGroup cgroup && cgroup.IsExecutableWithoutSubcommands)
                this.EmbedBuilder.WithDescription($"{this.EmbedBuilder.Description}\n\nThis group can be executed as a standalone command.");

            if (command.Aliases?.Any() == true)
                this.EmbedBuilder.AddField("Aliases", string.Join(", ", command.Aliases.Select(Formatter.InlineCode)), false);

            return this;
        }

        public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
        {
            this.EmbedBuilder.AddField(this.Command != null ? "Subcommands" : "Commands", string.Join(", ", subcommands.Select(x => Formatter.InlineCode(x.Name))), false);

            return this;
        }

        public override CommandHelpMessage Build()
        {
            if (this.Command == null)
                this.EmbedBuilder.WithDescription($"List of all server commands." +
                                                  $"\nPrefix for this server: {ConfigJson.GetConfigField().Prefix}, but you can use slash commands(just type `/`)" +
                                                  $"\nUse `/help <command>` to see certain command description");

            return new CommandHelpMessage(embed: this.EmbedBuilder.Build());
        }
    }