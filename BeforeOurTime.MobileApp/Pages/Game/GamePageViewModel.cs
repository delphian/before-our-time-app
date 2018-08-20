using Autofac;
using BeforeOurTime.MobileApp.Services.Games;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Items.Attributes.Players;
using BeforeOurTime.Models.Items.Attributes.Characters;
using BeforeOurTime.Models.Messages;
using BeforeOurTime.Models.Messages.Events.Arrivals;
using BeforeOurTime.Models.Messages.Events.Departures;
using BeforeOurTime.Models.Messages.Responses.List;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Characters;

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
        private int _exitElements { set; get; }
        public int ExitElements
        {
            get { return _exitElements; }
            set { _exitElements = value; NotifyPropertyChanged("ExitElements"); }
        }
        private string _locationName { set; get; } = "Before Our Time";
        /// <summary>
        /// Short name of the current location
        /// </summary>
        public string LocationName {
            get { return _locationName; }
            set { _locationName = value; NotifyPropertyChanged("LocationName"); }
        }
        private string _locationDescription { set; get; }
        /// <summary>
        /// Long description of the current location
        /// </summary>
        public string LocationDescription
        {
            get { return _locationDescription; }
            set { _locationDescription = value; NotifyPropertyChanged("LocationDescription"); }
        }
        private List<Item> _exits { set; get; } = new List<Item>();
        /// <summary>
        /// All items that offer an exit
        /// </summary>
        public List<Item> Exits
        {
            get { return _exits; }
            set { _exits = value; NotifyPropertyChanged("Exits"); }
        }
        private List<Item> _objects { set; get; } = new List<Item>();
        /// <summary>
        /// All objects (dumb items) at current location
        /// </summary>
        public List<Item> Objects
        {
            get { return _objects; }
            set { _objects = value; NotifyPropertyChanged("Objects"); }
        }
        private List<Item> _characters { set; get; } = new List<Item>();
        /// <summary>
        /// Character items at current location
        /// </summary>
        public List<Item> Characters
        {
            get { return _characters; }
            set { _characters = value; NotifyPropertyChanged("Characters"); }
        }
        private string _eventStream { set; get; }
        /// <summary>
        /// Last message recieved from server in it's raw format
        /// </summary>
        public string EventStream
        {
            get { return _eventStream; }
            set { _eventStream = value; NotifyPropertyChanged("EventStream"); }
        }
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
                ProcessListLocationResponse(summaryTask.Result.GetMessageAsType<ListLocationResponse>());
            });
            GameService.OnMessage += OnMessage;
        }
        /// <summary>
        /// Listen to unprompted incoming messages (events)
        /// </summary>
        /// <param name="message"></param>
        private void OnMessage(IMessage message)
        {
            EventStream = DateTime.Now + " " + message.GetMessageName();
            if (message.IsMessageType<ListLocationResponse>())
            {
                ProcessListLocationResponse(message.GetMessageAsType<ListLocationResponse>());
            }
            if (message.IsMessageType<ArrivalEvent>())
            {
                ProcessArrivalEvent(message.GetMessageAsType<ArrivalEvent>());
            }
            if (message.IsMessageType<DepartureEvent>())
            {
                ProcessDepartureEvent(message.GetMessageAsType<DepartureEvent>());
            }
        }
        private void ProcessListLocationResponse(ListLocationResponse listLocationResponse)
        {
            LocationName = listLocationResponse.Item.Name;
            LocationDescription = listLocationResponse.Item.Description;
            Objects = listLocationResponse.Objects;
            Characters = listLocationResponse.Characters;
            ProcessExits(listLocationResponse);
        }
        private void ProcessExits(ListLocationResponse listLocationResponse)
        {
            Exits = listLocationResponse.Exits.Select(x => x.Item).ToList();
        }
        private void ProcessArrivalEvent(ArrivalEvent arrivalEvent)
        {
            if ((arrivalEvent.Item.HasAttribute<CharacterAttribute>() ||
                arrivalEvent.Item.HasAttribute<PlayerAttribute>()) &&
                arrivalEvent.Item.Id != Me.Id)
            {
                Characters.Add(arrivalEvent.Item);
                // Force notify to fire
                Characters = Characters.ToList();
            }
            else
            {
                Objects.Add(arrivalEvent.Item);
            }
        }
        private void ProcessDepartureEvent(DepartureEvent departureEvent)
        {
            if ((departureEvent.Item.HasAttribute<CharacterAttribute>() ||
                departureEvent.Item.HasAttribute<PlayerAttribute>()) &&
                departureEvent.Item.Id != Me.Id)
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
