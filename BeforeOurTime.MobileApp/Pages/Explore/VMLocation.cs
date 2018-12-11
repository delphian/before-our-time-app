using Autofac;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Exceptions;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Messages.UseItem;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using BeforeOurTime.Models.Modules.World.ItemProperties.Exits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Pages.Explore
{
    /// <summary>
    /// View model for location
    /// </summary>
    public class VMLocation : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// Location item
        /// </summary>
        public Item Item { set; get; }
        /// <summary>
        /// Title of location
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        private string _name { set; get; }
        /// <summary>
        /// Description of location
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged("Description"); }
        }
        private string _description { set; get; }
        /// <summary>
        /// Description of location including items present and hypertext
        /// </summary>
        public FormattedString DescriptionFormatted
        {
            get { return _descriptionFormatted; }
            set { _descriptionFormatted = value; NotifyPropertyChanged("DescriptionFormatted"); }
        }
        private FormattedString _descriptionFormatted { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public VMLocation(IContainer container)
        {
            Container = container;
        }
        /// <summary>
        /// Update view model to reflect new location
        /// </summary>
        /// <param name="item">location item</param>
        /// <param name="children">Children items at location</param>
        /// <returns></returns>
        public VMLocation Set(Item location, List<Item> children = null)
        {
            Item = location;
            Name = Item.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
            Description = Item.GetProperty<VisibleItemProperty>()?.Description ?? "**Unknown**";
            DescriptionFormatted = new FormattedString();
            DescriptionFormatted.Spans.Add(new Span() { Text = Description });
            children?.ForEach(child =>
            {
                if (child.HasProperty<VisibleItemProperty>())
                {
                    var visible = child.GetProperty<VisibleItemProperty>();
                    var name = visible?.Name ?? "**Unknown**";
                    var desc = visible?.Description ?? "**Something hidden is here**";
                    DescriptionFormatted.Spans.Add(new Span()
                    {
                        Text = $". "
                    });
                    var span = new Span();
                        span.Text = $"{name}";
                        span.TextColor = Color.LightBlue;
                        span.TextDecorations = TextDecorations.Underline;
                        span.GestureRecognizers.Add(new TapGestureRecognizer()
                        {
                            CommandParameter = child,
                            Command = new Command<Item>(async (item) =>
                            {
                                await TextClickCommand(item);
                            }),
                        });
                    DescriptionFormatted.Spans.Add(span);
                    DescriptionFormatted.Spans.Add(new Span()
                    {
                        Text = $" {desc}"
                    });
                }
            });
            return this;
        }
        /// <summary>
        /// Inline text item has been clicked
        /// </summary>
        public async Task TextClickCommand(Item item)
        {
            var goGuid = new Guid("c558c1f9-7d01-45f3-bc35-dcab52b5a37c");
            var goCommand = item?.GetProperty<CommandItemProperty>()?.Commands?
                   .Where(x => x.Id == goGuid)
                   .FirstOrDefault();
            if (goCommand != null)
            {
                var useRequest = new CoreUseItemRequest()
                {
                    ItemId = item.Id,
                    Use = goCommand
                };
                var response = await Container.Resolve<IMessageService>().SendRequestAsync<CoreUseItemResponse>(useRequest);
                if (!response.IsSuccess())
                {
                    throw new BeforeOurTimeException(response._responseMessage);
                }
            }
        }
    }
}
