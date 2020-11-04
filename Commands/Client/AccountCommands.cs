using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using TCAdminNexus.Helpers;
using TCAdminNexus.Services;

namespace TCAdminNexus.Commands.Client
{
    [Group("account")]
    [Description("Account Commands.")]
    public class AccountCommands : BaseCommandModule
    {
        [Command("Who")]
        public async System.Threading.Tasks.Task Who(CommandContext ctx)
        {
            var user = AccountsService.GetUser(ctx.User.Id);
            await ctx.RespondAsync(
                embed: EmbedTemplates.CreateInfoEmbed("User Information", $"You are: **{user.UserName}**"));
        }
    }
}