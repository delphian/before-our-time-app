using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.MobileApp.Services.WebSockets;
using BeforeOurTime.Models.Exceptions;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Messages;
using BeforeOurTime.Models.Messages.CRUD.Items.DeleteItem;
using BeforeOurTime.Models.Messages.CRUD.Items.ReadItem;
using BeforeOurTime.Models.Messages.CRUD.Items.UpdateItem;
using BeforeOurTime.Models.Messages.Requests;
using BeforeOurTime.Models.Messages.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Items
{
    /// <summary>
    /// Item service for CRUD operations
    /// </summary>
    public class ItemService : IItemService
    {
        /// <summary>
        /// Manage IMessage messages between client and server
        /// </summary>
        IMessageService MessageService { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageService">Manage IMessage messages between client and server</param>
        public ItemService(MessageService messageService)
        {
            MessageService = messageService;
        }
        /// <summary>
        /// Read multiple items based on a list of unique item identifiers
        /// </summary>
        /// <param name="itemIds">List of unique item identifiers</param>
        /// <returns></returns>
        public async Task<List<Item>> ReadAsync(List<Guid> itemIds)
        {
            var response = await MessageService.SendRequestAsync<ReadItemResponse>(new ReadItemRequest()
            {
                ItemIds = itemIds
            });
            return response.ReadItemEvent.Items;
        }
        /// <summary>
        /// Read multiple items based on a list of item attribute types
        /// </summary>
        /// <param name="attributeTypes">List of unique attribute type names (class names)</param>
        /// <returns></returns>
        public async Task<List<Item>> ReadByTypeAsync(List<string> attributeTypes)
        {
            var response = await MessageService.SendRequestAsync<ReadItemResponse>(new ReadItemRequest()
            {
                ItemAttributeTypes = attributeTypes
            });
            return response.ReadItemEvent.Items;
        }
        /// <summary>
        /// Update multiple items
        /// </summary>
        /// <param name="items">List of items to update</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unable to update items</exception>
        public async Task UpdateAsync(List<Item> items)
        {
            var response = await MessageService.SendRequestAsync<UpdateItemResponse>(new UpdateItemRequest()
            {
                Items = items
            });
            if (!response.IsSuccess())
            {
                throw new Exception("Unable to update items");
            }
        }
        /// <summary>
        /// Delete multiple items
        /// </summary>
        /// <param name="itemIds">List of unique item identifiers to delete</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unable to delete items</exception>
        public async Task DeleteAsync(List<Guid> itemIds)
        {
            var response = await MessageService.SendRequestAsync<DeleteItemResponse>(new DeleteItemRequest()
            {
                ItemIds = itemIds
            });
            if (!response.IsSuccess())
            {
                throw new Exception("Unable to delete items: " + response._responseMessage);
            }
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
