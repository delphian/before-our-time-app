using Autofac;
using BeforeOurTime.MobileApp.Pages.Account.Character.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Account.Character
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountCharacterPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public AccountCharacterPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
		public AccountCharacterPage (IContainer container)
		{
			InitializeComponent ();
            Container = container;
            BindingContext = ViewModel = new AccountCharacterPageViewModel(container);
        }
        /// <summary>
        /// Load list of characters when page appears
        /// </summary>
        protected override async void OnAppearing()
        {
            await GetAccountCharacters(true);
        }
        /// <summary>
        /// Get all characters for logged in account
        /// </summary>
        /// <param name="force">Force update from server</param>
        public async Task GetAccountCharacters(bool force = false)
        {
            try
            {
                await ViewModel.GetAccountCharacters(force);
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message, "OK");
            }
        }
        /// <summary>
        /// Select a new character to play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void CharacterListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (((ListView)sender).SelectedItem != null)
                {
                    ViewModel.Characters.ForEach(character =>
                    {
                        character.IsSelected = false;
                    });
                    ((VMAccountCharacterEntry)CharacterListView.SelectedItem).IsSelected = true;
                    ((ListView)sender).SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        /// <summary>
        /// Open character update modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            var updateLoginPage = new UpdateCharacterPage(
                Container,
                ViewModel.Characters.Where(x => x.IsSelected == true).Select(x => x.CharacterItem).First());
            await Navigation.PushModalAsync(updateLoginPage);
        }
    }
}