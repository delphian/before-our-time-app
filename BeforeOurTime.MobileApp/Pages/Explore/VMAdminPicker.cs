using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.Location;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Messages.ItemCrud.CreateItem;
using BeforeOurTime.Models.Modules.Core.Models.Data;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.CreateLocation;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.ReadLocationSummary;
using BeforeOurTime.Models.Modules.World.ItemProperties.Physicals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Pages.Explore
{
    /// <summary>
    /// Manage state changes for admin picker control
    /// </summary>
    public class VMAdminPicker : BotViewModel
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// Page on which the control is located
        /// </summary>
        private Page Page { set; get; }
        /// <summary>
        /// View model for current location
        /// </summary>
        private VMLocation VMLocation { set; get; }
        /// <summary>
        /// Message service
        /// </summary>
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// Display control
        /// </summary>
        public bool Visible
        {
            set { _visible = value; NotifyPropertyChanged("Visible"); }
            get { return _visible; }
        }
        private bool _visible { set; get; }
        /// <summary>
        /// Admin commands
        /// </summary>
        public List<string> Source
        {
            set { _source = value; NotifyPropertyChanged("Source"); }
            get { return _source; }
        }
        private List<string> _source { set; get; }
        /// <summary>
        /// Selected source element
        /// </summary>
        public string Selected
        {
            set { _selected = value; NotifyPropertyChanged("Selected"); }
            get { return _selected; }
        }
        private string _selected { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public VMAdminPicker(IContainer container, Page page, VMLocation vmLocation)
        {
            Container = container;
            MessageService = container.Resolve<IMessageService>();
            Page = page;
            VMLocation = vmLocation;
            Source = new List<string>()
            {
                "Edit Location",
                "Create Location",
                "Create Item"
            };
        }
        /// <summary>
        /// Perform an admin command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async Task OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Selected == "Edit Location")
                {
                    var itemId = VMLocation.Item.Id;
                    var jsonEditorPage = new JsonEditorPage(Container);
                    jsonEditorPage.ViewModel.ItemId = itemId.ToString();
                    jsonEditorPage.Disappearing += (disSender, disE) =>
                    {
                        MessageService.Send(new WorldReadLocationSummaryRequest() { });
                    };
                    await Page.Navigation.PushModalAsync(jsonEditorPage);
                }
                else if (Selected == "Create Location")
                {
                    await CreateFromCurrentLocation();
                    MessageService.Send(new WorldReadLocationSummaryRequest() { });
                }
                else if (Selected == "Create Item")
                {
                    await CreateGenericItem();
                    MessageService.Send(new WorldReadLocationSummaryRequest() { });
                }
                Selected = null;
            }
            catch (Exception ex)
            {
                await Page.DisplayAlert("Error", ex.Message, "Ok");
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
    }
}
