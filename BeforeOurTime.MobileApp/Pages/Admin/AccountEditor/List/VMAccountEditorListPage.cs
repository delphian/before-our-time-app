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
    public class VMAccountEditorListPage : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Structure that subscriber must implement to recieve property updates
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Notify all subscribers that a property has been updated
        /// </summary>
        /// <param name="propertyName">Name of public property that has changed</param>
        protected void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Depedency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// Indicate if network request is still pending with service
        /// </summary>
        public bool Working
        {
            get { return _working; }
            set { _working = value; NotifyPropertyChanged("Working"); }
        }
        protected bool _working { set; get; } = false;
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
        public VMAccountEditorListPage(IContainer container)
        {
            Container = container;
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
        /// <summary>
        /// Delete currently selected account
        /// </summary>
        /// <returns></returns>
        public async Task DeleteSelectedAccount()
        {
            var accountEntry = AccountList.Where(x => x.IsSelected).FirstOrDefault();
            if (accountEntry != null)
            {
                await AccountService.DeleteAsync(accountEntry.AccountItem.Id);
                AccountList.Remove(accountEntry);
                AccountList.ToList();
            }
        }
    }
}
