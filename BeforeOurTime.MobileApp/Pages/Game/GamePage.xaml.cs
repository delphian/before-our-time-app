using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.Location;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.ReadLocationSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Game
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public GamePageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public GamePage (IContainer container)
		{
			InitializeComponent();
            Container = container;
            MessageService = container.Resolve<IMessageService>();
            BindingContext = ViewModel = new GamePageViewModel(Container);
        }
        public async void ButtonWest_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.UseExit("west");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        public async void ButtonEast_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.UseExit("east");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        public async void ButtonNorth_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.UseExit("north");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        public async void ButtonSouth_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.UseExit("south");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        /// <summary>
        /// Edit current location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonLocationEdit_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var itemId = ViewModel.Location.Id;
                var locationEditorPage = new LocationEditorPage(Container);
                locationEditorPage.ViewModel.PreSelectLocation = itemId;
                locationEditorPage.Disappearing += (disSender, disE) =>
                {
                    MessageService.Send(new WorldReadLocationSummaryRequest() { });
                };
                await Navigation.PushModalAsync(locationEditorPage);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        /// <summary>
        /// Create new location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonLocationCreate_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.CreateFromCurrentLocation();
                MessageService.Send(new WorldReadLocationSummaryRequest() { });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        /// <summary>
        /// Create new generic item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemCreate_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.CreateGenericItem();
                MessageService.Send(new WorldReadLocationSummaryRequest() { });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        public async void ButtonInventory_OnClicked(object sender, EventArgs e)
        {
            try
            {
                ViewModel.ShowInventory = !ViewModel.ShowInventory;
                ViewModel.LocationItems = ViewModel.LocationItems.ToList();
                ViewModel.Inventory.Items = ViewModel.Inventory.Items.ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        /// <summary>
        /// Perform an emote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void EmotePicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.VMEmotes.PerformSelectedEmote();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        /// <summary>
        /// Perform an emote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ItemCommandPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var itemCommand = (VMItemCommand)((Picker)sender).SelectedItem;
                if (itemCommand != null)
                {
                    if (itemCommand.Name == "* Edit JSON")
                    {
                        var itemId = itemCommand.Item.Id;
                        var jsonEditorPage = new JsonEditorPage(Container);
                        jsonEditorPage.ViewModel.ItemId = itemId.ToString();
                        await jsonEditorPage.ViewModel.ReadItem();
                        jsonEditorPage.Disappearing += (disSender, disE) =>
                        {
                            MessageService.Send(new WorldReadLocationSummaryRequest() { });
                            ((Picker)sender).SelectedItem = null;
                        };
                        await Navigation.PushModalAsync(jsonEditorPage);
                    }
                    else
                    {
                        await ViewModel.VMItemCommands.PerformSelectedCommand();
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}