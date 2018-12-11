using Autofac;
using BeforeOurTime.MobileApp.Services.Games;
using BeforeOurTime.Models.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using BeforeOurTime.MobileApp.Services.Characters;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Messages.UseItem;
using BeforeOurTime.Models.Modules.Core.Messages.MoveItem;
using BeforeOurTime.MobileApp.Controls;
using System.Windows.Input;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using System.Threading.Tasks;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.World.Messages.Emotes;
using BeforeOurTime.Models.Modules.World.Messages.Emotes.PerformEmote;
using BeforeOurTime.Models.Modules.Account.Models.Data;
using BeforeOurTime.Models.Modules.Core.Messages.ItemCrud.CreateItem;
using BeforeOurTime.Models.Modules.Core.Models.Data;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.CreateLocation;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.ReadLocationSummary;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.World.ItemProperties.Physicals;
using BeforeOurTime.Models.Exceptions;
using BeforeOurTime.Models.Modules.Core.Messages.ItemCrud.ReadItem;

namespace BeforeOurTime.MobileApp.Pages.Explore
{
    public class VMExplorePage : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Structure that subscriber must implement to recieve property updates
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Depedency injection container
        /// </summary>
        public IContainer Container
        {
            get { return _container; }
            set { _container = value; NotifyPropertyChanged("Container"); }
        }
        private IContainer _container { set; get; }
        /// <summary>
        /// Message service
        /// </summary>
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// Player's character
        /// </summary>
        public Item Me { set; get; }
        /// <summary>
        /// Player's inventory
        /// </summary>
        public VMInventory Inventory
        {
            get { return _inventory; }
            set { _inventory = value; NotifyPropertyChanged("Inventory"); }
        }
        private VMInventory _inventory { set; get; }
        /// <summary>
        /// Show player inventory or location items
        /// </summary>
        public bool ShowInventory
        {
            get { return _showInventory; }
            set { _showInventory = value; NotifyPropertyChanged("ShowInventory"); }
        }
        private bool _showInventory { set; get; } = false;
        /// <summary>
        /// Current account has administrative permissions
        /// </summary>
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; NotifyPropertyChanged("IsAdmin"); }
        }
        private bool _isAdmin { set; get; }
        /// <summary>
        /// Current location view model
        /// </summary>
        public VMLocation VMLocation
        {
            get { return _vmLocation; }
            set { _vmLocation = value; NotifyPropertyChanged("VMLocation"); }
        }
        private VMLocation _vmLocation { set; get; }
        public int ExitElements
        {
            get { return _exitElements; }
            set { _exitElements = value; NotifyPropertyChanged("ExitElements"); }
        }
        private int _exitElements { set; get; }
        /// <summary>
        /// All items that offer an exit
        /// </summary>
        public List<Item> Exits
        {
            get { return _exits; }
            set { _exits = value; NotifyPropertyChanged("Exits"); }
        }
        private List<Item> _exits { set; get; } = new List<Item>();
        /// <summary>
        /// All objects (dumb items) at current location
        /// </summary>
        public List<Item> LocationItems
        {
            get { return _locationItems; }
            set { _locationItems = value; NotifyPropertyChanged("LocationItems"); }
        }
        private List<Item> _locationItems { set; get; } = new List<Item>();
        /// <summary>
        /// Character items at current location
        /// </summary>
        public List<Item> Characters
        {
            get { return _characters; }
            set { _characters = value; NotifyPropertyChanged("Characters"); }
        }
        private List<Item> _characters { set; get; } = new List<Item>();
        /// <summary>
        /// Callback when location item has been clicked
        /// </summary>
        public ICommand LocationItemTable_OnClicked
        {
            get { return _locationItemTable_OnClicked; }
            set { _locationItemTable_OnClicked = value; NotifyPropertyChanged("LocationItemTable_OnClicked"); }
        }
        private ICommand _locationItemTable_OnClicked { set; get; }
        /// <summary>
        /// Currently selected item at location
        /// </summary>
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; NotifyPropertyChanged("SelectedItem"); }
        }
        private Item _selectedItem { set; get; }
        /// <summary>
        /// An item is selected
        /// </summary>
        public bool IsItemSelected
        {
            get { return _isItemSelected; }
            set { _isItemSelected = value; NotifyPropertyChanged("IsItemSelected"); }
        }
        private bool _isItemSelected { set; get; }
        /// <summary>
        /// Visible property of currently selected item at location
        /// </summary>
        public VisibleItemProperty SelectedVisibleProperty
        {
            get { return _selectedVisibleProperty; }
            set { _selectedVisibleProperty = value; NotifyPropertyChanged("SelectedVisibleProperty"); }
        }
        private VisibleItemProperty _selectedVisibleProperty { set; get; }
        /// <summary>
        /// Selected item commands
        /// </summary>
        public List<ItemCommand> Commands
        {
            get { return _commands; }
            set { _commands = value; NotifyPropertyChanged("Commands"); }
        }
        private List<ItemCommand> _commands { set; get; }
        /// <summary>
        /// Last message recieved from server in it's raw format
        /// </summary>
        public VMEventStream EventStream
        {
            get { return _eventStream; }
            set { _eventStream = value; NotifyPropertyChanged("EventStream"); }
        }
        private VMEventStream _eventStream { set; get; } = new VMEventStream();
        /// <summary>
        /// View model for all possible emotes
        /// </summary>
        public VMEmotes VMEmotes
        {
            get { return _vmEmotes; }
            set { _vmEmotes = value; NotifyPropertyChanged("VMEmotes"); }
        }
        private VMEmotes _vmEmotes { set; get; }
        /// <summary>
        /// View model for all item commands
        /// </summary>
        public VMItemCommands VMItemCommands
        {
            get { return _vmItemCommands; }
            set { _vmItemCommands = value; NotifyPropertyChanged("VMItemCommands"); }
        }
        private VMItemCommands _vmItemCommands { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public VMExplorePage(IContainer container)
        {
            Container = container;
            Me = Container.Resolve<ICharacterService>().GetCharacter();
            VMLocation = new VMLocation(Container);
            Inventory = new VMInventory(Container);
            if (Me.ChildrenIds.Count() > 0)
            {
                Container.Resolve<IMessageService>().SendRequestAsync<CoreReadItemCrudResponse>(new CoreReadItemCrudRequest()
                {
                    ItemIds = Me.ChildrenIds
                }).ContinueWith(messageTask =>
                {
                    Inventory.Add(messageTask.Result.CoreReadItemCrudEvent.Items);
                });
            }
            MessageService = Container.Resolve<IMessageService>();
            IsAdmin = Me.GetData<AccountData>().Admin;
            VMEmotes = new VMEmotes(Container);
            ExitElements = Convert.ToInt32(Math.Floor(Application.Current.MainPage.Width / 200));
            var GameService = container.Resolve<IGameService>();
            GameService.OnMessage += OnMessage;
            LocationItemTable_OnClicked = new Xamarin.Forms.Command((object itemTableControl) =>
            {
                var control = (ItemTableControl)itemTableControl;
                SelectedItem = control?.SelectedItem;
                IsItemSelected = (SelectedItem != null);
                control.SetHighlight(SelectedItem);
                SelectedVisibleProperty = SelectedItem?.GetProperty<VisibleItemProperty>();
                VMItemCommands.ClearItemCommands();
                VMItemCommands.AddItemCommands(SelectedItem);
            });
            MessageService.SendRequestAsync<WorldReadLocationSummaryResponse>(new WorldReadLocationSummaryRequest() { });
        }
        /// <summary>
        /// Listen to unprompted incoming messages (events)
        /// </summary>
        /// <param name="message"></param>
        private void OnMessage(IMessage message)
        {
            if (message.IsMessageType<WorldReadLocationSummaryResponse>())
            {
                ProcessListLocationResponse(message.GetMessageAsType<WorldReadLocationSummaryResponse>());
            }
            else if (message.IsMessageType<CoreMoveItemEvent>())
            {
                var moveItemEvent = message.GetMessageAsType<CoreMoveItemEvent>();
                if (moveItemEvent.NewParent.Id == VMLocation.Item.Id)
                {
                    ProcessArrivalEvent(moveItemEvent);
                }
                if (moveItemEvent.OldParent.Id == VMLocation.Item.Id)
                {
                    ProcessDepartureEvent(moveItemEvent);
                }
            }
            // Output as text message into event stream
            EventStream.OnIMessage(message, this);
        }
        public async Task UseExit(string exitName)
        {
            var goGuid = new Guid("c558c1f9-7d01-45f3-bc35-dcab52b5a37c");
            var item = LocationItems.Where(x => x.GetProperty<CommandItemProperty>() != null)?
                   .Where(x => x.GetProperty<CommandItemProperty>().Commands
                       .Any(y => y.Id == goGuid && x.GetProperty<VisibleItemProperty>().Name.ToLower().Contains(exitName.ToLower())))
                   .FirstOrDefault();
            var itemCommand = item?.GetProperty<CommandItemProperty>()?.Commands?
                   .Where(x => x.Id == goGuid)
                   .FirstOrDefault();
            if (item == null || itemCommand == null)
            {
                throw new Exception("No such exit found");
            }
            var useRequest = new CoreUseItemRequest()
            {
                ItemId = item.Id,
                Use = itemCommand
            };
            var response = await Container.Resolve<IMessageService>().SendRequestAsync<CoreUseItemResponse>(useRequest);
            if (!response.IsSuccess())
            {
                throw new BeforeOurTimeException(response._responseMessage);
            }
        }
        /// <summary>
        /// Location has updated
        /// </summary>
        /// <param name="listLocationResponse"></param>
        private void ProcessListLocationResponse(WorldReadLocationSummaryResponse listLocationResponse)
        {
            SelectedItem = null;
            IsItemSelected = false;
            VMLocation.Set(listLocationResponse.Item, listLocationResponse.Items);
            Characters = listLocationResponse.Characters;
            LocationItems = listLocationResponse.Items;
            LocationItems = LocationItems.ToList();
            VMItemCommands = new VMItemCommands(Container, VMLocation.Item);
            ProcessExits(listLocationResponse);
        }
        private void ProcessExits(WorldReadLocationSummaryResponse listLocationResponse)
        {
            Exits = listLocationResponse.Exits.Select(x => x.Item).ToList();
        }
        private void ProcessArrivalEvent(CoreMoveItemEvent arrivalEvent)
        {
            if (arrivalEvent.Item.Id != Me.Id)
            {
                LocationItems.Add(arrivalEvent.Item);
                if (arrivalEvent.OldParent.Id == Me.Id)
                {
                    Inventory.Remove(new List<Item>() { arrivalEvent.Item });
                }
                // Force notify to fire
                LocationItems = LocationItems.ToList();
                Inventory.Items = Inventory.Items.ToList();
            }
        }
        private void ProcessDepartureEvent(CoreMoveItemEvent departureEvent)
        {
            if (departureEvent.Item.Id != Me.Id)
            {
                LocationItems.Remove(LocationItems
                    .Where(x => x.Id == departureEvent.Item.Id)
                    .Select(x => x)
                    .FirstOrDefault());
                if (departureEvent.NewParent.Id == Me.Id)
                {
                    Inventory.Add(new List<Item>() { departureEvent.Item });
                }
                // Force notify to fire
                LocationItems = LocationItems.ToList();
            }
        }
        /// <summary>
        /// Create new location and link through exits to current location
        /// </summary>
        public async Task CreateFromCurrentLocation()
        {
            try
            {
                var fromLocationItemId = VMLocation.Item.Id;
                var result = await MessageService
                    .SendRequestAsync<WorldCreateLocationResponse>(new WorldCreateLocationQuickRequest()
                    {
                        FromLocationItemId = fromLocationItemId
                    });
                if (!result.IsSuccess())
                {
                    throw new Exception(result._responseMessage);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Create new generic item at current location
        /// </summary>
        public async Task CreateGenericItem()
        {
            try
            {
                var fromLocationItemId = VMLocation.Item.Id;
                var result = await MessageService
                    .SendRequestAsync<CoreCreateItemCrudResponse>(new CoreCreateItemCrudRequest()
                    {
                        Item = new Item()
                        {
                            ParentId = fromLocationItemId,
                            Data = new List<IItemData>()
                            {
                                new VisibleItemData()
                                {
                                    Name = "New Item",
                                    Description = "New description"
                                },
                                new PhysicalItemData()
                                {
                                    Mobile = true,
                                    Weight = 0
                                }
                            }
                        }
                    });
                if (!result.IsSuccess())
                {
                    throw new Exception(result._responseMessage);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Notify all subscribers that a property has been updated
        /// </summary>
        /// <param name="propertyName">Name of public property that has changed</param>
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
    }
}
