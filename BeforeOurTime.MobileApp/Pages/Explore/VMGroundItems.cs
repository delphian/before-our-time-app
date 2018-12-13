using Autofac;
using BeforeOurTime.Models.Messages;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Messages.MoveItem;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.World.ItemProperties.Exits;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.ReadLocationSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Explore
{
    /// <summary>
    /// View model for items on the ground
    /// </summary>
    public class VMGroundItems : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// Location resource
        /// </summary>
        private VMLocation VMLocation { set; get; }
        /// <summary>
        /// Player's character item id
        /// </summary>
        private Guid PlayerId { set; get; }
        /// <summary>
        /// List of items on the ground
        /// </summary>
        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; NotifyPropertyChanged("Items"); }
        }
        private List<Item> _items { set; get; } = new List<Item>();
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public VMGroundItems(
            IContainer container,
            VMLocation vmLocation,
            Guid playerId)
        {
            Container = container;
            VMLocation = vmLocation;
            PlayerId = playerId;
        }
        /// <summary>
        /// Listen to unprompted incoming messages (events)
        /// </summary>
        /// <param name="message"></param>
        public void OnMessage(IMessage message)
        {
            // Location has changed
            if (message.IsMessageType<WorldReadLocationSummaryResponse>())
            {
                var msg = message.GetMessageAsType<WorldReadLocationSummaryResponse>();
                Items.RemoveAll(x => true);
                Add(msg.Items);
            }
            // Item has moved
            if (message.IsMessageType<CoreMoveItemEvent>())
            {
                var msg = message.GetMessageAsType<CoreMoveItemEvent>();
                if (msg.NewParent.Id == VMLocation.Item.Id && msg.Item.Id != PlayerId)
                {
                    // Item has arrived
                    Add(new List<Item>() { msg.Item });
                }
                if (msg.OldParent.Id == VMLocation.Item.Id && msg.Item.Id != PlayerId)
                {
                    // Item has departed
                    Items.Remove(Items
                        .Where(x => x.Id == msg.Item.Id)
                        .Select(x => x)
                        .FirstOrDefault());
                    Items = Items.ToList();
                }
            }
        }
        /// <summary>
        /// Add items to the locally tracked items on ground
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public VMGroundItems Add(List<Item> items)
        {
            items.ForEach(item =>
            {
                if (!item.HasProperty<ExitItemProperty>())
                {
                    Items.Add(item);
                }
            });
            Items = Items.ToList();
            return this;
        }
        /// <summary>
        /// Remove items from the locally tracked items on ground
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public VMGroundItems Remove(List<Item> items)
        {
            Items.RemoveAll(x => items.Select(y => y.Id).ToList().Contains(x.Id));
            Items = Items.ToList();
            return this;
        }
    }
}
