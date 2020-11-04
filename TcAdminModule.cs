using System;
using System.Linq;
using System.Reflection;
using Alexr03.Common.TCAdmin.Proxy;
using Alexr03.Common.TCAdmin.Proxy.Requests;
using TCAdmin.Interfaces.Logging;
using TCAdmin.Interfaces.Server;
using TCAdmin.SDK;
using TCAdmin.SDK.Proxies;

namespace TCAdminNexus
{
    public class TcAdminModule : IMonitorService
    {
        public static TcAdminModule Instance;
        public DiscordBot DiscordBot;

        public TcAdminModule()
        {
            ConfigurationKey = "TCAdminNexus";
        }

        public void Initialize(params object[] args)
        {
            DiscordBot = new DiscordBot();
            Instance = this;
        }

        public void Start()
        {
            Status = ServiceStatus.Starting;
            LogManager.Write("Starting Nexus...", LogType.Console);
            DiscordBot.RunAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            Status = ServiceStatus.Running;
            RegisterCommandProxies();
            LogManager.Write("Nexus has successfully started!", LogType.Console);
        }

        public void Stop()
        {
            Status = ServiceStatus.Stopping;
            LogManager.Write("Stopping Nexus...", LogType.Console);
            UnRegisterCommandProxies();
            DiscordBot.Stop().ConfigureAwait(false).GetAwaiter().GetResult();
            LogManager.Write("Nexus has successfully stopped!", LogType.Console);
            Status = ServiceStatus.Stopped;
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        private void RegisterCommandProxies()
        {
            var proxyRequests = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(ProxyRequest)) && !t.IsAbstract)
                .Select(t => (ProxyRequest) Activator.CreateInstance(t)).ToList();
            foreach (var request in proxyRequests)
            {
                new CommandProxy(request.Execute)
                {
                    CommandName = request.CommandName, KeepAlive = true, NeverDie = true
                }.RegisterProxy();
            }
        }

        private void UnRegisterCommandProxies()
        {
            ProxyManager.UnRegisterProxies();
        }

        public string ConfigurationKey { get; }

        public ServiceStatus Status { get; set; }
    }
}