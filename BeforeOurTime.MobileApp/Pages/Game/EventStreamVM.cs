using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Game
{
    /// <summary>
    /// View model and logic for controlling the text stream
    /// </summary>
    public class EventStreamVM : BotViewModel, System.ComponentModel.INotifyPropertyChanged
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
        /// Constructor
        /// </summary>
        public EventStreamVM()
        {

        }
        /// <summary>
        /// Remove last message and push another one
        /// </summary>
        /// <param name="message"></param>
        public void Push(string message)
        {
            if (Events.Count() > 2)
            {
                Events.Remove(Events.OrderBy(x => x.Time).First());
            }
            Events.Add(new EventMessageVM()
            {
                Time = DateTime.Now,
                Message = DateTime.Now.ToString("hh:mm:ss") + " " + message
            });
            // Force notify property changed to fire
            Events = Events.ToList();
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
