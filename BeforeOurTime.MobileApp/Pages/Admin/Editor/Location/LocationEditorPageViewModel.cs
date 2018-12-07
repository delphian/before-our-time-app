using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.MobileApp.Services.Loggers;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.CreateLocation;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.DeleteLocation;
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
        /// Record errors and information during program execution
        /// </summary>
        private ILoggerService LoggerService { set; get; }
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        private IItemService ItemService { set; get; }
        /// <summary>
        /// Manage IMessage messages between client and server
        /// </summary>
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// List of all locations 
        /// </summary>
        private List<Item> _locations { set; get; }
        /// <summary>
        /// Specify to automatically load one of the locations into editor
        /// </summary>
        public Guid? PreSelectLocation { set; get; }
        /// <summary>
        /// List of all locations as view models
        /// </summary>
        public List<VMLocation> VMLocations
        {
            get { return _vmLocations; }
            set { _vmLocations = value; NotifyPropertyChanged("VMLocations"); }
        }
        private List<VMLocation> _vmLocations { set; get; } = new List<VMLocation>();
        /// <summary>
        /// Selected location view model
        /// </summary>
        public VMLocation VMSelectedLocation
        {
            get { return _vmSelectedLocation; }
            set {
                _vmSelectedLocation = value;
                NotifyPropertyChanged("VMSelectedLocation");
            }
        }
        private VMLocation _vmSelectedLocation { set; get; }
        /// <summary>
        /// Indicate if a location is currently selected in the editor
        /// </summary>
        public bool LocationSelected
        {
            get { return _locationSelected; }
            set { _locationSelected = value; NotifyPropertyChanged("LocationSelected"); }
        }
        private bool _locationSelected { set; get; } = false; 
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public LocationEditorPageViewModel(IContainer container) : base(container)
        {
            LoggerService = container.Resolve<ILoggerService>();
            ItemService = container.Resolve<IItemService>();
            MessageService = container.Resolve<IMessageService>();
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
                    _locations = await ItemService.ReadByDataTypeAsync(new List<string>() {
                        typeof(LocationItemData).ToString()
                    });
                    var vmLocations = new List<VMLocation>();
                    _locations.ForEach((location) =>
                    {
                        vmLocations.Add(new VMLocation()
                        {
                            ItemId = location.Id.ToString(),
                            LocationId = location.GetData<LocationItemData>().Id.ToString(),
                            Name = location.GetData<VisibleItemData>().Name,
                            Description = location.GetData<VisibleItemData>().Description
                        });
                    });
                    VMLocations = vmLocations;
                }
                if (PreSelectLocation != null)
                {
                    VMSelectedLocation = VMLocations
                        .Where(x => x.ItemId == PreSelectLocation.Value.ToString())
                        .FirstOrDefault();
                    LocationSelected = true;
                    PreSelectLocation = null;
                }
            }
            catch (Exception e)
            {
                LoggerService.Log("Unable to load locations", e);
                throw e;
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
                var visibleItemData = item.GetData<VisibleItemData>();
                visibleItemData.Name = VMSelectedLocation.Name;
                visibleItemData.Description = VMSelectedLocation.Description;
                await ItemService.UpdateAsync(new List<Item>() { item });
            }
            catch (Exception e)
            {
                LoggerService.Log("Unable to update location", e);
                throw e;
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Create new location and link through exits to existing selected location
        /// </summary>
        public async Task CreateFromSelectedLocation()
        {
            Working = true;
            try
            {
                Guid.TryParse(VMSelectedLocation.ItemId, out Guid fromLocationItemId);
                var result = await MessageService
                    .SendRequestAsync<WorldCreateLocationResponse>(new WorldCreateLocationQuickRequest()
                    {
                        FromLocationItemId = fromLocationItemId
                    });
                if (!result.IsSuccess())
                {
                    throw new Exception(result._responseMessage);
                }
                _locations.Add(result.CreateLocationEvent.Item);
                var vmLocations = VMLocations;
                vmLocations.Add(new VMLocation()
                {
                    ItemId = result.CreateLocationEvent.Item.Id.ToString(),
                    LocationId = result.CreateLocationEvent.Item.GetData<LocationItemData>().Id.ToString(),
                    Name = result.CreateLocationEvent.Item.GetProperty<VisibleItemProperty>().Name,
                    Description = result.CreateLocationEvent.Item.GetProperty<VisibleItemProperty>().Description
                });
                (VMLocations = new List<VMLocation>()).AddRange(vmLocations);
            }
            catch (Exception e)
            {
                LoggerService.Log("Unable to create location", e);
                throw e;
            }
            finally
            {
                Working = false;
            }
        }
        /// <summary>
        /// Delete currently selected location
        /// </summary>
        public async Task DeleteSelectedLocation()
        {
            Working = true;
            try
            {
                Guid.TryParse(VMSelectedLocation.ItemId, out Guid locationItemId);
                var result = await MessageService
                    .SendRequestAsync<WorldDeleteLocationResponse>(new WorldDeleteLocationRequest()
                    {
                        LocationItemId = locationItemId
                    });
                if (!result.IsSuccess())
                {
                    throw new Exception(result._responseMessage);
                }
                _locations.Remove(result.DeleteItemEvent.Items.FirstOrDefault());
                var vmLocations = VMLocations;
                vmLocations.Remove(VMSelectedLocation);
                (VMLocations = new List<VMLocation>()).AddRange(vmLocations);
                VMSelectedLocation = null;
                LocationSelected = false;
            }
            catch (Exception e)
            {
                LoggerService.Log("Unable to delete location", e);
                throw e;
            }
            finally
            {
                Working = false;
            }
        }
    }
}
