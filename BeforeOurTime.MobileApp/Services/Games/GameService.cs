using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Messages;
using BeforeOurTime.Models.Messages.Locations.ReadLocationSummary;
using BeforeOurTime.Models.Messages.Requests.List;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Games
{
    /// <summary>
    /// Business logic and data store for the client's core gameplay functionality
    /// </summary>
    public class GameService : IGameService
    {
        /// <summary>
        /// Event that will fire when IMessage is recieved
        /// </summary>
        public event RecieveMessage OnMessage;
        /// <summary>
        /// Account management service
        /// </summary>
        private IAccountService AccountService { set; get; }
        /// <summary>
        /// Manage IMessage messages between client and server
        /// </summary>
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// Player's location
        /// </summary>
        public Item Location { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountService">Account management service</param>
        /// <param name="messageService">Manage IMessage messages between client and server</param>
        public GameService(IAccountService accountService, IMessageService messageService)
        {
            AccountService = accountService;
            MessageService = messageService;
            MessageService.OnMessage += OnMessageListener;
        }
        /// <summary>
        /// Listen for incoming messages of a game nature
        /// </summary>
        /// <param name="message">Communication from the server</param>
        private void OnMessageListener(IMessage message)
        {
            if (message.IsMessageType<ReadLocationSummaryResponse>()) {
                Location = message.GetMessageAsType<ReadLocationSummaryResponse>().Item;
            }
            OnMessage?.Invoke(message);
        }
        /// <summary>
        /// Get summary of current player's location, potential exits, and items present
        /// </summary>
        /// <typeparam name="ListLocationResponse"></typeparam>
        /// <returns></returns>
        public async Task<ReadLocationSummaryResponse> GetLocationSummary()
        {
            return await MessageService.SendRequestAsync<ReadLocationSummaryResponse>(new ListLocationRequest() { });
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
