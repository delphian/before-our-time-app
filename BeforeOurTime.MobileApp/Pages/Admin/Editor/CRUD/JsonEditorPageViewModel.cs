using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Messages.CRUD.Items.ReadItem;
using BeforeOurTime.Models.Modules.Core.Messages.ReadItemJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD
{
    public class JsonEditorPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        protected IItemService ItemService { set; get; }
        /// <summary>
        /// Unique item identifier to operate on
        /// </summary>
        public string ItemId {
            get { return _itemId.ToString(); }
            set {
                if (Guid.TryParse(value, out Guid newGuid))
                {
                    _itemId = newGuid;
                    NotifyPropertyChanged("ItemId");
                }
            }
        }
        private Guid _itemId { set; get; }
        /// <summary>
        /// Item as json data
        /// </summary>
        public string ItemJson
        {
            get { return _itemJson; }
            set { _itemJson = value; NotifyPropertyChanged("ItemJson"); }
        }
        private string _itemJson { set; get; }
        /// <summary>
        /// Item loaded from a previous Read or Create
        /// </summary>
        public CoreItemJson CoreItemJson
        {
            get { return _coreItemJson; }
            set
            {
                _coreItemJson = value;
                ItemId = _coreItemJson.Id.ToString();
                ItemJson = _coreItemJson.JSON;
                NotifyPropertyChanged("CoreItemJson");
            }
        }
        private CoreItemJson _coreItemJson { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        /// <param name="itemId">When provided automatically load this item for editing</param>
        public JsonEditorPageViewModel(IContainer container, Guid? ItemId = null) : base(container)
        {
            ItemService = Container.Resolve<IItemService>();
            if (ItemId != null)
            {
                _itemId = ItemId.Value;
            }
        }
        /// <summary>
        /// Read item from server
        /// </summary>
        /// <returns></returns>
        public async Task ReadItem()
        {
            CoreItemJson = (await ItemService.ReadJsonAsync(new List<Guid>() { _itemId }))?.FirstOrDefault();
        }
        /// <summary>
        /// Update multiple items on server
        /// </summary>
        /// <returns></returns>
        public async Task UpdateItem()
        {
            Working = true;
            try
            {
                var item = JsonConvert.DeserializeObject<Item>(_itemJson);
                await ItemService.UpdateAsync(new List<Item>() { item });
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Delete multiple items on server
        /// </summary>
        /// <returns></returns>
        public async Task DeleteItem()
        {
            Working = true;
            try
            {
                await ItemService.DeleteAsync(new List<Guid>() { _itemId });
            }
            finally
            {
                Working = false;
            }
        }
    }
}
