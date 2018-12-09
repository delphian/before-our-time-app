using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Game
{
    /// <summary>
    /// View model and logic for controlling the text stream
    /// </summary>
    public class VMEventStream : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// List of text events to be displayed
        /// </summary>
        public List<EventMessageVM> Events
        {
            get { return _events; }
            set { _events = value; NotifyPropertyChanged("Events"); }
        }
        private List<EventMessageVM> _events { set; get; } = new List<EventMessageVM>();
        /// <summary>
        /// All text events as a single string suitable for output
        /// </summary>
        public string Output
        {
            get { return _output; }
            set { _output = value; NotifyPropertyChanged("Output"); }
        }
        private string _output { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public VMEventStream()
        {
            Push("Logos...");
        }
        /// <summary>
        /// Remove last message and push another one
        /// </summary>
        /// <param name="message"></param>
        public void Push(string message)
        {
            if (Events.Count() > 6)
            {
                Events.Remove(Events.OrderBy(x => x.Time).First());
            }
            Events.Add(new EventMessageVM()
            {
                Time = DateTime.Now,
                Message = message
            });
            // Force notify property changed to fire
            Events = Events.ToList();
            Output = BuildOutput(Events);
        }
        /// <summary>
        /// Build all events into a single string
        /// </summary>
        /// <param name="events"></param>
        private string BuildOutput(List<EventMessageVM> events)
        {
            var output = "";
            events?.ForEach(eventMessage => {
                output += $"{eventMessage.Time.ToString("hh:mm:ss")} {eventMessage.Message}\n";
            });
            return output;
        }
    }
    /// <summary>
    /// An individual event message
    /// </summary>
    public class EventMessageVM : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Time that message was recieved
        /// </summary>
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; NotifyPropertyChanged("Time"); }
        }
        private DateTime _time { set; get; }
        /// <summary>
        /// Message
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; NotifyPropertyChanged("Message"); }
        }
        private string _message { set; get; }
    }
}
