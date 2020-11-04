using System;
using System.Linq;
using Alexr03.Common.TCAdmin.Proxy.Requests;

namespace TCAdminNexus.Proxy.Requests.Bot
{
    [Serializable]
    public class GetStatistics : ProxyRequest
    {
        public override string CommandName => "GetStatistics";

        public int GuildCount { get; set; }

        public int MemberCount { get; set; }

        public override object Execute(object arguments)
        {
            var bot = DiscordBot.Client;
            return new GetStatistics
            {
                GuildCount = bot.Guilds.Count,
                MemberCount = bot.Guilds.Sum(keyValuePair => keyValuePair.Value.MemberCount)
            };
        }
    }
}