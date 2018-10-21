using Autofac;
using BeforeOurTime.MobileApp.Models;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Characters;
using BeforeOurTime.MobileApp.Services.WebSockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Pages.Server
{
    /// <summary>
    /// View model for server page
    /// </summary>
    public class ServerPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
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
        /// Account character management service
        /// </summary>
        private ICharacterService CharacterService { set; get; }
        /// <summary>
        /// All available servers to choose from
        /// </summary>
        public List<string> ConnectionStrings { set; get; } = new List<string>()
        {
            "ws://10.0.2.2:2024/ws",
            "ws://localhost:2024/ws",
            "ws://beforeourtime.world:2024/ws"
        };
        /// <summary>
        /// Currently selected connection string
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; NotifyPropertyChanged("ConnectionString"); }
        }
        private string _connectionString { set; get; } = "ws://beforeourtime.world:2024/ws";
        /// <summary>
        /// Description of network error
        /// </summary>
        public string Error
        {
            get { return _error; }
            set { _error = value; NotifyPropertyChanged("Error"); }
        }
        private string _error { set; get; } = null;
        /// <summary>
        /// Presence of absence of network error
        /// </summary>
        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; NotifyPropertyChanged("IsError"); }
        }
        private bool _isError { set; get; } = false;
        /// <summary>
        /// Indicate if client is currently connected to the server
        /// </summary>
        public bool IsConnected
        {
            get { return _isConnected; }
            set { _isConnected = value; NotifyPropertyChanged("IsConnected"); }
        }
        private bool _isConnected { set; get; } = false;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public ServerPageViewModel(IContainer container) : base(container)
        {
            WebSocketService = Container.Resolve<IWebSocketService>();
            AccountService = Container.Resolve<IAccountService>();
            CharacterService = Container.Resolve<ICharacterService>();
            WebSocketService.OnStateChange += OnWebSocketStateChange;
        }
        /// <summary>
        /// Update connection status based on WebSocket state
        /// </summary>
        /// <param name="webSocketState"></param>
        public void OnWebSocketStateChange(WebSocketState webSocketState)
        {
            IsConnected = WebSocketService.IsConnected();
        }
        /// <summary>
        /// Disconnect from server
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectAsync()
        {
            Working = true;
            try
            {
                await WebSocketService.CloseAsync();
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <param name="connectionString">Server connection string</param>
        /// <returns></returns>
        public async Task ConnectAsync(string connectionString)
        {
            var webSocketService = Container.Resolve<IWebSocketService>();
            var accountService = Container.Resolve<IAccountService>();
            IsError = false;
            Error = null;
            Working = true;
            webSocketService.SetConnectionString(connectionString);
            try
            {
                if (webSocketService.GetState() == WebSocketState.Open)
                {
                    await Container.Resolve<IWebSocketService>().CloseAsync();
                }
                await webSocketService.ConnectAsync();
                await webSocketService.ListenAsync();
                if (Application.Current.Properties.ContainsKey("Server:ConnectionString"))
                {
                    Application.Current.Properties["Server:ConnectionString"] = connectionString;
                }
                else
                {
                    Application.Current.Properties.Add("Server:ConnectionString", connectionString);
                }
                await Application.Current.SavePropertiesAsync();
                Working = false;
            }
            catch (Exception e)
            {
                Error = e.Message;
                Exception traverse = e.InnerException;
                while (traverse != null)
                {
                    Error += $"({traverse.Message})";
                    traverse = traverse.InnerException;
                }
                IsError = true;
                Application.Current.Properties.Remove("Server:ConnectionString");
                await Application.Current.SavePropertiesAsync();
                Working = false;
            }
        }
        /// <summary>
        /// Attempt to use connect to last used server
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            var connectionString = "ws://beforeourtime.world:2024/ws";
            if (Application.Current.Properties.ContainsKey("Server:ConnectionString"))
            {
                connectionString = (string)Application.Current.Properties["Server:ConnectionString"];
            }
            await ConnectAsync(connectionString);
        }
        /// <summary>
        /// Log in with cached credentials or create a temporary account
        /// </summary>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            Settings settings;
            if (Application.Current.Properties.ContainsKey("Settings"))
            {
                settings = (Settings)Application.Current.Properties["Settings"];
            }
            else
            {
                settings = new Settings();
                Application.Current.Properties.Add("Settings", settings);
                await Application.Current.SavePropertiesAsync();
            }
            if (settings.Name == null)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var suffix = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
                var name = "Account_" + suffix;
                var account = await AccountService.RegisterAsync(name, "password", true);
                settings.AccountId = account.AccountId;
                settings.Name = account.Name;
                settings.Password = "password";
                Application.Current.Properties["Settings"] = settings;
                await Application.Current.SavePropertiesAsync();
            }
            else
            {
                var account = await AccountService.LoginAsync(settings.Name, settings.Password);
            }
        }
        /// <summary>
        /// Select last played character or create new temporary character
        /// </summary>
        /// <returns></returns>
        public async Task SelectCharacterAsync()
        {
            var settings = (Settings)Application.Current.Properties["Settings"];
            if (settings.CharacterId == null)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var suffix = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
                var name = "Ghost_" + suffix;
                var characterItemId = await CharacterService.CreateAccountCharacterAsync(
                    settings.AccountId.Value,
                    name,
                    true);
                settings.CharacterId = characterItemId;
                Application.Current.Properties["Settings"] = settings;
                await Application.Current.SavePropertiesAsync();
            }
            var characters = await CharacterService.GetAccountCharactersAsync(settings.AccountId.Value);
            var character = characters.Where(x => x.Id == settings.CharacterId).FirstOrDefault();
            if (character != null)
            {
                await CharacterService.PlayAccountCharacterAsync(character);
            }
            else
            {
                settings.CharacterId = null;
                Application.Current.Properties["Settings"] = settings;
                await Application.Current.SavePropertiesAsync();
                throw new Exception("Can't locate temporary character");
            }
        }
    }
}
