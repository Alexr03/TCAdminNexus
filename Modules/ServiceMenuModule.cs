using System.Threading.Tasks;
using Alexr03.Common.Configuration;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using TCAdminNexus.Attributes;
using TCAdminNexus.Configurations;
using TCAdminNexus.Configurations.Modules;

namespace TCAdminNexus.Modules
{
    public class ServiceMenuModule : NexusModule
    {
        public readonly ServiceMenuActionSettings Settings = new ServiceMenuActionSettings();

        public ServiceMenuModule()
        {
            Configuration = new LocalConfiguration<ServiceMenuActionSettings>(GetType().Name);
            var config = Configuration.GetConfiguration();

            if (config != null)
            {
                Settings.ActionCommandAttribute = config.ActionCommandAttribute;
                Settings.ViewOrder = config.ViewOrder;
            }
            else
            {
                DefaultSettings();
            }
        }

        public CommandAttributes.RequireAuthentication Authentication { get; internal set; }

        public LocalConfiguration<ServiceMenuActionSettings> Configuration { get; }

        public DiscordMessage MenuMessage { get; set; }

        public CommandContext CommandContext { get; set; }

        public virtual void DefaultSettings()
        {
        }

        /// <summary>
        ///     This is fired when the user clicks on the emoji.
        /// </summary>
        public virtual async System.Threading.Tasks.Task DoAction()
        {
            if (Settings.ActionCommandAttribute.DeleteMenu) await MenuMessage.DeleteAsync();
        }
    }
}