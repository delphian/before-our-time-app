using Autofac;
using BeforeOurTime.MobileApp.Controls;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.Location;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using BeforeOurTime.Models.Modules.World.ItemProperties.Locations.Messages.ReadLocationSummary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Explore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExplorePage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public VMExplorePage ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public ExplorePage (IContainer container)
		{
			InitializeComponent();
            Container = container;
            MessageService = container.Resolve<IMessageService>();
            BindingContext = ViewModel = new VMExplorePage(Container, this);
        }
        /// <summary>
        /// Toggle between inventory and location items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonInventory_OnClicked(object sender, EventArgs e)
        {
            try
            {
                ViewModel.ShowInventory = !ViewModel.ShowInventory;
                ViewModel.VMGroundItems.Items = ViewModel.VMGroundItems.Items.ToList();
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
    }
}