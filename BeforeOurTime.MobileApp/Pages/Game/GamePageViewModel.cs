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
        /// Last message recieved from server in it's raw format
        /// </summary>
        public string EventStream
        {
            get { return _eventStream; }
            set { _eventStream = value; NotifyPropertyChanged("EventStream"); }
        }
        private string _eventStream { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public GamePageViewModel(IContainer container)
        {
            Container = container;
            Me = Container.Resolve<ICharacterService>().GetCharacter();
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
            });
        }
        /// <summary>
        /// Create the item table
        /// </summary>
        /// <param name="mainContent"></param>
        public void CreateItemTable(View mainContent)
        {
            var iconTable = new FlexLayout();
                iconTable.HeightRequest = 50;
                iconTable.Wrap = FlexWrap.Wrap;
            Characters.ForEach(character =>
            {
                iconTable.Children.Add(new ItemButtonControl()
                {
                    Image = null,
                    Name = character.Visible.Name,
                    Description = character.Visible.Description,
                    ImageDefault = "character",
                    MinimumWidthRequest = 200,
                    HeightRequest = 50
                });
            });
            ((FlexLayout)mainContent).Children.Add(iconTable);
            FlexLayout.SetBasis(iconTable, new FlexBasis(0.25f, true));
            FlexLayout.SetGrow(iconTable, 1);
            FlexLayout.SetOrder(iconTable, 0);
        }
        /// <summary>
        /// Listen to unprompted incoming messages (events)
        /// </summary>
        /// <param name="message"></param>
        private void OnMessage(IMessage message)
        {
            EventStream = DateTime.Now + " " + message.GetMessageName();
            if (message.IsMessageType<WorldReadLocationSummaryResponse>())
            {
                ProcessListLocationResponse(message.GetMessageAsType<WorldReadLocationSummaryResponse>());
            }
            if (message.IsMessageType<CoreMoveItemEvent>())
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
        }
        private void ProcessListLocationResponse(WorldReadLocationSummaryResponse listLocationResponse)
        {
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
            if (arrivalEvent.Item is CharacterItem && arrivalEvent.Item.Id != Me.Id)
            {
                Characters.Add(arrivalEvent.Item.GetAsItem<CharacterItem>());
                // Force notify to fire
                Characters = Characters.ToList();
            }
            else
            {
                Objects.Add(arrivalEvent.Item);
            }
        }
        private void ProcessDepartureEvent(CoreMoveItemEvent departureEvent)
        {
            if (departureEvent.Item is CharacterItem && departureEvent.Item.Id != Me.Id)
            {
                Characters.Remove(Characters
                    .Where(x => x.Id == departureEvent.Item.Id)
                    .Select(x => x)
                    .FirstOrDefault());
                // Force notify to fire
                Characters = Characters.ToList();
            }
            else
            {
                Objects.Remove(departureEvent.Item);
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
