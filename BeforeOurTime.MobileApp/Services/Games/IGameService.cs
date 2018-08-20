using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Messages.Responses.List;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Games
{
    /// <summary>
    /// Business logic and data store for the client's core gameplay functionality
    /// </summary>
    public interface IGameService : IService
    {
        /// <summary>
        /// Event that will fire when IMessage is recieved
        /// </summary>
        event RecieveMessage OnMessage;
        /// <summary>
        /// Get summary of current player's location, potential exits, and items present
        /// </summary>
        /// <typeparam name="ListLocationResponse"></typeparam>
        /// <returns></returns>
        Task<ListLocationResponse> GetLocationSummary();
    }
}
