using Alexr03.Common.TCAdmin.Proxy.Requests;
using Newtonsoft.Json;

namespace TCAdminNexus.Proxy.Requests.Discord
{
    public class LeaveGuild : ProxyRequest
    {
        public override string CommandName { get; } = "LeaveGuild";

        public override object Execute(object arguments)
        {
            var guild = DiscordBot.Client.GetGuildAsync(ulong.Parse(arguments.ToString())).Result;
            guild.LeaveAsync();

            return JsonConvert.SerializeObject(true);
        }
    }
}