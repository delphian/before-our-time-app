using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.MobileApp.Services.Loggers;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.World.Messages.Location.CreateLocation;
using BeforeOurTime.Models.Modules.World.Messages.Location.DeleteLocation;
using BeforeOurTime.Models.Modules.World.Models.Data;
using BeforeOurTime.Models.Modules.World.Models.Items;
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
        private List<LocationItem> _locations { set; get; }
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
            set { _vmSelectedExit = value; NotifyPropertyChanged("VMSelectedExit"); }
        }
        private ViewModelExit _vmSelectedExit { set; get; }
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
                    var items = await ItemService.ReadByTypeAsync(new List<string>() {
                        typeof(LocationItem).ToString()
                    });
                    _locations = items.Select(x => x.GetAsItem<LocationItem>()).ToList();
                    var vmLocations = new List<ViewModelLocation>();
                    _locations.ForEach((location) =>
                    {
                        vmLocations.Add(new ViewModelLocation()
                        {
                            ItemId = location.Id.ToString(),
                            LocationId = location.GetData<LocationData>().Id.ToString(),
                            Name = location.Visible.Name,
                            Description = location.Visible.Description
                        });
                    });
                    VMLocations = vmLocations;
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
        /// Load all exits for a given location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public async Task LoadExits(ViewModelLocation location)
        {
            Working = true;
            try
            {
                if (location == null)
                {
                    throw new Exception("Invalid location");
                }
                var childrenIds = _locations
                    .Where(x => x.Id.ToString() == location.ItemId)
                    .SelectMany(x => x.ChildrenIds)
                    .ToList();
                var children = await ItemService.ReadAsync(childrenIds);
                VMExits = children?
                    .Where(x => x.Type == ItemType.Exit)
                    .Select(x => x.GetAsItem<ExitItem>())
                    .ToList()
                    .Select(x => new ViewModelExit()
                        {
                            ItemId = x.Id,
                            ExitId = x.GetData<ExitData>().Id,
                            Name = ((ExitItem)x).Visible.Name,
                            Description = ((ExitItem)x).Visible.Description
                        })
                    .ToList();
            }
            catch (Exception e)
            {
                LoggerService.Log("Unable to load exits", e);
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
                var location = item.GetData<LocationData>();
                location.Name = VMSelectedLocation.Name;
                location.Description = VMSelectedLocation.Description;
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
                    .SendRequestAsync<WorldCreateLocationQuickResponse>(new WorldCreateLocationQuickRequest()
                    {
                        FromLocationItemId = fromLocationItemId
                    });
                if (!result.IsSuccess())
                {
                    throw new Exception(result._responseMessage);
                }
                _locations.Add(result.CreateLocationEvent.Item);
                var vmLocations = VMLocations;
                vmLocations.Add(new ViewModelLocation()
                {
                    ItemId = result.CreateLocationEvent.Item.Id.ToString(),
                    LocationId = result.CreateLocationEvent.Item.GetData<LocationData>().Id.ToString(),
                    Name = result.CreateLocationEvent.Item.Visible.Name,
                    Description = result.CreateLocationEvent.Item.Visible.Description
                });
                (VMLocations = new List<ViewModelLocation>()).AddRange(vmLocations);
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
                _locations.Remove(result.DeleteItemEvent.Items.FirstOrDefault().GetAsItem<LocationItem>());
                var vmLocations = VMLocations;
                vmLocations.Remove(VMSelectedLocation);
                (VMLocations = new List<ViewModelLocation>()).AddRange(vmLocations);
                VMSelectedLocation = null;
                VMSelectedExit = null;
                VMExits = new List<ViewModelExit>();
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
