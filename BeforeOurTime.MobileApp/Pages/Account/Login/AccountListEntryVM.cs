using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Account.Login
{
    /// <summary>
    /// Account wrapper to be used in a list 
    /// </summary>
    public class AccountListEntryVM : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Is list item in XAML List View control currently selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; NotifyPropertyChanged("IsSelected"); }
        }
        private bool _isSelected { set; get; }
        /// <summary>
        /// List item itself
        /// </summary>
        public BeforeOurTime.Models.Modules.Account.Models.Account Account
        {
            get { return _account; }
            set { _account = value; NotifyPropertyChanged("AccountItem"); }
        }
        private BeforeOurTime.Models.Modules.Account.Models.Account _account { set; get; }
    }
}
