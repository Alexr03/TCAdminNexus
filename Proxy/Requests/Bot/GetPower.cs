using System;
using Alexr03.Common.TCAdmin.Proxy.Requests;
using TCAdmin.Interfaces.Server;

namespace TCAdminNexus.Proxy.Requests.Bot
{
    [Serializable]
    public class GetPower : ProxyRequest
    {
        public override string CommandName => "GetPower";

        public ServiceStatus Status { get; set; } = ServiceStatus.Stopped;

        public override object Execute(object arguments)
        {
            var botPower = new GetPower {Status = TcAdminModule.Instance.Status};
            return botPower;
        }
    }
}