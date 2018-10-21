using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Characters;
using BeforeOurTime.Models.Modules.World.Models.Items;
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
        public List<AccountCharacterListEntryVM> Characters
        {
            get { return _characters; }
            set { _characters = value; NotifyPropertyChanged("Characters"); }
        }
        private List<AccountCharacterListEntryVM> _characters { set; get; } = new List<AccountCharacterListEntryVM>();
        /// <summary>
        /// Account character currently being played
        /// </summary>
        public CharacterItem PlayingCharacter
        {
            get { return _playingCharacter; }
            set { _playingCharacter = value; NotifyPropertyChanged("PlayingCharacter"); }
        }
        private CharacterItem _playingCharacter { set; get; }
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
                Guid? accountId = AccountService.GetAccount()?.AccountId;
                if (accountId == null)
                {
                    throw new Exception("Not logged in");
                }
                Characters = new List<AccountCharacterListEntryVM>();
                var results = await Container.Resolve<ICharacterService>()
                    .GetAccountCharactersAsync(accountId.Value, force);
                results?.ForEach(result =>
                {
                    Characters.Add(new AccountCharacterListEntryVM()
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
