using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.WebSockets
{
    /// <summary>
    /// Methods and events to communicate with websocket server
    /// </summary>
    public interface IWebSocketService : IService
    {
        /// <summary>
        /// Events that will fire when connection state changes
        /// </summary>
        event RecieveState OnStateChange;
        /// <summary>
        /// Event that will fire when an error is encountered
        /// </summary>
        event RecieveMessage OnError;
        /// <summary>
        /// Connect to the websocket server
        /// </summary>
        Task ConnectAsync();
        /// <summary>
        /// Listen to the websocket
        /// </summary>
        /// <returns></returns>
        Task ListenAsync();
        /// <summary>
        /// Disconnect from the websocket server
        /// </summary>
        Task CloseAsync();
        /// <summary>
        /// Send message to server
        /// </summary>
        /// <param name="message"></param>
        Task SendAsync(string message);
        /// <summary>
        /// Set the connection string
        /// </summary>
        /// <param name="connectionString"></param>
        void SetConnectionString(string connectionString);
        /// <summary>
        /// Get current websocket connection state
        /// </summary>
        WebSocketState GetState();
        /// <summary>
        /// Report if websocket is connected to a server
        /// </summary>
        /// <returns></returns>
        bool IsConnected();
    }
}
