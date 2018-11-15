using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.Account.Messages.UpdateAccount;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Account.Login.Update
{
    public class UpdateLoginPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        private IAccountService AccountService { set; get; }
        /// <summary>
        /// View model to update an existing registered account
        /// </summary>
        public VMUpdateAccount VMUpdateAccount
        {
            get { return _vmUpdateAccount; }
            set { _vmUpdateAccount = value; NotifyPropertyChanged("VMUpdateAccount"); }
        }
        private VMUpdateAccount _vmUpdateAccount { set; get; }
        /// <summary>
        /// Account that will be updated
        /// </summary>
        public BeforeOurTime.Models.Modules.Account.Models.Account Account
        {
            get { return _account; }
            set { _account = value; NotifyPropertyChanged("Account"); }
        }
        private BeforeOurTime.Models.Modules.Account.Models.Account _account { set; get; }
        private BeforeOurTime.Models.Modules.Account.Models.Account _originalAccount { set; get; }
        /// <summary>
        /// Confirm change password value
        /// </summary>
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; NotifyPropertyChanged("ConfirmPassword"); }
        }
        private string _confirmPassword { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="account">Account that will be updated</param>
        public UpdateLoginPageViewModel(
            IContainer container, 
            BeforeOurTime.Models.Modules.Account.Models.Account account) : base(container)
        {
            VMUpdateAccount = new VMUpdateAccount(container, account);
            AccountService = container.Resolve<IAccountService>();
            Account = account;
            _originalAccount = JsonConvert.DeserializeObject<BeforeOurTime.Models.Modules.Account.Models.Account>(JsonConvert.SerializeObject(account));
        }
        /// <summary>
        /// Register for permanent account
        /// </summary>
        public async Task Register()
        {
            if (Account.Password != ConfirmPassword)
            {
                throw new Exception("Passwords do not match");
            }
            try
            {
                Account.Temporary = false;
                await AccountService.UpdateAsync(Account);
            }
            catch (Exception e)
            {
                Cancel();
                throw e;
            }
        }
        /// <summary>
        /// Update the account's password
        /// </summary>
        /// <returns></returns>
        public async Task<BeforeOurTime.Models.Modules.Account.Models.Account> UpdatePasswordAsync()
        {
            Account = await VMUpdateAccount.UpdatePasswordAsync();
            return Account;
        }
        /// <summary>
        /// Restore original values
        /// </summary>
        public void Cancel()
        {
            Account.Name = _originalAccount.Name;
            Account.Password = _originalAccount.Password;
        }
    }
}
