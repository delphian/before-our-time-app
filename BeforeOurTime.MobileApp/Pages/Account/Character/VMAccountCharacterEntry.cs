using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Account.Character
{
    /// <summary>
    /// Account character wrapper to be used in a list 
    /// </summary>
    public class VMAccountCharacterEntry : BotViewModel, System.ComponentModel.INotifyPropertyChanged
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
        /// Name of character
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        private string _name { set; get; }
        /// <summary>
        /// Description of character
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged("Description"); }
        }
        private string _description { set; get; }
        /// <summary>
        /// List item itself
        /// </summary>
        public Item Item
        {
            get { return _item; }
            set
            {
                _item = value;
                Name = _item.GetProperty<VisibleItemProperty>()?.Name;
                Description = _item.GetProperty<VisibleItemProperty>()?.Description;
                NotifyPropertyChanged("Item");
            }
        }
        private Item _item { set; get; }
    }
}
