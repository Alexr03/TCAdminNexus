using System;
using System.Threading.Tasks;
using Alexr03.Common.TCAdmin.Configuration;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using Microsoft.Extensions.Logging;
using TCAdmin.SDK;
using TCAdmin.SDK.Mail;
using TCAdminNexus.Commands.Admin;
using TCAdminNexus.Commands.Client;
using TCAdminNexus.Configurations;
using TCAdminNexus.Exceptions;
using TCAdminNexus.Logger;

namespace TCAdminNexus
{
    public class DiscordBot : MarshalByRefObject
    {
        public static DiscordClient Client;

        public static readonly BotConfiguration NexusConfiguration =
            new DatabaseConfiguration<BotConfiguration>(Globals.ModuleId, nameof(BotConfiguration)).GetConfiguration();

        public DiscordBot()
        {
            var logger = Alexr03.Common.Logging.Logger.Create<DiscordBot>();
            if (string.IsNullOrEmpty(NexusConfiguration.Token))
            {
                logger.Fatal(
                    $"No token is set. Configure the token in the configuration tab @ https://{new CompanyInfo(2).ControlPanelUrl}/Nexus");
                return;
            }

            var discordConfig = new DiscordConfiguration
            {
                AutoReconnect = true,
                LargeThreshold = 250,
                MinimumLogLevel = LogLevel.Debug,
                Token = NexusConfiguration.Token,
                TokenType = TokenType.Bot,
                MessageCacheSize = 2048,
                LoggerFactory = new LoggingFactory()
            };

            Client = new DiscordClient(discordConfig);

            var commandsNextConfiguration = new CommandsNextConfiguration
            {
                StringPrefixes = NexusConfiguration.Prefixes,
                EnableDms = false,
                EnableMentionPrefix = true,
                CaseSensitive = false,
                EnableDefaultHelp = false
            };

            Client.UseInteractivity(
                new InteractivityConfiguration
                {
                    Timeout = TimeSpan.FromMinutes(2),
                    PaginationDeletion = PaginationDeletion.DeleteMessage,
                    PollBehaviour = PollBehaviour.DeleteEmojis
                });

            Client.UseCommandsNext(commandsNextConfiguration);
            var commandsNextService = Client.GetCommandsNext();

            // Admin Commands
            commandsNextService.RegisterCommands<NodeCommands>();
            commandsNextService.RegisterCommands<TaskManagerCommands>();
            commandsNextService.RegisterCommands<TcAdministrationCommands>();
            commandsNextService.RegisterCommands<UserCommands>();

            // User Commands
            commandsNextService.RegisterCommands<AccountCommands>();
            commandsNextService.RegisterCommands<LinkCommands>();
            commandsNextService.RegisterCommands<QueryCommands>();
            commandsNextService.RegisterCommands<TcAdminCommands>();

            commandsNextService.CommandExecuted += CommandExecutionEvent;
            commandsNextService.CommandErrored += CommandFailed;
        }

        public async System.Threading.Tasks.Task RunAsync()
        {
            var activity = new DiscordActivity
            {
                Name = "Nexus",
                ActivityType = ActivityType.Playing
            };
            await Client.ConnectAsync(activity);
            var botConfiguration =
                new DatabaseConfiguration<BotConfiguration>(Globals.ModuleId, nameof(BotConfiguration));
            var configuration = botConfiguration.GetConfiguration();
            configuration.ClientId = Client.CurrentApplication.Id;
            botConfiguration.SetConfiguration(configuration);

            await System.Threading.Tasks.Task.Delay(0);
        }

        public async System.Threading.Tasks.Task Stop()
        {
            await Client.DisconnectAsync();
        }

        public virtual DiscordBot GetDiscordBot()
        {
            return this;
        }

        private async Task<bool> HandleException(Exception e, CommandContext ctx)
        {
            switch (e)
            {
                case InvalidOperationException _:
                case CommandNotFoundException _:
                    return true;
                case TaskCanceledException _:
                    return true;
                case CustomMessageException customMessage:
                    customMessage.Context = ctx;

                    if (!string.IsNullOrEmpty(customMessage.Message)) await ctx.RespondAsync(customMessage.Message);

                    if (customMessage.Embed != null) await ctx.RespondAsync(embed: customMessage.Embed);

#pragma warning disable 4014
                    System.Threading.Tasks.Task.Run(async () => await customMessage.DoAction());
#pragma warning restore 4014

                    return customMessage.Handled;
                default:
                {
                    switch (e.Message)
                    {
                        case "Could not find a suitable overload for the command.":
                            return true;
                        case "No matching sub-commands were found, and this group is not executable.":
                            return true;
                    }

                    LogManager.WriteError(e);
                    return false;
                }
            }
        }

        private static System.Threading.Tasks.Task CommandExecutionEvent(CommandExecutionEventArgs e)
        {
            return System.Threading.Tasks.Task.Delay(0);
        }

        private async System.Threading.Tasks.Task CommandFailed(CommandErrorEventArgs e)
        {
            if (e.Exception?.Message != null && e.Exception.Message.Contains("403"))
            {
                await e.Context.RespondAsync(
                    "**This command required me to have the `Administrator` permission in your discord server. Please allow me this permission and try the command again!**");
                return;
            }

            var exceptionHandled = await HandleException(e.Exception, e.Context);
            if (exceptionHandled) return;

            if (e.Exception is ChecksFailedException cfe)
                foreach (var ex in cfe.FailedChecks)
                    switch (ex)
                    {
                        case CooldownAttribute cooldown:
                            await e.Context.RespondAsync(
                                $"Cooldown: **{cooldown.GetRemainingCooldown(e.Context).Seconds}s**");
                            return;
                    }
        }
    }
}