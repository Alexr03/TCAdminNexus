using System;
using DSharpPlus;
using Microsoft.Extensions.Logging;
using TCAdmin.SDK;

namespace TCAdminNexus.Logger
{
    public class Logger : ILogger<BaseDiscordClient>
    {
        private static readonly object Lock = new object();

        public Logger(LogLevel minimumLevel = LogLevel.Debug, string timestampFormat = "yyyy-MM-dd HH:mm:ss zzz")
        {
            MinimumLevel = minimumLevel;
            TimestampFormat = timestampFormat;
        }

        private LogLevel MinimumLevel { get; }
        private string TimestampFormat { get; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
            Func<TState, Exception, string> formatter)
        {
            lock (Lock)
            {
                if (logLevel < MinimumLevel) return;
                var ename = eventId.Name;
                ename = ename?.Length > 12 ? ename?.Substring(0, 12) : ename;
                Console.Write($"[{DateTimeOffset.Now.ToString(TimestampFormat)}] [{eventId.Id,-4}/{ename,-12}] ");

                switch (logLevel)
                {
                    case LogLevel.Trace:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case LogLevel.Debug:
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        break;

                    case LogLevel.Information:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        break;

                    case LogLevel.Warning:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    case LogLevel.Error:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;

                    case LogLevel.Critical:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                }

                switch (logLevel)
                {
                    case LogLevel.Trace:
                        Console.WriteLine("[Trace] ");
                        break;
                    case LogLevel.Debug:
                        Console.WriteLine("[Debug] ");
                        break;
                    case LogLevel.Information:
                        Console.WriteLine("[Info] ");
                        break;
                    case LogLevel.Warning:
                        Console.WriteLine("[Warn] ");
                        break;
                    case LogLevel.Error:
                        Console.WriteLine("[Error] ");
                        break;
                    case LogLevel.Critical:
                        Console.WriteLine("[Critical] ");
                        break;
                    case LogLevel.None:
                        Console.WriteLine("[???] ");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }

                Console.ResetColor();

                var message = formatter(state, exception);
                LogManager.WriteToLog("Nexus", message, true, "Nexus");
                if (exception != null)
                {
                    LogManager.WriteToLog("Nexus", exception.Message, true, "Nexus-Errors");
                    LogManager.WriteToLog("Nexus", exception.StackTrace, true, "Nexus-Errors");
                    LogManager.WriteError(exception);
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= MinimumLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}