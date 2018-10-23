using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Account.Login
{
    public class AccountLoginPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        private IAccountService AccountService { set; get; }
        /// <summary>
        /// List of all accounts
        /// </summary>
        public List<AccountListEntryVM> Accounts
        {
            get { return _accounts; }
            set { _accounts = value; NotifyPropertyChanged("Accounts"); }
        }
        private List<AccountListEntryVM> _accounts { set; get; } = new List<AccountListEntryVM>();
        /// <summary>
        /// Account currently logged in
        /// </summary>
        public BeforeOurTime.Models.Modules.Account.Models.Account LoggedInAccount
        {
            get { return _loggedInAccount; }
            set { _loggedInAccount = value; NotifyPropertyChanged("LoggedInAccount"); }
        }
        private BeforeOurTime.Models.Modules.Account.Models.Account _loggedInAccount { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public AccountLoginPageViewModel(IContainer container) : base(container)
        {
            AccountService = Container.Resolve<IAccountService>();
        }
        /// <summary>
        /// Get all cached accounts
        /// </summary>
        /// <param name="force">Force update from server</param>
        /// <returns></returns>
        public async Task GetAccounts(bool force = false)
        {
            await Task.Run(() =>
            {
                LoggedInAccount = AccountService.GetAccount();
                Accounts = new List<AccountListEntryVM>();
                Accounts.Add(new AccountListEntryVM()
                {
                    IsSelected = false,
                    Account = LoggedInAccount
                });
                Accounts = Accounts.ToList();
            });
        }
    }
}
