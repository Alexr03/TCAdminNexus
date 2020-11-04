using System.Collections.Generic;
using System.Linq;
using DSharpPlus;
using DSharpPlus.Entities;

namespace TCAdminNexus.Models
{
    public class DiscordGuildModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string WidgetImage { get; set; }
        public List<string> Features { get; set; }
        
        public PremiumTier PremiumTier { get; set; }
        
        public static DiscordGuildModel Map(DiscordGuild guild)
        {
            return new DiscordGuildModel
            {
                Id = guild.Id.ToString(),
                Name = guild.Name,
                Description = guild.Description,
                IconUrl = guild.IconUrl,
                WidgetImage = guild.GetWidgetImage(),
                Features = guild.Features.ToList(),
                PremiumTier = guild.PremiumTier
            };
        }
    }
}