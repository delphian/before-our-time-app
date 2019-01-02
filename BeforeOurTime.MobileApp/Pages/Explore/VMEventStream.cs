using BeforeOurTime.Models.Messages;
using BeforeOurTime.Models.Messages.Systems.Ping;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Messages.ItemCrud.CreateItem;
using BeforeOurTime.Models.Modules.Core.Messages.ItemCrud.DeleteItem;
using BeforeOurTime.Models.Modules.Core.Messages.ItemJson.ReadItemJson;
using BeforeOurTime.Models.Modules.Core.Messages.ItemJson.UpdateItemJson;
using BeforeOurTime.Models.Modules.Core.Messages.MoveItem;
using BeforeOurTime.Models.Modules.Core.Messages.UseItem;
using BeforeOurTime.Models.Modules.World.ItemProperties.Characters;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.ReadLocationSummary;
using BeforeOurTime.Models.Modules.World.Messages.Emotes;
using BeforeOurTime.Models.Modules.World.Messages.Emotes.PerformEmote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Explore
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
        }
        /// <summary>
        /// Deconstruct an IMessage into a text message
        /// </summary>
        /// <param name="message">IMessage to deconstruct into a simple text message</param>
        /// <param name="vmGamePage">View model of explore page</param>
        public VMEventStream OnIMessage(IMessage message, VMExplorePage vmExplorePage)
        {
            if (message.IsMessageType<WorldEmoteEvent>())
            {
                var messageEvent = message.GetMessageAsType<WorldEmoteEvent>();
                var visible = messageEvent.Origin?.GetProperty<VisibleItemProperty>();
                if (visible != null)
                {
                    var emote = "does something unexpected!";
                    if (messageEvent.EmoteType == WorldEmoteType.Smile)
                        emote = "smiles happily";
                    if (messageEvent.EmoteType == WorldEmoteType.Frown)
                        emote = "frowns in consternation";
                    Push($"{visible.Name} {emote}");
                }
            }
            else if (message.IsMessageType<CoreMoveItemEvent>()) {
                var messageEvent = message.GetMessageAsType<CoreMoveItemEvent>();
                if (messageEvent.Item.Id != vmExplorePage.Me.Id)
                {
                    bool arrival = (messageEvent.NewParent.Id == vmExplorePage.VMLocation.Item.Id);
                    var what = messageEvent.Item.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
                    var text = "";
                    if (arrival)
                    {
                        var who = messageEvent.OldParent.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
                        text = (messageEvent.OldParent.HasProperty<CharacterItemProperty>()) ?
                            $"{who} has dropped {what}" :
                            $"{what} has arrived";
                    }
                    else
                    {
                        var who = messageEvent.NewParent.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
                        text = (messageEvent.NewParent.HasProperty<CharacterItemProperty>()) ?
                            $"{who} has taken {what}" :
                            $"{what} has departed";
                    }
                    Push(text);
                }
            }
            else if (message.IsMessageType<CoreUpdateItemJsonEvent>())
            {
                var messageEvent = message.GetMessageAsType<CoreUpdateItemJsonEvent>();
                var who = messageEvent.Origin?.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
                var what = messageEvent.Items?.First()?.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
                var streamMessage = $"{who} used the magic of JSON to change {what}";
                Push(streamMessage);
            }
            else if (message.IsMessageType<CoreCreateItemCrudEvent>())
            {
                var messageEvent = message.GetMessageAsType<CoreCreateItemCrudEvent>();
                var what = messageEvent.Item.GetProperty<VisibleItemProperty>()?.Name ??
                           "**Unknown**";
                var streamMessage = $"{what} materializes into existance";
                Push(streamMessage);
            }
            else if (message.IsMessageType<CoreDeleteItemCrudEvent>())
            {
                var messageEvent = message.GetMessageAsType<CoreDeleteItemCrudEvent>();
                var what = messageEvent.Items.First().GetProperty<VisibleItemProperty>()?.Name ??
                           "**Unknown**";
                var streamMessage = $"{what} disintegrates into the ether";
                Push(streamMessage);
            }
            else if (message.IsMessageType<CoreReadItemJsonEvent>())
            {
                var messageEvent = message.GetMessageAsType<CoreReadItemJsonEvent>();
                var who = messageEvent.Origin?.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
                var what = messageEvent.Items?.First()?.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
                var streamMessage = $"{who} interrogates {what} through the power of JSON magic";
                Push(streamMessage);
            }
            else if (message.IsMessageType<WorldPerformEmoteResponse>() ||
                     message.IsMessageType<CoreUseItemResponse>() ||
                     message.IsMessageType<CoreUpdateItemJsonResponse>() ||
                     message.IsMessageType<CoreReadItemJsonResponse>() ||
                     message.IsMessageType<WorldReadLocationSummaryResponse>() ||
                     message.IsMessageType<PingSystemMessage>())
            {
                // Sit on it
            }
            else
            {
                Push(message.GetMessageName());
            }
            return this;
        }
        /// <summary>
        /// Reset all events
        /// </summary>
        public void Clear()
        {
            Events.Clear();
            Output = BuildOutput(Events);
        }
        /// <summary>
        /// Remove last message and push another one
        /// </summary>
        /// <param name="message"></param>
        public void Push(string message)
        {
            if (Events.Count() > 5)
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
