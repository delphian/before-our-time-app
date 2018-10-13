using Autofac;
using BeforeOurTime.Models.Modules.World.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Accounts.Characters.Select
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectCharacterPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public SelectCharacterViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public SelectCharacterPage(IContainer container)
		{
            InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new SelectCharacterViewModel(Container);
        }
        /// <summary>
        /// Get all characters for logged in account
        /// </summary>
        /// <param name="force">Force update from server</param>
        private async Task GetAccountCharacters(bool force = false)
        {
            try
            {
                await ViewModel.GetAccountCharacters(force);
            }
            catch (Exception e)
            {
                await Task.Delay(1000);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // If called from constructor allow UI time to display.
                    await DisplayAlert("Error", e.Message, "OK");
                });
            }
        }
        /// <summary>
        /// Load 
        /// </summary>
        protected override async void OnAppearing()
        {
            await GetAccountCharacters(true);
        }
        /// <summary>
        /// Open game page when play button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonPlay_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Play.PlayPage(Container));
        }
        /// <summary>
        /// Select which character is desired for attachment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void CharacterListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                await ViewModel.PlayAccountCharacter((CharacterItem)CharacterListView.SelectedItem);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}