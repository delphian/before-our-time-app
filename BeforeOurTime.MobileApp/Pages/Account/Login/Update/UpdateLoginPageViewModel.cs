using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Account.Login.Update
{
    public class UpdateLoginPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Account that will be updated
        /// </summary>
        public BeforeOurTime.Models.Modules.Account.Models.Account Account
        {
            get { return _account; }
            set { _account = value; NotifyPropertyChanged("Account"); }
        }
        private BeforeOurTime.Models.Modules.Account.Models.Account _account { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="account">Account that will be updated</param>
        public UpdateLoginPageViewModel(
            IContainer container, 
            BeforeOurTime.Models.Modules.Account.Models.Account account) : base(container)
        {
            Account = account;
        }
    }
}
