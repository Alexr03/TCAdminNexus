﻿using DSharpPlus.Entities;

namespace TCAdminNexus.Configurations.EmbedTemplates
{
    public class EmbedTemplateSettings
    {
        public DiscordEmbed EmbedBuilder { get; set; } = new DiscordEmbedBuilder();
    }
}