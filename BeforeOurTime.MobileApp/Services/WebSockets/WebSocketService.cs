using BeforeOurTime.MobileApp.Services.Loggers;
using BeforeOurTime.Models.Messages.Requests;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.WebSockets
{
    /// <summary>
    /// Structure that a WebSocket connection state event handler must implement
    /// </summary>
    public delegate void RecieveState(WebSocketState wsState);
    /// <summary>
    /// Structure that callback function must implement
    /// </summary>
    /// <param name="message"></param>
    public delegate void RecieveMessage(string message);
    /// <summary>
    /// Methods and events to communicate with websocket server
    /// </summary>
    public class WebSocketService : IWebSocketService
    {
        /// <summary>
        /// Record errors and information during program execution
        /// </summary>
        private LoggerService LoggerService { set; get; }
        /// <summary>
        /// Connection string for websocket server
        /// </summary>
        /// <example>
        /// "ws://websocketserver.com:5000/ws"
        /// </example>
        public string ConnectionString { set; get; }
        /// <summary>
        /// Websocket client
        /// </summary>
        public ClientWebSocket Client { set; get; }
        public CancellationTokenSource Cts { set; get; }
        /// <summary>
        /// Current websocket state
        /// </summary>
        public WebSocketState State = WebSocketState.None;
        /// <summary>
        /// Event that will fire when data is recieved
        /// </summary>
        public event RecieveMessage OnData;
        /// <summary>
        /// Events that will fire when connection state changes
        /// </summary>
        public event RecieveState OnStateChange;
        /// <summary>
        /// Event that will fire when an error is encountered
        /// </summary>
        public event RecieveMessage OnError;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerService">Record errors and information during program execution</param>
        /// <param name="connectionString">Connection string for websocket server</param>
        public WebSocketService(LoggerService loggerService, string connectionString)
        {
            LoggerService = loggerService;
            ConnectionString = connectionString;
        }
        /// <summary>
        /// Connect to the websocket server
        /// </summary>
        public async Task ConnectAsync()
        {
            if (GetState() == WebSocketState.Closed || GetState() == WebSocketState.Aborted || GetState() == WebSocketState.None)
            {
                Client = new ClientWebSocket();
                Client.Options.SetBuffer(1024 * 1, 1024 * 1);
                Cts = new CancellationTokenSource();
                var timeoutCancel = new CancellationTokenSource();
                Task timeoutTask = null;
                Task connectTask = null;
                SetState(WebSocketState.Connecting);
                try
                {
                    timeoutTask = Task.Delay(10000);
                    connectTask = Client.ConnectAsync(new Uri(ConnectionString), Cts.Token);
                    if (await Task.WhenAny(timeoutTask, connectTask) == timeoutTask || connectTask.IsFaulted)
                    {
                        Cts.Cancel();
                        Client.Dispose();
                        if (connectTask.IsFaulted)
                        {
                            throw connectTask.Exception;
                        }
                        else
                        {
                            throw new Exception("Timeout connecting to server");
                        }
                    }
                    else
                    {
                        timeoutCancel.Cancel();
                        SetState(WebSocketState.Open);
                    }
                }
                catch (Exception e)
                {
                    SetState(WebSocketState.Aborted);
                    OnError?.Invoke(e.Message);
                    throw e;
                }
            }
        }
        /// <summary>
        /// Listen to the websocket
        /// </summary>
        /// <returns></returns>
        public async Task ListenAsync() { 
            await Task.Factory.StartNew(async () =>
            {
                var buffer = new Byte[1024 * 4];
                string messageJson = "";
                while (Client.State == WebSocketState.Open)
                {
                    try
                    {
                        Array.Clear(buffer, 0, buffer.Count());
                        var result = await Client.ReceiveAsync(new ArraySegment<byte>(buffer), Cts.Token);
                        messageJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        while (!result.EndOfMessage)
                        {
                            Array.Clear(buffer, 0, buffer.Count());
                            result = await Client.ReceiveAsync(new ArraySegment<byte>(buffer), Cts.Token);
                            messageJson += Encoding.UTF8.GetString(buffer, 0, result.Count);
                        }
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            await CloseAsync();
                        }
                        else if (result.MessageType == WebSocketMessageType.Binary)
                        {
                            await Client.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "Cannot accept binary frame", CancellationToken.None);
                        }
                        else
                        {
                            OnData?.Invoke(messageJson);
                        }
                    }
                    catch (Exception)
                    {
                        LoggerService.Log(LogLevel.Error, $"While distributing websocket message: {messageJson}");
                    }
                }
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }
        /// <summary>
        /// Disconnect from the websocket server
        /// </summary>
        public async Task CloseAsync()
        {
            SetState(WebSocketState.CloseSent);
            try
            {
                await Client.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                Client.Dispose();
                Cts.Dispose();
                SetState(WebSocketState.Closed);
            }
            catch (Exception e)
            {
                Client.Dispose();
                Cts.Dispose();
                SetState(WebSocketState.Aborted);
                OnError?.Invoke(e.Message);
            }
        }
        /// <summary>
        /// Send message to server asyncronously
        /// </summary>
        /// <param name="message"></param>
        public IWebSocketService Send(string message)
        {
            var byteMessage = new UTF8Encoding(false, true).GetBytes(message);
            var offset = 0;
            try
            {
                var endOfMessage = false;
                do
                {
                    var remainingBytes = byteMessage.Count() - offset;
                    var sendBytes = Math.Min(1024, remainingBytes);
                    var segment = new ArraySegment<byte>(byteMessage, offset, sendBytes);
                    offset += 1024;
                    endOfMessage = remainingBytes == sendBytes;
                    Client.SendAsync(segment, WebSocketMessageType.Text, endOfMessage, Cts.Token);
                } while (!endOfMessage);
            }
            catch (Exception e)
            {
                SetState(Client.State);
                OnError?.Invoke(e.Message);
            }
            return this;
        }
        /// <summary>
        /// Send message to server asyncronously
        /// </summary>
        /// <param name="message"></param>
        public async Task SendAsync(string message)
        {
            var byteMessage = new UTF8Encoding(false, true).GetBytes(message);
            var offset = 0;
            try
            {
                var endOfMessage = false;
                do
                {
                    var remainingBytes = byteMessage.Count() - offset;
                    var sendBytes = Math.Min(1024, remainingBytes);
                    var segment = new ArraySegment<byte>(byteMessage, offset, sendBytes);
                    offset += 1024;
                    endOfMessage = remainingBytes == sendBytes;
                    await Client.SendAsync(segment, WebSocketMessageType.Text, endOfMessage, Cts.Token);
                } while (!endOfMessage);
            }
            catch (Exception e)
            {
                SetState(Client.State);
                OnError?.Invoke(e.Message);
            }
        }
        /// <summary>
        /// Set the connection string
        /// </summary>
        /// <param name="connectionString"></param>
        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
        /// <summary>
        /// Set the new WebSocket connection state and propogate event
        /// </summary>
        /// <param name="state"></param>
        private void SetState(WebSocketState state)
        {
            if (state != State)
            {
                State = state;
                OnStateChange?.Invoke(state);
            }
        }
        /// <summary>
        /// Get current websocket connection state
        /// </summary>
        public WebSocketState GetState ()
        {
            return State;
        }
        /// <summary>
        /// Report if websocket is connected to a server
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return (GetState() == WebSocketState.Open);
        }
        /// <summary>
        /// Clear any caches the service may be using
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            await Task.Delay(0);
        }
    }
}
