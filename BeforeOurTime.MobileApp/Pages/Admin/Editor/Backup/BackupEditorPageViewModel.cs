using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.MobileApp.Services.Loggers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Backup
{
    public class BackupEditorPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Record errors and information during program execution
        /// </summary>
        private ILoggerService LoggerService { set; get; }
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        private IItemService ItemService { set; get; }
        public string JsonItems
        {
            get { return _jsonItems; }
            set { _jsonItems = value; NotifyPropertyChanged("JsonItems"); }
        } 
        private string _jsonItems { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public BackupEditorPageViewModel(IContainer container) : base(container)
        {
            LoggerService = container.Resolve<ILoggerService>();
            ItemService = container.Resolve<IItemService>();
        }
        /// <summary>
        /// Retrieve JSON backup of all items from server
        /// </summary>
        /// <returns></returns>
        public async Task LoadJsonItems()
        {
            Working = true;
            try
            {
                var items = await ItemService.ReadAsync(new List<Guid>());
                JsonItems = JsonConvert.SerializeObject(items, Formatting.Indented);
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Copy JSON encoded item backup to system clipboard
        /// </summary>
        public void CopyToClipboard()
        {
            Plugin.Clipboard.CrossClipboard.Current.SetText(JsonItems);
        }
    }
}
