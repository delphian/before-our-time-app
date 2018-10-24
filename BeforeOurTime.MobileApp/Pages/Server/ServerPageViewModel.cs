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
        /// Local storage settings
        /// </summary>
        public Settings Settings { set; get; }
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
        /// Setup
        /// </summary>
        /// <returns></returns>
        public async Task OnAppearing()
        {
            if (Settings == null)
            {
                if (Application.Current.Properties.ContainsKey("Settings"))
                {
                    Settings = JsonConvert.DeserializeObject<Settings>((string)Application.Current.Properties["Settings"]);
                }
                else
                {
                    Settings = new Settings();
                    Application.Current.Properties.Add("Settings", JsonConvert.SerializeObject(Settings));
                    await Application.Current.SavePropertiesAsync();
                }
            }
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
            webSocketService.SetConnectionString(connectionString);
            try
            {
                if (webSocketService.GetState() == WebSocketState.Open)
                {
                    await Container.Resolve<IWebSocketService>().CloseAsync();
                }
                await webSocketService.ConnectAsync();
                await webSocketService.ListenAsync();
                Settings.ConnectionString = connectionString;
                Application.Current.Properties["Settings"] = JsonConvert.SerializeObject(Settings);
                await Application.Current.SavePropertiesAsync();
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
            }
        }
        /// <summary>
        /// Attempt to use connect to last used server
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            var connectionString = Settings.ConnectionString ?? "ws://beforeourtime.world:2024/ws";
            await ConnectAsync(connectionString);
        }
        /// <summary>
        /// Log in with cached credentials or create a temporary account
        /// </summary>
        /// <returns></returns>
        public async Task LoginAsync()
        {
            var account = AccountService.GetAccount();
            if (account?.Name == null)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var suffix = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
                var name = "Account_" + suffix;
                account = await AccountService.RegisterAsync(name, "password", true);
            }
            else
            {
                account = await AccountService.LoginAsync(account.Name, account.Password);
            }
        }
        /// <summary>
        /// Select last played character or create new temporary character
        /// </summary>
        /// <returns></returns>
        public async Task SelectCharacterAsync()
        {
            var account = AccountService.GetAccount();
            if (Settings.CharacterId == null)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var suffix = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
                var name = "Ghost_" + suffix;
                var characterItemId = await CharacterService.CreateAccountCharacterAsync(
                    account.Id,
                    name,
                    true);
                Settings.CharacterId = characterItemId;
                Application.Current.Properties["Settings"] = JsonConvert.SerializeObject(Settings);
                await Application.Current.SavePropertiesAsync();
            }
            var characters = await CharacterService.GetAccountCharactersAsync(account.Id);
            var character = characters.Where(x => x.Id == Settings.CharacterId).FirstOrDefault();
            if (character != null)
            {
                await CharacterService.PlayAccountCharacterAsync(character);
            }
            else
            {
                Settings.CharacterId = null;
                Application.Current.Properties["Settings"] = JsonConvert.SerializeObject(Settings);
                await Application.Current.SavePropertiesAsync();
                throw new Exception("Can't locate temporary character");
            }
        }
    }
}
