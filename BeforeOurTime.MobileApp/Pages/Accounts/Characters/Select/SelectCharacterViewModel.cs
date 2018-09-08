using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Characters;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Items.Characters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Accounts.Characters.Select
{
    public class SelectCharacterViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Structure that subscriber must implement to recieve property updates
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// List of all characters owned by account
        /// </summary>
        public List<CharacterItem> Characters
        {
            get { return _characters; }
            set { _characters = value; NotifyPropertyChanged("Characters"); }
        }
        private List<CharacterItem> _characters { set; get; } = new List<CharacterItem>();
        /// <summary>
        /// Character list contains one or more characters
        /// </summary>
        public bool CharactersPresent
        {
            get { return _charactersPresent; }
            set { _charactersPresent = value; NotifyPropertyChanged("CharactersPresent"); }
        }
        private bool _charactersPresent { set; get; } = false;
        /// <summary>
        /// Account character currently being played
        /// </summary>
        public CharacterItem Character
        {
            get { return _character; }
            set { _character = value; NotifyPropertyChanged("Character"); }
        }
        private CharacterItem _character { set; get; }
        /// <summary>
        /// Play button title featuring selected character's name
        /// </summary>
        public string PlayButtonTitle
        {
            get { return _playButtonTitle; }
            set { _playButtonTitle = value; NotifyPropertyChanged("PlayButtonTitle"); }
        }
        private string _playButtonTitle { set; get; }
        /// <summary>
        /// Account is currently playing a character
        /// </summary>
        public bool Playing
        {
            get { return _playing; }
            set { _playing = value; NotifyPropertyChanged("Playing"); }
        }
        private bool _playing { set; get; } = false;
        /// <summary>
        /// Account is logged in and ready to retrieve character list
        /// </summary>
        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; NotifyPropertyChanged("LoggedIn"); }
        }
        private bool _loggedIn { set; get; } = false;
        /// <summary>
        /// Service is currently working
        /// </summary>
        public bool SvcWorking
        {
            get { return _svcWorking; }
            set { _svcWorking = value; NotifyPropertyChanged("SvcWorking"); }
        }
        private bool _svcWorking { set; get; } = false;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public SelectCharacterViewModel(IContainer container)
        {
            Container = container;
            var accountService = Container.Resolve<IAccountService>();
            var characterService = Container.Resolve<ICharacterService>();
            LoggedIn = accountService.IsLoggedIn();
            Playing = characterService.IsPlaying();
            accountService.OnStateChange += ((loginState) =>
            {
                LoggedIn = accountService.IsLoggedIn();
            });
            characterService.OnPlayStateChange += ((playState) =>
            {
                Playing = characterService.IsPlaying();
            });
        }
        /// <summary>
        /// Get all characters for logged in account
        /// </summary>
        /// <param name="force">Force update from server</param>
        /// <returns></returns>
        public async Task GetAccountCharacters(bool force = false)
        {
            SvcWorking = true;
            try
            {
                Guid? accountId = Container.Resolve<IAccountService>().GetAccount()?.AccountId;
                if (!LoggedIn || accountId == null)
                {
                    throw new Exception("Account not logged in");
                }
                Characters = await Container.Resolve<ICharacterService>()
                    .GetAccountCharactersAsync(accountId.Value, force);
                CharactersPresent = Characters.Count > 0;
            }
            finally
            {
                SvcWorking = false;
            }
        }
        /// <summary>
        /// Choose an account character to play
        /// </summary>
        /// <param name="accountCharacter">Account character item to play</param>
        /// <returns></returns>
        public async Task PlayAccountCharacter(CharacterItem accountCharacter)
        {
            try
            {
                SvcWorking = true;
                await Container.Resolve<ICharacterService>().PlayAccountCharacterAsync(accountCharacter);
                Character = Container.Resolve<ICharacterService>().GetCharacter();
                PlayButtonTitle = "Play " + Character.Visible.Name;
            }
            finally
            {
                SvcWorking = false;
            }
        }
        /// <summary>
        /// Notify all subscribers that a property has been updated
        /// </summary>
        /// <param name="propertyName">Name of public property that has changed</param>
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
