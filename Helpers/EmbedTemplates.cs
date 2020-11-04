using System;
using Alexr03.Common.Configuration;
using DSharpPlus.Entities;
using TCAdminNexus.Configurations.EmbedTemplates;

namespace TCAdminNexus.Helpers
{
    public static class EmbedTemplates
    {
        public static DiscordEmbedBuilder CreateSuccessEmbed(string title = "Success",
            string description = "The task executed successfully", bool showTimestamp = true)
        {
            var config = new LocalConfiguration<SuccessEmbedSettings>("SuccessEmbedConfig").GetConfiguration();

            var embed = new DiscordEmbedBuilder(config.EmbedBuilder) {Title = title, Description = description};

            if (showTimestamp) embed.WithTimestamp(DateTime.Now);

            return embed;
        }

        public static DiscordEmbedBuilder CreateErrorEmbed(string title = "Error",
            string description = "The task errored", bool showTimestamp = true)
        {
            var config = new LocalConfiguration<ErrorEmbedSettings>("ErrorEmbedConfig").GetConfiguration();
            var embed = new DiscordEmbedBuilder(config.EmbedBuilder) {Title = title, Description = description};

            if (showTimestamp) embed.WithTimestamp(DateTime.Now);

            return embed;
        }

        public static DiscordEmbedBuilder CreateInfoEmbed(string title,
            string description, bool showTimestamp = true)
        {
            var config = new LocalConfiguration<InfoEmbedSettings>("InfoEmbedConfig").GetConfiguration();

            var embed = new DiscordEmbedBuilder(config.EmbedBuilder) {Title = title, Description = description};

            if (showTimestamp) embed.WithTimestamp(DateTime.Now);

            return embed;
        }
    }
}