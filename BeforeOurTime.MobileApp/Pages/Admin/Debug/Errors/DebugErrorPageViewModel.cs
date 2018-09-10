using Autofac;
using BeforeOurTime.MobileApp.Services.Loggers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Admin.Debug.Errors
{
    public class DebugErrorPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Record errors and information during program execution
        /// </summary>
        private ILoggerService Logger { set; get; }
        /// <summary>
        /// List of logs in simple string form
        /// </summary>
        public List<string> Logs
        {
            get { return _logs; }
            set { _logs = value; NotifyPropertyChanged("Logs"); }
        }
        private List<string> _logs { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public DebugErrorPageViewModel(IContainer container) : base(container)
        {
            Logger = container.Resolve<ILoggerService>();
        }
        /// <summary>
        /// Load all logs from logging service
        /// </summary>
        /// <returns></returns>
        public async Task LoadLogs()
        {
            Working = true;
            try
            {
                Logs = await Logger.GetLogs();
            }
            finally
            {
                Working = false;
            }
        }
    }
}
