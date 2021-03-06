﻿using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.World.ItemProperties.Exits;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Exit
{
    public class ExitEditorPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        protected IItemService ItemService { set; get; }
        /// <summary>
        /// List of all exits
        /// </summary>
        private List<Item> _exits { set; get; }
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
            Item exitItem = (await ItemService.ReadAsync(new List<Guid>() { itemId }))?
                .FirstOrDefault();
            VMSelectedExit = new ViewModelExit()
            {
                ItemId = exitItem.Id.ToString(),
                ExitId = exitItem.GetData<ExitItemData>().Id.ToString(),
                Name = exitItem.GetData<VisibleItemData>().Name,
                Description = exitItem.GetData<VisibleItemData>().Description
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
                    var items = await ItemService.ReadByDataTypeAsync(new List<string>() {
                        typeof(ExitItemData).ToString()
                    });
                    _exits = items.ToList();
                    var vmExits = new List<ViewModelExit>();
                    _exits.ForEach((exit) =>
                    {
                        vmExits.Add(new ViewModelExit()
                        {
                            ItemId = exit.Id.ToString(),
                            ExitId = exit.GetData<ExitItemData>().Id.ToString(),
                            Name = exit.GetProperty<VisibleItemProperty>().Name,
                            Description = exit.GetProperty<VisibleItemProperty>().Description
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
            var exit = item.GetData<VisibleItemData>();
            exit.Name = VMSelectedExit.Name;
            exit.Description = VMSelectedExit.Description;
            await ItemService.UpdateAsync(new List<Item>() { item });
        }
    }
}
