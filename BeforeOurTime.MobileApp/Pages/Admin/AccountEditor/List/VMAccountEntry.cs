using BeforeOurTime.Models.Modules.Account.Models.Data;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using BeforeOurTime.Models.Modules.World.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Admin.AccountEditor.List
{
    /// <summary>
    /// Account wrapper to be used in a list 
    /// </summary>
    public class VMAccountEntry : BotViewModel, System.ComponentModel.INotifyPropertyChanged
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
        /// Unique id of account (transformed into a string)
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
        public AccountData AccountItem
        {
            get { return _accountItem; }
            set { _accountItem = value; NotifyPropertyChanged("AccountItem"); }
        }
        private AccountData _accountItem { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountData"></param>
        public VMAccountEntry(AccountData accountData)
        {
            IsSelected = false;
            AccountItem = accountData;
            Id = AccountItem.Id.ToString();
        }
    }
}
