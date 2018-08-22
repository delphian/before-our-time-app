using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Items.Attributes.Exits;
using BeforeOurTime.Models.Items.Attributes.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Location
{
    public class LocationEditorPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        protected IItemService ItemService { set; get; }
        /// <summary>
        /// List of all locations 
        /// </summary>
        private List<Item> _locations { set; get; }
        /// <summary>
        /// List of all locations as view models
        /// </summary>
        public List<ViewModelLocation> VMLocations
        {
            get { return _vmLocations; }
            set { _vmLocations = value; NotifyPropertyChanged("VMLocations"); }
        }
        private List<ViewModelLocation> _vmLocations { set; get; } = new List<ViewModelLocation>();
        /// <summary>
        /// Selected location view model
        /// </summary>
        public ViewModelLocation VMSelectedLocation
        {
            get { return _vmSelectedLocation; }
            set {
                _vmSelectedLocation = value;
                NotifyPropertyChanged("VMSelectedLocation");
            }
        }
        private ViewModelLocation _vmSelectedLocation { set; get; }
        /// <summary>
        /// List of all exits for selected location
        /// </summary>
        public List<ViewModelExit> VMExits
        {
            get { return _vmExits; }
            set { _vmExits = value; NotifyPropertyChanged("VMExits"); }
        }
        private List<ViewModelExit> _vmExits { set; get; }
        /// <summary>
        /// Selected exit view model
        /// </summary>
        public ViewModelExit VMSelectedExit
        {
            get { return _vmSelectedExit; }
            set
            {
                _vmSelectedExit = value;
                NotifyPropertyChanged("VMSelectedExit");
            }
        }
        private ViewModelExit _vmSelectedExit { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public LocationEditorPageViewModel(IContainer container) : base(container)
        {
            ItemService = container.Resolve<IItemService>();
        }
        /// <summary>
        /// Load all items with a location attribute
        /// </summary>
        /// <returns></returns>
        public async Task LoadLocations()
        {
            Working = true;
            try
            {
                if (_locations == null)
                {
                    _locations = await ItemService.ReadByTypeAsync(new List<string>() {
                        typeof(LocationAttribute).ToString()
                    });
                    var vmLocations = new List<ViewModelLocation>();
                    _locations.ForEach((location) =>
                    {
                        vmLocations.Add(new ViewModelLocation()
                        {
                            ItemId = location.Id.ToString(),
                            LocationId = location.GetAttribute<LocationAttribute>().Id.ToString(),
                            Name = location.Name,
                            Description = location.Description
                        });
                    });
                    VMLocations = vmLocations;
                }
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Load all exits for a given location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task LoadExits(ViewModelLocation location)
        {
            Working = true;
            try
            {
                var childrenIds = _locations
                    .Where(x => x.Id.ToString() == location.ItemId)
                    .SelectMany(x => x.ChildrenIds)
                    .ToList();
                var children = await ItemService.ReadAsync(childrenIds);
                VMExits = children
                    .Where(x => x.HasAttribute<ExitAttribute>())
                    .Select(x => new ViewModelExit()
                        {
                            ItemId = x.Id,
                            ExitId = x.GetAttribute<ExitAttribute>().Id,
                            Name = x.Name,
                            Description = x.Description
                        })
                    .ToList();
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
        public async Task UpdateSelectedLocation()
        {
            Working = true;
            try
            {
                var item = _locations
                    .Where(x => x.Id.ToString() == VMSelectedLocation.ItemId)
                    .FirstOrDefault();
                var location = item.GetAttribute<LocationAttribute>();
                location.Name = VMSelectedLocation.Name;
                location.Description = VMSelectedLocation.Description;
                await ItemService.UpdateAsync(new List<Item>() { item });
            }
            finally
            {
                Working = false;
            }
        }
    }
}
