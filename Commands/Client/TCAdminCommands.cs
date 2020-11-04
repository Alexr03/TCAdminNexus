using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminNexus.Attributes;
using TCAdminNexus.Objects.Emulators;
using TCAdminNexus.ServiceMenu.Buttons;

namespace TCAdminNexus.Commands.Client
{
    [Group("tca")]
    [Aliases("service")]
    [Cooldown(1, 5.0, CooldownBucketType.User)]
    public class TcAdminCommands : BaseCommandModule
    {
        [GroupCommand]
        public async System.Threading.Tasks.Task MainCommand(CommandContext ctx)
        {
            var context = new CommandAttributes.RequireAuthentication();

            await context.ExecuteCheckAsync(ctx, false);

            await new TcaServiceMenu().MenuEmulation(ctx, context);
        }
        
        [GroupCommand]
        public async System.Threading.Tasks.Task MainCommand(CommandContext ctx, string command)
        {
            await ctx.TriggerTypingAsync();
            var context = new CommandAttributes.RequireAuthentication();
            await context.ExecuteCheckAsync(ctx, false);

            switch (command.ToLower())
            {
                case "start":
                    await new StartButton()
                    {
                        Authentication = context,
                        CommandContext = ctx
                    }.DoAction();
                    break;
                case "restart":
                    await new RestartButton()
                    {
                        Authentication = context,
                        CommandContext = ctx
                    }.DoAction();
                    break;
                case "stop":
                    await new StopButton()
                    {
                        Authentication = context,
                        CommandContext = ctx
                    }.DoAction();
                    break;
            }
        }

        [Command("RCon")]
        public async System.Threading.Tasks.Task RemoteConsoleCommand(CommandContext ctx, [RemainingText] string command)
        {
            var context = new CommandAttributes.RequireAuthentication();

            await context.ExecuteCheckAsync(ctx, false);

            var remoteConsole = new RemoteConsoleButton
            {
                Authentication = context,
                CommandContext = ctx
            };

            await remoteConsole.RconTask(command);
        }
    }
}