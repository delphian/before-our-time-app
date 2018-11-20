using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Loggers;
using BeforeOurTime.Models.Modules.Account.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Admin.AccountEditor.List
{
    public class VMAccountEditorListPage : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Record errors and information during program execution
        /// </summary>
        private ILoggerService LoggerService { set; get; }
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        private IAccountService AccountService { set; get; }
        /// <summary>
        /// UI List of all accounts
        /// </summary>
        public List<VMAccountEntry> AccountList
        {
            get { return _accountList; }
            set { _accountList = value; NotifyPropertyChanged("AccountList"); }
        }
        private List<VMAccountEntry> _accountList { set; get; } = new List<VMAccountEntry>();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public VMAccountEditorListPage(IContainer container) : base(container)
        {
            LoggerService = container.Resolve<ILoggerService>();
            AccountService = container.Resolve<IAccountService>();
        }
        /// <summary>
        /// Load account data into view model properties
        /// </summary>
        /// <returns></returns>
        public async Task ReadAccounts()
        {
            var accounts = await AccountService.ReadAccountDataAsync();
            var updatedList = new List<VMAccountEntry>();
            accounts.ForEach(account =>
            {
                updatedList.Add(new VMAccountEntry(account));
            });
            // Force notify to fire
            AccountList = updatedList.ToList();
        }
    }
}
