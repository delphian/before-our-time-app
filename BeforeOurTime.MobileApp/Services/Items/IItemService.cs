using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Messages;
using BeforeOurTime.Models.Messages.CRUD.Items.ReadItemGraph;
using BeforeOurTime.Models.Messages.Requests;
using BeforeOurTime.Models.Messages.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Items
{
    /// <summary>
    /// Item service for CRUD operations
    /// </summary>
    public interface IItemService : IService
    {
        /// <summary>
        /// Read multiple items based on a list of unique item identifiers
        /// </summary>
        /// <param name="itemIds">List of unique item identifiers</param>
        /// <returns></returns>
        Task<List<Item>> ReadAsync(List<Guid> itemIds);
        /// <summary>
        /// Read item graph
        /// </summary>
        /// <param name="itemIds">Item to begin graph with</param>
        /// <returns></returns>
        Task<ItemGraph> ReadGraphAsync(Guid? itemId = null);
        /// <summary>
        /// Read multiple items based on a list of item attribute types
        /// </summary>
        /// <param name="AttributeTypes">List of unique attribute type names (class names)</param>
        /// <returns></returns>
        Task<List<Item>> ReadByTypeAsync(List<string> AttributeTypes);
        /// <summary>
        /// Update multiple items
        /// </summary>
        /// <param name="items">List of items to update</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unable to update items</exception>
        Task UpdateAsync(List<Item> items);
        /// <summary>
        /// Delete multiple items
        /// </summary>
        /// <param name="itemIds">List of unique item identifiers to delete</param>
        /// <returns></returns>
        /// <exception cref="Exception">Unable to delete items</exception>
        Task DeleteAsync(List<Guid> itemIds);
    }
}
