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
using BeforeOurTime.Models.Modules.World.Models.Items;
using BeforeOurTime.Models.Modules.World.Messages.Location.ReadLocationSummary;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Messages.UseItem;
using BeforeOurTime.Models.Modules.Core.Messages.MoveItem;
using BeforeOurTime.MobileApp.Controls;
using System.Windows.Input;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using System.Threading.Tasks;
using BeforeOurTime.Models.Modules.World.Models.Data;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.World.Messages.Emotes;
using BeforeOurTime.Models.Modules.World.Messages.Emotes.PerformEmote;

namespace BeforeOurTime.MobileApp.Pages.Game
{
    public class GamePageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Structure that subscriber must implement to recieve property updates
        /// </summary>
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// Player's character
        /// </summary>
        private Item Me { set; get; }
        /// <summary>
        /// Current location item
        /// </summary>
        public Item Location
        {
            get { return _location; }
            set { _location = value; NotifyPropertyChanged("Location"); }
        }
        private Item _location { set; get; }
        public int ExitElements
        {
            get { return _exitElements; }
            set { _exitElements = value; NotifyPropertyChanged("ExitElements"); }
        }
        private int _exitElements { set; get; }
        /// <summary>
        /// Short name of the current location
        /// </summary>
        public string LocationName {
            get { return _locationName; }
            set { _locationName = value; NotifyPropertyChanged("LocationName"); }
        }
        private string _locationName { set; get; } = "Before Our Time";
        /// <summary>
        /// Long description of the current location
        /// </summary>
        public string LocationDescription
        {
            get { return _locationDescription; }
            set { _locationDescription = value; NotifyPropertyChanged("LocationDescription"); }
        }
        private string _locationDescription { set; get; }
        /// <summary>
        /// All items that offer an exit
        /// </summary>
        public List<ExitItem> Exits
        {
            get { return _exits; }
            set { _exits = value; NotifyPropertyChanged("Exits"); }
        }
        private List<ExitItem> _exits { set; get; } = new List<ExitItem>();
        /// <summary>
        /// All objects (dumb items) at current location
        /// </summary>
        public List<Item> Objects
        {
            get { return _objects; }
            set { _objects = value; NotifyPropertyChanged("Objects"); }
        }
        private List<Item> _objects { set; get; } = new List<Item>();
        /// <summary>
        /// Character items at current location
        /// </summary>
        public List<CharacterItem> Characters
        {
            get { return _characters; }
            set { _characters = value; NotifyPropertyChanged("Characters"); }
        }
        private List<CharacterItem> _characters { set; get; } = new List<CharacterItem>();
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
        public VisibleProperty SelectedVisibleProperty
        {
            get { return _selectedVisibleProperty; }
            set { _selectedVisibleProperty = value; NotifyPropertyChanged("SelectedVisibleProperty"); }
        }
        private VisibleProperty _selectedVisibleProperty { set; get; }
        /// <summary>
        /// Selected item commands
        /// </summary>
        public List<BeforeOurTime.Models.Modules.Core.Models.Properties.Command> Commands
        {
            get { return _commands; }
            set { _commands = value; NotifyPropertyChanged("Commands"); }
        }
        private List<BeforeOurTime.Models.Modules.Core.Models.Properties.Command> _commands { set; get; }
        /// <summary>
        /// Last message recieved from server in it's raw format
        /// </summary>
        public EventStreamVM EventStream
        {
            get { return _eventStream; }
            set { _eventStream = value; NotifyPropertyChanged("EventStream"); }
        }
        private EventStreamVM _eventStream { set; get; } = new EventStreamVM();
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
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public GamePageViewModel(IContainer container)
        {
            Container = container;
            Me = Container.Resolve<ICharacterService>().GetCharacter();
            _vmEmotes = new VMEmotes(Container);
            ExitElements = Convert.ToInt32(Math.Floor(Application.Current.MainPage.Width / 200));
            var GameService = container.Resolve<IGameService>();
            GameService.GetLocationSummary().ContinueWith((summaryTask) =>
            {
                ProcessListLocationResponse(summaryTask.Result.GetMessageAsType<WorldReadLocationSummaryResponse>());
            });
            GameService.OnMessage += OnMessage;
            LocationItemTable_OnClicked = new Xamarin.Forms.Command((object itemTableControl) =>
            {
                var control = (ItemTableControl)itemTableControl;
                SelectedItem = control?.SelectedItem;
                IsItemSelected = (SelectedItem != null);
                SelectedVisibleProperty = SelectedItem?.GetProperty<VisibleProperty>();
                Commands = SelectedItem?.GetProperty<CommandProperty>()?.Commands;
            });
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
                if (moveItemEvent.NewParent.Id == Location.Id)
                {
                    ProcessArrivalEvent(moveItemEvent);
                }
                if (moveItemEvent.OldParent.Id == Location.Id)
                {
                    ProcessDepartureEvent(moveItemEvent);
                }
            }
            else if (message.IsMessageType<WorldEmoteEvent>())
            {
                var emoteEvent = message.GetMessageAsType<WorldEmoteEvent>();
                var visible = emoteEvent.Origin?.GetProperty<VisibleProperty>();
                if (visible != null)
                {
                    var emote = "does something unexpected!";
                    if (emoteEvent.EmoteType == WorldEmoteType.Smile)
                        emote = "smiles happily";
                    if (emoteEvent.EmoteType == WorldEmoteType.Frown)
                        emote = "frowns in consternation";
                    EventStream.Push($"{visible.Name} {emote}");
                }
            }
            else if (message.IsMessageType<WorldPerformEmoteResponse>())
            {
                // Sit on it
            }
            else
            {
                EventStream.Push(message.GetMessageName());
            }
        }
        public async Task UseExit(string exitName)
        {
            var goGuid = new Guid("c558c1f9-7d01-45f3-bc35-dcab52b5a37c");
            var test = Objects.Where(x => x.GetProperty<CommandProperty>() != null).ToList();
            var item = Objects.Where(x => x.GetProperty<CommandProperty>() != null)?
                   .Where(x => x.GetProperty<CommandProperty>().Commands
                       .Any(y => y.Id == goGuid && x.GetProperty<VisibleProperty>().Name.ToLower().Contains(exitName.ToLower())))
                   .FirstOrDefault();
            var itemCommand = item?.GetProperty<CommandProperty>()?.Commands?
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
            await Container.Resolve<IMessageService>().SendAsync(useRequest);
        }
        private void ProcessListLocationResponse(WorldReadLocationSummaryResponse listLocationResponse)
        {
            SelectedItem = null;
            IsItemSelected = false;
            Location = listLocationResponse.Item;
            LocationName = listLocationResponse.Item.Visible.Name;
            LocationDescription = listLocationResponse.Item.Visible.Description;
            Characters = listLocationResponse.Characters;
            Objects = listLocationResponse.Items;
            ProcessExits(listLocationResponse);
        }
        private void ProcessExits(WorldReadLocationSummaryResponse listLocationResponse)
        {
            Exits = listLocationResponse.Exits.Select(x => x.Item.GetAsItem<ExitItem>()).ToList();
        }
        private void ProcessArrivalEvent(CoreMoveItemEvent arrivalEvent)
        {
            if (arrivalEvent.Item.Id != Me.Id)
            {
                var name = arrivalEvent.Item.GetProperty<VisibleProperty>()?.Name ?? "**Unknown**";
                EventStream.Push($"{name} has arrived");
                Objects.Add(arrivalEvent.Item.GetAsItem<CharacterItem>());
                // Force notify to fire
                Objects = Objects.ToList();
            }
        }
        private void ProcessDepartureEvent(CoreMoveItemEvent departureEvent)
        {
            if (departureEvent.Item.Id != Me.Id)
            {
                var name = departureEvent.Item.GetProperty<VisibleProperty>()?.Name ?? "**Unknown**";
                EventStream.Push($"{name} has departed");
                Objects.Remove(Objects
                    .Where(x => x.Id == departureEvent.Item.Id)
                    .Select(x => x)
                    .FirstOrDefault());
                // Force notify to fire
                Objects = Objects.ToList();
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
