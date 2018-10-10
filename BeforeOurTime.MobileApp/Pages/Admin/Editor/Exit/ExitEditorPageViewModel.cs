using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Items;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Data;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Exit
{
    public class ExitEditorPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        protected IItemService ItemService { set; get; }
        /// <summary>
        /// List of all locations 
        /// </summary>
        private List<ExitItem> _exits { set; get; }
        /// <summary>
        /// List of all locations as view models
        /// </summary>
        public List<ViewModelExit> VMExits
        {
            get { return _vmExits; }
            set { _vmExits = value; NotifyPropertyChanged("VMExits"); }
        }
        private List<ViewModelExit> _vmExits { set; get; } = new List<ViewModelExit>();
        /// <summary>
        /// Selected location view model
        /// </summary>
        public ViewModelExit VMSelectedExit
        {
            get { return _vmSelectedExit; }
            set { _vmSelectedExit = value; NotifyPropertyChanged("VMSelectedExit"); }
        }
        private ViewModelExit _vmSelectedExit { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public ExitEditorPageViewModel(IContainer container) : base(container)
        {
            ItemService = Container.Resolve<IItemService>();
        }
        /// <summary>
        /// Read item from server
        /// </summary>
        /// <returns></returns>
        public async Task ReadItem(Guid itemId)
        {
            ExitItem exitItem = (await ItemService.ReadAsync(new List<Guid>() { itemId }))?
                .FirstOrDefault()
                .GetAsItem<ExitItem>();
            VMSelectedExit = new ViewModelExit()
            {
                ItemId = exitItem.Id.ToString(),
                ExitId = exitItem.GetAttribute<ExitData>().Id.ToString(),
                Name = exitItem.Visible.Name,
                Description = exitItem.Visible.Description
            };
        }
        /// <summary>
        /// Load all items with a location attribute
        /// </summary>
        /// <returns></returns>
        public async Task LoadExits()
        {
            Working = true;
            try
            {
                if (_exits == null || _exits.Count == 0)
                {
                    var items = await ItemService.ReadByTypeAsync(new List<string>() {
                        typeof(ExitData).ToString()
                    });
                    _exits = items.Select(x => x.GetAsItem<ExitItem>()).ToList();
                    var vmExits = new List<ViewModelExit>();
                    _exits.ForEach((exit) =>
                    {
                        vmExits.Add(new ViewModelExit()
                        {
                            ItemId = exit.Id.ToString(),
                            ExitId = exit.GetAttribute<ExitData>().Id.ToString(),
                            Name = exit.Visible.Name,
                            Description = exit.Visible.Description
                        });
                    });
                    VMExits = vmExits;
                }
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Update currently loaded location
        /// </summary>
        /// <returns></returns>
        public async Task UpdateSelectedExit()
        {
            var item = _exits
                .Where(x => x.Id.ToString() == VMSelectedExit.ItemId)
                .FirstOrDefault();
            var exit = item.GetAttribute<ExitData>();
            exit.Name = VMSelectedExit.Name;
            exit.Description = VMSelectedExit.Description;
            await ItemService.UpdateAsync(new List<Item>() { item });
        }
    }
}
