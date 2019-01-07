using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Modules.Core.Messages.ItemJson;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Admin.JsonEditor
{
    public class VMJsonEditorPage : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        protected IItemService ItemService { set; get; }
        public VMVisible VMVisible
        {
            get { return _vmVisible; }
            set { _vmVisible = value; NotifyPropertyChanged("VMVisible"); }
        }
        private VMVisible _vmVisible { set; get; }
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
        /// When page appears ItemId should be used to load item from server
        /// </summary>
        public bool PreLoad
        {
            get { return _preLoad; }
            set { _preLoad = value; NotifyPropertyChanged("PreLoad"); }
        }
        private bool _preLoad { set; get; } = false;
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
        public VMJsonEditorPage(IContainer container, Guid? ItemId = null) : base(container)
        {
            ItemService = Container.Resolve<IItemService>();
            if (ItemId != null)
            {
                _itemId = ItemId.Value;
            }
            VMVisible = new VMVisible();
        }
        /// <summary>
        /// Read item from server
        /// </summary>
        /// <returns></returns>
        public async Task ReadItem()
        {
            CoreItemJson = (await ItemService.ReadJsonAsync(new List<Guid>() { _itemId }))?.FirstOrDefault();
            VMVisible.SetVisibility(CoreItemJson.JSON);
        }
        /// <summary>
        /// Create items from json
        /// </summary>
        /// <returns></returns>
        public async Task CreateItem()
        {
            Working = true;
            try
            {
                var coreItemJsons = await ItemService.CreateJsonAsync(ItemJson);
                CoreItemJson = coreItemJsons.First();
                VMVisible.SetVisibility(CoreItemJson.JSON);
            }
            finally
            {
                Working = false;
            }
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
                var coreItemJson = new CoreItemJson()
                {
                    Id = ItemId,
                    IncludeChildren = true,
                    JSON = ItemJson
                };
                await ItemService.UpdateJsonAsync(new List<CoreItemJson>() { coreItemJson });
                VMVisible.SetVisibility(coreItemJson.JSON);
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
