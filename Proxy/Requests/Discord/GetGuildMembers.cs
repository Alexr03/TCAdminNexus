using System.Linq;
using Alexr03.Common.TCAdmin.Proxy.Requests;
using Newtonsoft.Json;
using TCAdminNexus.Models;

namespace TCAdminNexus.Proxy.Requests.Discord
{
    public class GetGuildMembers : ProxyRequest
    {
        public override string CommandName => "GetGuildMembers";

        public override object Execute(object arguments)
        {
            var guildId = ulong.Parse(arguments.ToString());
            var discordGuild = DiscordBot.Client.GetGuildAsync(guildId).Result;
            var members = discordGuild.Members;
            return JsonConvert.SerializeObject(members.Select(x => DiscordMemberModel.Map(x.Value)),
                Alexr03.Common.Utilities.NoErrorJsonSettings);
        }
    }
}