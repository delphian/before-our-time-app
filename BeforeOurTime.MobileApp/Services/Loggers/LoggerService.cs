using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Loggers
{
    /// <summary>
    /// Record errors and information during program execution
    /// </summary>
    public class LoggerService : ILoggerService
    {
        private List<string> Logs { set; get; } = new List<string>();
        private LogLevel LogLevel { set; get; }

        public LoggerService()
        {
            LogLevel = LogLevel.Information;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            bool enabled = false;
            switch (logLevel)
            {
                case LogLevel.Error:
                    enabled = true;
                    break;
            }
            return enabled;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel >= LogLevel)
            {
                Logs.Add(logLevel.ToString() + ": " + DateTime.Now.ToString() + ": " + state.ToString());
            }
        }
        /// <summary>
        /// Log simple error message with detailed exception history
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public void Log(string message, Exception exception)
        {
            var traverse = exception;
            while (traverse != null)
            {
                message += ": " + traverse.Message;
                traverse = traverse.InnerException;
            }
            Logs.Add(LogLevel.Error.ToString() + ": " + DateTime.Now.ToString() + ": " + message);
        }
        /// <summary>
        /// Get all logs in simple string form
        /// </summary>
        /// <returns>List of logs strings</returns>
        public async Task<List<string>> GetLogs()
        {
            var ts = new TaskCompletionSource<List<string>>();
            ts.SetResult(this.Logs);
            return await ts.Task;
        }
    }
}
