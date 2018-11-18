using BeforeOurTime.Models.Modules.World.Models.Items;
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
        /// List item itself
        /// </summary>
        public CharacterItem CharacterItem
        {
            get { return _characterItem; }
            set { _characterItem = value; NotifyPropertyChanged("CharacterItem"); }
        }
        private CharacterItem _characterItem { set; get; }
    }
}
