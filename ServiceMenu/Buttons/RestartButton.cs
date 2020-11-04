using System.Collections.Generic;
using System.Threading.Tasks;
using TCAdminNexus.Helpers;
using TCAdminNexus.Modules;
using TCAdminNexus.Objects;

namespace TCAdminNexus.ServiceMenu.Buttons
{
    public class RestartButton : ServiceMenuModule
    {
        public override void DefaultSettings()
        {
            Name = "Restart Button";
            var attribute =
                new ActionCommandAttribute("Restart", "Restart Server", ":arrows_counterclockwise:",
                    new List<string> {"StartStop"});
            Settings.ViewOrder = 2;
            Settings.ActionCommandAttribute = attribute;

            Configuration.SetConfiguration(Settings);
        }

        public override async System.Threading.Tasks.Task DoAction()
        {
            await base.DoAction();
            var service = Authentication.Service;
            service.Restart("Restarted by Nexus.");
            var embed = EmbedTemplates.CreateSuccessEmbed($"{service.NameNoHtml}", "**Restarted successfully**");
            await CommandContext.RespondAsync(embed: embed);
        }
    }
}