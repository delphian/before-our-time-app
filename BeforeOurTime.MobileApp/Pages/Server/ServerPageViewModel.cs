using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.WebSockets;
using System;
using System.Collections.Generic;
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
                Application.Current.Properties["Server:ConnectionString"] = connectionString;
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
            if (Application.Current.Properties.ContainsKey("Server:ConnectionString"))
            {
                var connectionString = (string)Application.Current.Properties["Server:ConnectionString"];
                await ConnectAsync(connectionString);
            }
        }
    }
}
