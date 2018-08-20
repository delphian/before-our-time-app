using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Loggers
{
    /// <summary>
    /// Record errors and information during program execution
    /// </summary>
    public interface ILoggerService : Microsoft.Extensions.Logging.ILogger
    {
        /// <summary>
        /// Get all logs in simple string form
        /// </summary>
        /// <returns>List of logs strings</returns>
        Task<List<string>> GetLogs();
    }
}
