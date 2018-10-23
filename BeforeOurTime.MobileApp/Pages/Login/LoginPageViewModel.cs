using Autofac;
using BeforeOurTime.MobileApp.Models;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.WebSockets;
using BeforeOurTime.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Login
{
    /// <summary>
    /// View model for login page
    /// </summary>
    public class LoginPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Methods and events to communicate with websocket server
        /// </summary>
        private IWebSocketService WebSocketService { set; get; }
        /// <summary>
        /// Account management service
        /// </summary>
        private IAccountService AccountService { set; get; }
        /// <summary>
        /// Status of current connection to server
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; NotifyPropertyChanged("IsConnected"); }
        }
        private bool _isConnected { set; get; }
        /// <summary>
        /// Status of user's authentication to server
        /// </summary>
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; NotifyPropertyChanged("IsLoggedIn"); }
        }
        private bool _isLoggedIn { set; get; }
        /// <summary>
        /// Account holder email address (as login name)
        /// </summary>
        public string Email
        {
            get { return _email; }
            set { _email = value; NotifyPropertyChanged("Email"); }
        }
        private string _email { set; get; }
        /// <summary>
        /// Account holder password
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; NotifyPropertyChanged("Password"); }
        }
        private string _password { set; get; }
        public BeforeOurTime.Models.Modules.Account.Models.Account Account
        {
            get { return _account; }
            set { _account = value; NotifyPropertyChanged("Account"); }
        }
        private BeforeOurTime.Models.Modules.Account.Models.Account _account { set; get; }
        /// <summary>
        /// Gatekeeper introduction text
        /// </summary>
        public string Gatekeeper
        {
            get { return _gatekeeper; }
            set { _gatekeeper = value; NotifyPropertyChanged("Gatekeeper"); }
        }
        private string _gatekeeper { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public LoginPageViewModel(IContainer container) : base(container)
        {
            WebSocketService = Container.Resolve<IWebSocketService>();
            AccountService = Container.Resolve<IAccountService>();
            IsConnected = WebSocketService.IsConnected();
            IsLoggedIn = AccountService.IsLoggedIn();
            WebSocketService.OnStateChange += OnWebSocketStateChange;
            AccountService.OnStateChange += OnAccountLoginStateChange;
            Gatekeeper = "The Gatekeeper, unsure of your visage, eyes you with "
                       + "suspicious anxiety. Do you even belong in this "
                       + "place? A glare in your eye can be sourced to the Gatekeeper's " 
                       + "shining bald head. A perpetual fidgeting of his right hand cause "
                       + "the constent ringing of several old keys grinding "
                       + "one against the other. He may be prepared to allow "
                       + "you passage, but only with some additional proding";
        }
        /// <summary>
        /// Authenticate to the server with the credentials provided by the form
        /// </summary>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            Working = true;
            try
            {
                Account = await AccountService.LoginAsync(Email, Password);
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Create new account with credentials provided by the form
        /// </summary>
        /// <returns>Guid of newly created account</returns>
        public async Task RegisterAsync()
        {
            try
            {
                Working = true;
                Account = await AccountService.RegisterAsync(Email, Password);
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Log account out of the server
        /// </summary>
        public async Task LogoutAsync()
        {
            var AccountService = Container.Resolve<IAccountService>();
            Working = true;
            bool logout = await AccountService.Logout();
            Account = AccountService.GetAccount();
            Working = false;
        }
        /// <summary>
        /// Update IsConnected status each time the WebSocket state changes
        /// </summary>
        /// <param name="state">New WebSocket connection status state</param>
        private void OnWebSocketStateChange(WebSocketState state)
        {
            IsConnected = WebSocketService.IsConnected();
        }
        /// <summary>
        /// Update IsLoggedIn status each time the account login state changes
        /// </summary>
        /// <param name="state">New login state value</param>
        private void OnAccountLoginStateChange(LoginState state)
        {
            IsLoggedIn = AccountService.IsLoggedIn();
        }
    }
}
