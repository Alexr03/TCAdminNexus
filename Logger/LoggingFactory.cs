using System.Collections.Generic;
using Alexr03.Common.Configuration;
using Microsoft.Extensions.Logging;
using TCAdminNexus.Configurations;

namespace TCAdminNexus.Logger
{
    public class LoggingFactory : ILoggerFactory
    {
        private bool _isDisposed;
        private List<ILoggerProvider> Providers { get; } = new List<ILoggerProvider>();

        public void AddProvider(ILoggerProvider provider)
        {
            Providers.Add(provider);
        }

        public ILogger CreateLogger(string categoryName)
        {
            var nexusConfiguration = new LocalConfiguration<BotConfiguration>().GetConfiguration();
            return new Logger(nexusConfiguration.MinimumLogLevel);
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;
            _isDisposed = true;

            foreach (var provider in Providers)
                provider.Dispose();

            Providers.Clear();
        }
    }
}