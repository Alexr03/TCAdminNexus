using System.Collections.Generic;
using TCAdminNexus.Modules;
using TCAdminNexus.Objects;

namespace TCAdminNexus.ServiceMenu.Buttons
{
    public class ExitModule : ServiceMenuModule
    {
        public override void DefaultSettings()
        {
            Name = "Exit";
            var attribute =
                new ActionCommandAttribute("Exit", "Exit", ":octagonal_sign:",
                    new List<string> {string.Empty},
                    true);
            Settings.ViewOrder = 0;
            Settings.ActionCommandAttribute = attribute;

            Configuration.SetConfiguration(Settings);
        }
    }
}