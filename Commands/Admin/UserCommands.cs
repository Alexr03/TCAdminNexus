﻿using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using TCAdmin.SDK.Objects;
using TCAdminNexus.Helpers;
using TCAdminNexus.Services;

namespace TCAdminNexus.Commands.Admin
{
    [Group("User")]
    public class UserCommands : BaseCommandModule
    {
        [Command("SetVariable")]
        public async System.Threading.Tasks.Task SetVariable(CommandContext ctx, int userId, string variableName, string variableValue)
        {
            await ctx.TriggerTypingAsync();
            var user = new User(userId);
            user.CustomFields[variableName] = variableValue;
            user.Save();

            await ctx.RespondAsync(embed: EmbedTemplates.CreateSuccessEmbed($"SetVariable for {user.UserName}",
                $"VariableName `{variableName}` value set to {variableValue}"));
        }

        [Command("SetVariable")]
        public async System.Threading.Tasks.Task SetVariable(CommandContext ctx, DiscordMember member, string variableName,
            string variableValue)
        {
            var user = AccountsService.GetUser(member.Id);
            if (user == null)
            {
                await ctx.RespondAsync(embed: EmbedTemplates.CreateErrorEmbed("SetVariable", "Could not find user."));
                return;
            }

            await SetVariable(ctx, user.UserId, variableName, variableValue);
        }

        [Command("GetVariable")]
        public async System.Threading.Tasks.Task GetVariable(CommandContext ctx, int userId, string variableName)
        {
            await ctx.TriggerTypingAsync();
            var user = new User(userId);
            var userCustomField = user.CustomFields[variableName];

            await ctx.RespondAsync(embed: EmbedTemplates.CreateSuccessEmbed($"GetVariable for {user.UserName}",
                $"VariableName `{variableName}` value is {userCustomField}"));
        }

        [Command("GetVariable")]
        public async System.Threading.Tasks.Task GetVariable(CommandContext ctx, DiscordMember member, string variableName)
        {
            var user = AccountsService.GetUser(member.Id);
            if (user == null)
            {
                await ctx.RespondAsync(embed: EmbedTemplates.CreateErrorEmbed("SetVariable", "Could not find user."));
                return;
            }

            await GetVariable(ctx, user.UserId, variableName);
        }
    }
}