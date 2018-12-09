using Autofac;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Game
{
    /// <summary>
    /// View model for player inventory
    /// </summary>
    public class VMInventory : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// List of items in inventory as view models
        /// </summary>
        public List<VMInventoryItem> VMItems
        {
            get { return _vmItems; }
            set { _vmItems = value; NotifyPropertyChanged("VMItems"); }
        }
        private List<VMInventoryItem> _vmItems { set; get; } = new List<VMInventoryItem>();
        /// <summary>
        /// List of items in inventory
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
        public VMInventory(IContainer container)
        {
            Container = container;
        }
        /// <summary>
        /// Add items to the locally tracked inventory
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public VMInventory Add(List<Item> items)
        {
            items.ForEach(item =>
            {
                Items.Add(item);
                VMItems.Add(new VMInventoryItem()
                {
                    Id = item.Id,
                    Name = item.GetProperty<VisibleItemProperty>()?.Name ?? "** Unknown **"
                });
            });
            Items = Items.ToList();
            VMItems = VMItems.ToList();
            return this;
        }
        /// <summary>
        /// Remove items from the locally tracked inventory
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public VMInventory Remove(List<Item> items)
        {
            VMItems.RemoveAll(x => items.Select(y => y.Id).ToList().Contains(x.Id));
            Items.RemoveAll(x => items.Select(y => y.Id).ToList().Contains(x.Id));
            Items = Items.ToList();
            VMItems = VMItems.ToList();
            return this;
        }
    }
    /// <summary>
    /// A single inventory item
    /// </summary>
    public class VMInventoryItem : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Unique item identifier
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set { _id = value; NotifyPropertyChanged("Id"); }
        }
        private Guid _id { set; get; }
        /// <summary>
        /// Name of item
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        private string _name { set; get; }
    }
}
