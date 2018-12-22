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
        /// Cast Account.Id as a string for display on the UI
        /// </summary>
        public string Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged("Id"); }
        }
        private string _id { set; get; }
        /// <summary>
        /// List item itself
        /// </summary>
        public BeforeOurTime.Models.Modules.Account.Models.Account Account
        {
            get { return _account; }
            set
            {
                _account = value;
                Id = _account.Id.ToString();
                NotifyPropertyChanged("AccountItem");
            }
        }
        private BeforeOurTime.Models.Modules.Account.Models.Account _account { set; get; }
    }
}
