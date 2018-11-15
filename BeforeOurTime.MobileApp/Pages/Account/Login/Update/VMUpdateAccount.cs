using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Exceptions;
using BeforeOurTime.Models.Modules.Account.Messages.UpdatePassword;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Account.Login.Update
{
    /// <summary>
    /// Update information on a registered account
    /// </summary>
    public class VMUpdateAccount : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        private IAccountService AccountService { set; get; }
        /// <summary>
        /// Account that will be updated
        /// </summary>
        private BeforeOurTime.Models.Modules.Account.Models.Account Account { set; get; }
        /// <summary>
        /// Existing password
        /// </summary>
        public string OldPassword
        {
            get { return _oldPassword; }
            set { _oldPassword = value; NotifyPropertyChanged("OldPassword"); }
        }
        private string _oldPassword { set; get; }
        /// <summary>
        /// New password
        /// </summary>
        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; NotifyPropertyChanged("NewPassword"); }
        }
        private string _newPassword { set; get; }
        /// <summary>
        /// New password confirmation
        /// </summary>
        public string NewPasswordConfirm
        {
            get { return _newPasswordConfirm; }
            set { _newPasswordConfirm = value; NotifyPropertyChanged("NewPasswordConfirm"); }
        }
        private string _newPasswordConfirm { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="account">Account that will be updated</param>
        public VMUpdateAccount(
            IContainer container, 
            BeforeOurTime.Models.Modules.Account.Models.Account account)
        {
            Container = container;
            Account = account;
            AccountService = Container.Resolve<IAccountService>();
        }
        /// <summary>
        /// Update the account's password
        /// </summary>
        /// <returns></returns>
        public async Task<BeforeOurTime.Models.Modules.Account.Models.Account> UpdatePasswordAsync()
        {
            if (NewPassword == null || NewPassword == "")
                throw new BeforeOurTimeException("New password may not be blank");
            if (NewPassword != NewPasswordConfirm)
                throw new BeforeOurTimeException("New passwords do not match");
            Account = await AccountService.UpdatePasswordAsync(Account.Id, OldPassword, NewPassword);
            return Account;
        }
    }
}