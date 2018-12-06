using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Characters;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.World.ItemProperties.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Account.Character
{
    public class AccountCharacterPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        private IAccountService AccountService { set; get; }
        private ICharacterService CharacterService { set; get; }
        /// <summary>
        /// List of all characters owned by account
        /// </summary>
        public List<VMAccountCharacterEntry> Characters
        {
            get { return _characters; }
            set { _characters = value; NotifyPropertyChanged("Characters"); }
        }
        private List<VMAccountCharacterEntry> _characters { set; get; } = new List<VMAccountCharacterEntry>();
        /// <summary>
        /// Account character currently being played
        /// </summary>
        public Item PlayingCharacter
        {
            get { return _playingCharacter; }
            set { _playingCharacter = value; NotifyPropertyChanged("PlayingCharacter"); }
        }
        private Item _playingCharacter { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public AccountCharacterPageViewModel(IContainer container) : base(container)
        {
            AccountService = Container.Resolve<IAccountService>();
            CharacterService = Container.Resolve<ICharacterService>();
        }
        /// <summary>
        /// Get all characters for logged in account
        /// </summary>
        /// <param name="force">Force update from server</param>
        /// <returns></returns>
        public async Task GetAccountCharacters(bool force = false)
        {
            Working = true;
            try
            {
                Guid? accountId = AccountService.GetAccount()?.Id;
                if (accountId == null)
                {
                    throw new Exception("Not logged in");
                }
                Characters = new List<VMAccountCharacterEntry>();
                var results = await Container.Resolve<ICharacterService>()
                    .GetAccountCharactersAsync(accountId.Value, force);
                results?.ForEach(result =>
                {
                    Characters.Add(new VMAccountCharacterEntry()
                    {
                        IsSelected = false,
                        CharacterItem = result
                    });
                    Characters = Characters.ToList();
                });
            }
            finally
            {
                Working = false;
            }
        }
    }
}
