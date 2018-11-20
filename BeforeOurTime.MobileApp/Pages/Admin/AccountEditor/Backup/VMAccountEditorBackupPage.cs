using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Loggers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Admin.AccountEditor.Backup
{
    public class VMAccountEditorBackupPage : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Record errors and information during program execution
        /// </summary>
        private ILoggerService LoggerService { set; get; }
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        private IAccountService AccountService { set; get; }
        public string JsonAccounts
        {
            get { return _jsonAccounts; }
            set { _jsonAccounts = value; NotifyPropertyChanged("JsonAccounts"); }
        } 
        private string _jsonAccounts { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public VMAccountEditorBackupPage(IContainer container) : base(container)
        {
            LoggerService = container.Resolve<ILoggerService>();
            AccountService = container.Resolve<IAccountService>();
        }
        /// <summary>
        /// Retrieve JSON backup of all items from server
        /// </summary>
        /// <returns></returns>
        public async Task LoadJsonAccounts()
        {
            var accountDatas = await AccountService.ReadAccountDataAsync();
            JsonAccounts = JsonConvert.SerializeObject(accountDatas, Formatting.Indented);
        }
        /// <summary>
        /// Copy JSON encoded item backup to system clipboard
        /// </summary>
        public void CopyToClipboard()
        {
            Plugin.Clipboard.CrossClipboard.Current.SetText(JsonAccounts);
        }
    }
}
