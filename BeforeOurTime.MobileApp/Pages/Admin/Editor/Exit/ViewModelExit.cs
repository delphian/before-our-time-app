using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Exit
{
    /// <summary>
    /// View model for exit item
    /// </summary>
    public class ViewModelExit : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Unique item identifier
        /// </summary>
        public string ItemId
        {
            get { return _itemId; }
            set { _itemId = value; NotifyPropertyChanged("ItemId"); }
        }
        private string _itemId { set; get; }
        /// <summary>
        /// Unique exit identifier
        /// </summary>
        public string ExitId
        {
            get { return _exitId; }
            set { _exitId = value; NotifyPropertyChanged("ExitId"); }
        }
        private string _exitId { set; get; }
        /// <summary>
        /// Name of exit
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        private string _name { set; get; }
        /// <summary>
        /// Description of exit
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged("Description"); }
        }
        private string _description { set; get; }
    }
}
