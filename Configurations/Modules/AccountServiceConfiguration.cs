using Alexr03.Common.Configuration;

namespace TCAdminNexus.Configurations.Modules
{
    public class AccountServiceConfiguration : LocalConfiguration<AccountServiceConfiguration>
    {
        public AccountServiceConfiguration() : base("AccountServiceConfiguration")
        {
        }

        public LoginConfiguration LoginConfiguration { get; set; } = new LoginConfiguration();
    }
}