using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Location
{
    /// <summary>
    /// View model for item locations
    /// </summary>
    public class ViewModelLocation : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Unique location item identifier
        /// </summary>
        public string ItemId
        {
            get { return _itemId; }
            set { _itemId = value; NotifyPropertyChanged("ItemId"); }
        }
        private string _itemId { set; get; }
        /// <summary>
        /// Unique location identifier
        /// </summary>
        public string LocationId
        {
            get { return _locationId; }
            set { _locationId = value; NotifyPropertyChanged("LocationId"); }
        }
        private string _locationId { set; get; }
        /// <summary>
        /// Name of location
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        private string _name { set; get; }
        /// <summary>
        /// Description of location
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged("Description"); }
        }
        private string _description { set; get; }
    }
}
