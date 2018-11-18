using Autofac;
using BeforeOurTime.MobileApp.Services.Characters;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Account.Character.Update
{
    public class VMUpdateCharacterPage : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        private ICharacterService CharacterService { set; get; }
        /// <summary>
        /// View model to update character
        /// </summary>
        public VMUpdateCharacter VMUpdateCharacter
        {
            get { return _vmUpdateCharacter; }
            set { _vmUpdateCharacter = value; NotifyPropertyChanged("VMUpdateCharacter"); }
        }
        private VMUpdateCharacter _vmUpdateCharacter { set; get; }
        /// <summary>
        /// Character that will be updated
        /// </summary>
        public Item Character
        {
            get { return _character; }
            set { _character = value; NotifyPropertyChanged("Character"); }
        }
        private Item _character { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="character">Character that will be updated</param>
        public VMUpdateCharacterPage(
            IContainer container, 
            Item character) : base(container)
        {
            VMUpdateCharacter = new VMUpdateCharacter(container, character);
            CharacterService = container.Resolve<ICharacterService>();
            Character = character;
        }
        /// <summary>
        /// Register for permanent character
        /// </summary>
        public async Task Register()
        {
            await VMUpdateCharacter.Register();
        }
        /// <summary>
        /// Cleanup before closing
        /// </summary>
        public void Cancel()
        {
        }
    }
}
