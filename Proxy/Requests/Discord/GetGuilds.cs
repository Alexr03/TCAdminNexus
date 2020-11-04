using System.Linq;
using Alexr03.Common.TCAdmin.Proxy.Requests;
using Newtonsoft.Json;

namespace TCAdminNexus.Proxy.Requests.Discord
{
    public class GetGuilds : ProxyRequest
    {
        public override string CommandName => "GetGuilds";

        public override object Execute(object arguments)
        {
            return JsonConvert.SerializeObject(DiscordBot.Client.Guilds.Select(x => x.Value),
                Alexr03.Common.Utilities.NoErrorJsonSettings);
        }
    }
}