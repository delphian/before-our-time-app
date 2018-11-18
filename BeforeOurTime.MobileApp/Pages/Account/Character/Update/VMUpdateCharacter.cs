using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Characters;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Exceptions;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using BeforeOurTime.Models.Modules.World.Models.Data;
using BeforeOurTime.Models.Modules.World.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Account.Character.Update
{
    /// <summary>
    /// Update information on a character
    /// </summary>
    public class VMUpdateCharacter : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        private ICharacterService CharacterService { set; get; }
        /// <summary>
        /// Character that will be updated
        /// </summary>
        private Item ItemCharacter { set; get; }
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
        /// Character is in evaluation mode
        /// </summary>
        public bool Temporary
        {
            get { return _temporary; }
            set { _temporary = value; NotifyPropertyChanged("Temporary"); }
        }
        private bool _temporary { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="account">Account that will be updated</param>
        public VMUpdateCharacter(
            IContainer container, 
            Item itemCharacter)
        {
            Container = container;
            ItemCharacter = itemCharacter;
            CharacterService = Container.Resolve<ICharacterService>();
            MapItemToProperties(ItemCharacter);
        }
        /// <summary>
        /// Map item to exposed view model properties
        /// </summary>
        /// <param name="item"></param>
        public void MapItemToProperties(Item item)
        {
            Name = item.GetProperty<VisibleProperty>().Name;
            Temporary = item.GetProperty<CharacterProperty>().Temporary;
        }
        /// <summary>
        /// Register for permanent character
        /// </summary>
        public async Task<Item> Register()
        {
            try
            {
                ItemCharacter = await CharacterService.RegisterCharacterAsync(ItemCharacter.Id, Name);
            }
            catch (Exception e)
            {
                MapItemToProperties(ItemCharacter);
                throw e;
            }
            return ItemCharacter;
        }
    }
}