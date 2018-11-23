using Autofac;
using BeforeOurTime.MobileApp.Controls;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Modules.Core.Messages.UseItem;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.World.Messages.Location.CreateLocation;
using BeforeOurTime.Models.Modules.World.Models.Items;
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
                await ViewModel.VMItemCommands.PerformSelectedCommand();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
}