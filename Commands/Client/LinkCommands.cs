using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using TCAdminNexus.Attributes;
using TCAdminNexus.Helpers;
using TCAdminNexus.Services;

namespace TCAdminNexus.Commands.Client
{
    public class LinkCommands : BaseCommandModule
    {
        [Command("Link")]
        [Aliases("Setup")]
        [Description("Link a game server")]
        [CommandAttributes.RequireAdministrator]
        [Cooldown(1, 30.00, CooldownBucketType.Guild)]
        public async System.Threading.Tasks.Task LinkServiceTask(CommandContext ctx)
        {
            var user = await AccountsService.GetUser(ctx);
            var service = await DiscordService.LinkService(ctx, user);

            await ctx.RespondAsync(embed: EmbedTemplates.CreateSuccessEmbed(service.Name, "Link Successful!"));
        }
    }
}