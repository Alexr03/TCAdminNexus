using System.Collections.Generic;
using System.Threading.Tasks;
using TCAdminNexus.Helpers;
using TCAdminNexus.Modules;
using TCAdminNexus.Objects;

namespace TCAdminNexus.ServiceMenu.Buttons
{
    public class StartButton : ServiceMenuModule
    {
        public override void DefaultSettings()
        {
            Name = "Start Button";
            var attribute =
                new ActionCommandAttribute("Start", "Start Server", ":arrow_forward:",
                    new List<string> {"StartStop"});
            Settings.ViewOrder = 1;
            Settings.ActionCommandAttribute = attribute;

            Configuration.SetConfiguration(Settings);
        }

        public override async System.Threading.Tasks.Task DoAction()
        {
            await base.DoAction();
            var service = Authentication.Service;
            service.Start("Started by Nexus.");
            var embed = EmbedTemplates.CreateSuccessEmbed($"{service.NameNoHtml}", "**Started successfully**");
            await CommandContext.RespondAsync(embed: embed);
        }
    }
}