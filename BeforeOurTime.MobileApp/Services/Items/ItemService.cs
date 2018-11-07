using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.MobileApp.Services.WebSockets;
using BeforeOurTime.Models.Exceptions;
using BeforeOurTime.Models.Modules.Core.Messages.ItemCrud.DeleteItem;
using BeforeOurTime.Models.Modules.Core.Messages.ItemCrud.ReadItem;
using BeforeOurTime.Models.Modules.Core.Messages.ItemCrud.UpdateItem;
using BeforeOurTime.Models.Modules.Core.Messages.ItemGraph;
using BeforeOurTime.Models.Modules.Core.Messages.ItemJson;
using BeforeOurTime.Models.Modules.Core.Messages.ItemJson.CreateItemJson;
using BeforeOurTime.Models.Modules.Core.Messages.ItemJson.ReadItemJson;
using BeforeOurTime.Models.Modules.Core.Messages.ItemJson.UpdateItemJson;
using BeforeOurTime.Models.Modules.Core.Models.Items;
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
        /// Create multiple items from JSON string
        /// </summary>
        /// <param name="json">String of json to create items from</param>
        /// <param name="recursive">Recursively include all children items in json response</param>
        /// <returns></returns>
        public async Task<List<CoreItemJson>> CreateJsonAsync(
            string json,
            bool recursive = false)
        {
            var response = await MessageService.SendRequestAsync<CoreCreateItemJsonResponse>(new CoreCreateItemJsonRequest()
            {
                ItemJson = json
            });
            if (!response.IsSuccess())
            {
                throw new Exception($"Unable to create items: {response._responseMessage}");
            }
            return response.CoreCreateItemJsonEvent.ItemsJson;
        }
        /// <summary>
        /// Read multiple items based on a list of unique item identifiers
        /// </summary>
        /// <param name="itemIds">List of unique item identifiers</param>
        /// <returns></returns>
        public async Task<List<Item>> ReadAsync(List<Guid> itemIds = null)
        {
            var response = await MessageService.SendRequestAsync<CoreReadItemCrudResponse>(new CoreReadItemCrudRequest()
            {
                ItemIds = itemIds
            });
            return response.CoreReadItemCrudEvent.Items;
        }
        /// <summary>
        /// Read JSON of multiple items based on a list of unique item identifiers
        /// </summary>
        /// <param name="itemIds">List of unique item identifiers</param>
        /// <param name="recursive">Recursively include all children items in json response</param>
        /// <returns></returns>
        public async Task<List<CoreItemJson>> ReadJsonAsync(
            List<Guid> itemIds = null,
            bool recursive = false)
        {
            var response = await MessageService.SendRequestAsync<CoreReadItemJsonResponse>(new CoreReadItemJsonRequest()
            {
                IncludeChildren = recursive,
                ItemIds = itemIds
            });
            if (!response.IsSuccess())
            {
                throw new Exception($"Unable to read items: {response._responseMessage}");
            }
            return response.CoreReadItemJsonEvent.ItemsJson;
        }
        /// <summary>
        /// Read item graph
        /// </summary>
        /// <param name="itemIds">Item to begin graph with</param>
        /// <returns></returns>
        public async Task<ItemGraph> ReadGraphAsync(Guid? itemId = null)
        {
            var response = await MessageService.SendRequestAsync<CoreReadItemGraphResponse>(new CoreReadItemGraphRequest()
            {
                ItemId = itemId
            });
            return response.CoreReadItemGraphEvent.ItemGraph;
        }
        /// <summary>
        /// Read multiple items based on a list of item types
        /// </summary>
        /// <param name="itemTypes">List of unique item type names (class names)</param>
        /// <returns></returns>
        public async Task<List<Item>> ReadByTypeAsync(List<string> itemTypes)
        {
            var response = await MessageService.SendRequestAsync<CoreReadItemCrudResponse>(
                new CoreReadItemCrudRequest()
                {
                    ItemTypes = itemTypes
                });
            return response.CoreReadItemCrudEvent.Items;
        }
        /// <summary>
        /// Update multiple items
        /// </summary>
        /// <param name="items">List of items to update</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unable to update items</exception>
        public async Task UpdateAsync(List<Item> items)
        {
            var response = await MessageService.SendRequestAsync<CoreUpdateItemCrudResponse>(new CoreUpdateItemCrudRequest()
            {
                Items = items
            });
            if (!response.IsSuccess())
            {
                throw new Exception("Unable to update items");
            }
        }
        /// <summary>
        /// Update multiple items through json objects
        /// </summary>
        /// <param name="itemsJson">List of items to update</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unable to update items</exception>
        public async Task UpdateJsonAsync(List<CoreItemJson> itemsJson)
        {
            var response = await MessageService.SendRequestAsync<CoreUpdateItemJsonResponse>(new CoreUpdateItemJsonRequest()
            {
                ItemsJson = itemsJson
            });
            if (!response.IsSuccess())
            {
                throw new Exception($"Unable to update items: {response._responseMessage}");
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
            var response = await MessageService.SendRequestAsync<CoreDeleteItemCrudResponse>(new CoreDeleteItemCrudRequest()
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
