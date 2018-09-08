using BeforeOurTime.MobileApp.Services.Loggers;
using BeforeOurTime.MobileApp.Services.WebSockets;
using BeforeOurTime.Models.Exceptions;
using BeforeOurTime.Models.Messages;
using BeforeOurTime.Models.Messages.Requests;
using BeforeOurTime.Models.Messages.Responses;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Messages
{
    /// <summary>
    /// Structure that OnMessage callback function must implement
    /// </summary>
    /// <param name="message"></param>
    public delegate void RecieveMessage(IMessage message);
    /// <summary>
    /// Manage IMessage messages between client and server
    /// </summary>
    public class MessageService : IMessageService
    {
        /// <summary>
        /// Provide methods and events to communicate with websocket server
        /// </summary>
        public WebSocketService WsService { set; get; }
        /// <summary>
        /// Record errors and information during program execution
        /// </summary>
        private LoggerService LoggerService { set; get; }
        /// <summary>
        /// Event that will fire when IMessage is recieved
        /// </summary>
        public event RecieveMessage OnMessage;
        /// <summary>
        /// Collection of delegates to invoke for recieved message types
        /// </summary>
        protected List<Subscription> OnMessageHandlers;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggerService">Record errors and information during program execution</param>
        /// <param name="wsService">Provide methods and events to communicate with websocket server</param>
        public MessageService(LoggerService loggerService, WebSocketService wsService)
        {
            LoggerService = loggerService;
            WsService = wsService;
            wsService.OnData += RecieveWsData;
            OnMessageHandlers = new List<Subscription>();
        }
        /// <summary>
        /// Recieve raw data from websocket connection and distribute to message handlers
        /// </summary>
        /// <param name="messageJson">Serialized json message</param>
        protected void RecieveWsData(string messageJson)
        {
            IMessage message = null;
            IResponse response = null;
            Message messageObj;
            Type messageType;
            try
            {
                LoggerService.Log(LogLevel.Debug, $"Recieved message: {messageJson}");
                messageObj = JsonConvert.DeserializeObject<Message>(messageJson);
                messageType = Message.GetMessageTypeDictionary()[messageObj.GetMessageId()];
                message = (IMessage)JsonConvert.DeserializeObject(messageJson, messageType);
                LoggerService.Log(LogLevel.Information, $"Recieved message: {message.GetMessageName()}");
                if (message.IsMessageType<Response>())
                {
                    response = (IResponse)JsonConvert.DeserializeObject(messageJson, typeof(Response));
                }
                OnMessage?.Invoke(message);
                OnMessageHandlers
                    .Where(x => x.Type == messageType)
                    .Where(x => x.ResponseInstanceId == null ||
                                x.ResponseInstanceId == response?.GetRequestInstanceId())
                    .ToList()
                    .ForEach(x => x.Invoke(message));
            }
            catch (Exception e)
            {
                LoggerService.Log($"Unable to interpret message from server", e);
                throw new MessageInvalidJsonException(e.Message);
            }
        }
        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message"></param>
        public async Task SendAsync(IMessage message)
        {
            LoggerService.Log(LogLevel.Information, $"Sending message: {message.GetMessageName()}");
            var messageJson = JsonConvert.SerializeObject(message);
            LoggerService.Log(LogLevel.Debug, $"Sending message: {messageJson}"); 
            await WsService.SendAsync(messageJson);
        }
        /// <summary>
        /// Send request to the server and return associated response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<T> SendRequestAsync<T>(IRequest request) where T : IResponse, new()
        {
            var promise = new TaskCompletionSource<T>();
            var subscription = Subscribe<T>(
                response =>
                {
                    promise.SetResult((T)response);
                }, request.GetRequestInstanceId(), 1);
            const int timeoutMs = 10000;
            var ct = new CancellationTokenSource(timeoutMs);
            ct.Token.Register(
                () => {
                    subscription.Unsubscribe();
                    promise.TrySetException(new MessageResponseTimeoutException());
                }, 
                useSynchronizationContext: false);
            await SendAsync(request);
            return await promise.Task;
        }
        /// <summary>
        /// Subscribe a delegate to a specific message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageHandler"></param>
        /// <param name="responseInstanceId"></param>
        /// <param name="limit">Maximum number of times to invoke delegate before unsubscribing</param>
        public Subscription Subscribe<T>(
            RecieveMessage messageHandler, 
            Guid? responseInstanceId = null,
            int? limit = null)
        {
            var subscription = new Subscription()
            {
                MessageService = this,
                Subscriber = messageHandler,
                Type = typeof(T),
                ResponseInstanceId = responseInstanceId,
                Limit = limit
            };
            OnMessageHandlers.Add(subscription);
            return subscription;
        }
        /// <summary>
        /// Unsubscribe a delegate from a specific message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recieveMessage"></param>
        public void Unsubscribe<T>(RecieveMessage recieveMessage)
        {
            OnMessageHandlers.RemoveAll(x => x.Subscriber == recieveMessage && x.Type == typeof(T));
        }
        /// <summary>
        /// Unsubscribe a delegate from a specific message type
        /// </summary>
        /// <param name="recieveMessage"></param>
        /// <param name="type"></param>
        public void Unsubscribe(RecieveMessage recieveMessage, Type type)
        {
            OnMessageHandlers.RemoveAll(x => x.Subscriber == recieveMessage && x.Type == type);
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
    public class Subscription
    {
        public MessageService MessageService { set; get; }
        /// <summary>
        /// Safely decrement count in multi-threaded environment
        /// </summary>
        public Object LockObject = new Object();
        /// <summary>
        /// Delegate to invoke for a received message
        /// </summary>
        public RecieveMessage Subscriber { set; get; }
        /// <summary>
        /// Type of message subscriber wishes to receive
        /// </summary>
        public Type Type { set; get; }
        /// <summary>
        /// If present a response must match the instance identifier of the request
        /// </summary>
        public Guid? ResponseInstanceId { set; get; }
        /// <summary>
        /// Maximum number of times to invoke Subscriber
        /// </summary>
        public int? Limit { set; get; }
        /// <summary>
        /// Number of time subscriber has been invoked
        /// </summary>
        public int Count { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public Subscription()
        {
            Count = 0;
        }
        /// <summary>
        /// Safely invoke subscriber with respect to maximum invoke limit
        /// </summary>
        /// <param name="message"></param>
        public void Invoke(IMessage message)
        {
            int count;
            lock(LockObject)
            {
                count = ++Count;
            }
            if (Limit == null || count <= Limit)
            {
                Subscriber.Invoke(message);
                if (Limit != null && count >= Limit)
                {
                    Unsubscribe();
                }
            }
        }
        /// <summary>
        /// Remove this subscription
        /// </summary>
        public void Unsubscribe()
        {
            MessageService.Unsubscribe(Subscriber, this.Type);
        }
    }
}
