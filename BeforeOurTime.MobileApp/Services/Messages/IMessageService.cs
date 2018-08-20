using BeforeOurTime.Models.Messages;
using BeforeOurTime.Models.Messages.Requests;
using BeforeOurTime.Models.Messages.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Messages
{
    public interface IMessageService : IService
    {
        /// <summary>
        /// Event that will fire when IMessage is recieved
        /// </summary>
        event RecieveMessage OnMessage;
        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="message"></param>
        Task SendAsync(IMessage message);
        /// <summary>
        /// Send request to the server and return associated response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<T> SendRequestAsync<T>(IRequest request) where T : IResponse, new();
        /// <summary>
        /// Subscribe a delegate to a specific message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messageHandler"></param>
        /// <param name="responseInstanceId"></param>
        /// <param name="limit">Maximum number of times to invoke delegate before unsubscribing</param>
        Subscription Subscribe<T>(
            RecieveMessage messageHandler,
            Guid? responseInstanceId = null,
            int? limit = null);
        /// <summary>
        /// Unsubscribe a delegate from a specific message type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recieveMessage"></param>
        void Unsubscribe<T>(RecieveMessage recieveMessage);
        /// <summary>
        /// Unsubscribe a delegate from a specific message type
        /// </summary>
        /// <param name="recieveMessage"></param>
        /// <param name="type"></param>
        void Unsubscribe(RecieveMessage recieveMessage, Type type);
    }
}
