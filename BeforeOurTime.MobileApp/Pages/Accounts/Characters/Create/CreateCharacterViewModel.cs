using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Characters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Accounts.Characters.Create
{
    public class CreateCharacterViewModel : System.ComponentModel.INotifyPropertyChanged
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
        /// Account related operations
        /// </summary>
        private IAccountService AccountService { set; get; }
        /// <summary>
        /// Character related operations
        /// </summary>
        private ICharacterService CharacterService { set; get; }
        private bool _loggedIn { set; get; } = false;
        /// <summary>
        /// Account is logged in and ready to retrieve character list
        /// </summary>
        public bool LoggedIn
        {
            get { return _loggedIn; }
            set { _loggedIn = value; NotifyPropertyChanged("LoggedIn"); }
        }
        private bool _svcWorking { set; get; } = false;
        /// <summary>
        /// Service is currently working
        /// </summary>
        public bool SvcWorking
        {
            get { return _svcWorking; }
            set { _svcWorking = value; NotifyPropertyChanged("SvcWorking"); }
        }
        private string _name { set; get; }
        /// <summary>
        /// Name of new character
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public CreateCharacterViewModel(IContainer container)
        {
            Container = container;
            AccountService = container.Resolve<IAccountService>();
            CharacterService = container.Resolve<ICharacterService>();
            LoggedIn = AccountService.IsLoggedIn();
            AccountService.OnStateChange += ((loginState) =>
            {
                LoggedIn = AccountService.IsLoggedIn();
            });
        }
        /// <summary>
        /// Create a new account character with values entered on form
        /// </summary>
        /// <returns></returns>
        public async Task CreateAccountCharacterAsync()
        {
            try
            {
                SvcWorking = true;
                await CharacterService.CreateAccountCharacterAsync(
                    AccountService.GetAccount().Id,
                    Name);
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
