using Autofac;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.World.Messages.Emotes;
using BeforeOurTime.Models.Modules.World.Messages.Emotes.PerformEmote;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Explore
{

    /// <summary>
    /// View model and logic for sending emotes
    /// </summary>
    public class VMEmotes : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// List of possible emotes
        /// </summary>
        public List<VMEmote> Emotes
        {
            get { return _emotes; }
            set { _emotes = value; NotifyPropertyChanged("Emotes"); }
        }
        private List<VMEmote> _emotes { set; get; } = new List<VMEmote>();
        /// <summary>
        /// Selected emote from list
        /// </summary>
        public VMEmote SelectedEmote
        {
            get { return _selectedEmote; }
            set { _selectedEmote = value; NotifyPropertyChanged("SelectedEmote"); }
        }
        private VMEmote _selectedEmote { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public VMEmotes(IContainer container)
        {
            Container = container;
            MessageService = Container.Resolve<IMessageService>();
            Emotes.Add(new VMEmote() { Name = "Smile", WorldEmoteType = WorldEmoteType.Smile });
            Emotes.Add(new VMEmote() { Name = "Frown", WorldEmoteType = WorldEmoteType.Frown });
        }
        /// <summary>
        /// Perform the selected emote
        /// </summary>
        /// <returns></returns>
        public async Task PerformSelectedEmote()
        {
            if (SelectedEmote != null)
            {
                var performEmoteRequest = new WorldPerformEmoteRequest()
                {
                    EmoteType = SelectedEmote.WorldEmoteType
                };
                await MessageService.SendAsync(performEmoteRequest);
                SelectedEmote = null;
            }
        }
    }
    /// <summary>
    /// A single emote
    /// </summary>
    public class VMEmote : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Name of emote
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        private string _name { set; get; }
        /// <summary>
        /// Type of emote
        /// </summary>
        public WorldEmoteType WorldEmoteType
        {
            get { return _worldEmoteType; }
            set { _worldEmoteType = value; NotifyPropertyChanged("WorldEmoteType"); }
        }
        private WorldEmoteType _worldEmoteType { set; get; }
    }
}
