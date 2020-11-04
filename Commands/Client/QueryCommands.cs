using System;
using System.Linq;
using System.Threading.Tasks;
using Alexr03.Common.Configuration;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using TCAdmin.GameHosting.SDK.GameMonitor;
using TCAdmin.GameHosting.SDK.Objects;
using TCAdminNexus.Configurations;
using TCAdminNexus.Configurations.Modules;
using TCAdminNexus.Helpers;
using TCAdminNexus.Services;

namespace TCAdminNexus.Commands.Client
{
    public class QueryCommands : BaseCommandModule
    {
        [Command("Help")]
        [Description("Displays the help embed")]
        public System.Threading.Tasks.Task HelpTask(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder
                {
                    Title = "Commands",
                    Color = DiscordColor.Green,
                    Description = "Shows Basic Commands",
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = "https://img.icons8.com/plasticine/100/000000/help.png"
                    }
                }
                .AddField(";Players", "Show all players on the server.", true)
                .AddField(";Service", "Show the Service Interface.", true)
                .AddField(";Link", "Link a Service to Discord.", true);

            return ctx.RespondAsync(embed: embed);
        }

        [Command("logout")]
        [Aliases("signout")]
        public async System.Threading.Tasks.Task SignOut(CommandContext ctx)
        {
            var user = AccountsService.GetUser(ctx.User.Id);
            if (user == null)
            {
                await ctx.RespondAsync(embed: EmbedTemplates.CreateErrorEmbed("Logout",
                    "**You have to be signed in, in order to logout.**"));
                return;
            }

            AccountsService.LogoutUser(user, ctx.User.Id);
            await ctx.RespondAsync(embed: EmbedTemplates.CreateSuccessEmbed("Logout", "**You have been logged out**"));
        }

        [Command("login")]
        [Aliases("signin")]
        public async System.Threading.Tasks.Task Signin(CommandContext ctx)
        {
            if (AccountsService.IsUserAuthenticated(ctx.User.Id))
            {
                await ctx.RespondAsync(
                    embed: EmbedTemplates.CreateErrorEmbed(description: "You are already logged in!"));
                return;
            }

            var user = AccountsService.GetUser(ctx.User.Id);
            if (user == null)
            {
                await AccountsService.SetupAccount(ctx);
                return;
            }

            await ctx.RespondAsync(embed: EmbedTemplates.CreateInfoEmbed("Login", "**You are already logged in**"));
        }

        [Command("Players")]
        [Description("View Players on the server.")]
        [Cooldown(1, 15.0, CooldownBucketType.User)]
        public async System.Threading.Tasks.Task PlayersTask(CommandContext ctx)
        {
            var settings =
                new LocalConfiguration<PlayersMenuSettings>("PlayerMenuSettings").GetConfiguration();
            await ctx.TriggerTypingAsync();
            var service = await DiscordService.GetService(ctx);
            var server = new Server(service.ServerId);

            var query = ServerQuery.GetQueryResults(server, new TCAdmin.GameHosting.SDK.Objects.Game(service.GameId),
                service);

            if (query.NumPlayers == 0)
            {
                await ctx.RespondAsync(
                    embed: EmbedTemplates.CreateErrorEmbed(service.NameNoHtml, "**No players online**"));
                return;
            }

            if (query.NumPlayers != query.Players.Count)
            {
                await ctx.RespondAsync(embed: EmbedTemplates.CreateInfoEmbed(service.NameNoHtml,
                    $"There are **{query.NumPlayers}/{query.MaxPlayers}** online!"));
                return;
            }

            var embed = new DiscordEmbedBuilder
            {
                Title = $"{service.Name} | Players: {query.NumPlayers}/{query.MaxPlayers}",
                Color = new Optional<DiscordColor>(new DiscordColor(settings.HexColor)),
                Timestamp = DateTime.Now,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = settings.ThumbnailUrl
                }
            };

            foreach (var player in query.Players.OrderBy(x => x.Name))
                embed.Description += $":bust_in_silhouette: {player.Name}\n";

            await ctx.RespondAsync(embed: embed);
        }
    }
}