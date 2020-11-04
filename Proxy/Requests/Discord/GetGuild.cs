using Alexr03.Common.TCAdmin.Proxy.Requests;
using Newtonsoft.Json;

namespace TCAdminNexus.Proxy.Requests.Discord
{
    public class GetGuild : ProxyRequest
    {
        public override string CommandName => "GetGuild";
        
        public override object Execute(object arguments)
        {
            var guildId = ulong.Parse(arguments.ToString());
            var discordGuild = DiscordBot.Client.GetGuildAsync(guildId).Result;
            return JsonConvert.SerializeObject(discordGuild,
                Alexr03.Common.Utilities.NoErrorJsonSettings);
        }
    }
}