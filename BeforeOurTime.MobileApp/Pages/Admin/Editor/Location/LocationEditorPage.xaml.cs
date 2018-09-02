using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.Exit;
using BeforeOurTime.MobileApp.Services.Loggers;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Messages.Locations.CreateLocation;
using BeforeOurTime.Models.Messages.Locations.Locations.CreateLocation;
using Plugin.Clipboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Location
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LocationEditorPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public LocationEditorPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        /// <param name="itemId">When provided automatically load this item for editing</param>
        public LocationEditorPage (IContainer container)
		{
			InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new LocationEditorPageViewModel(container);
        }
        /// <summary>
        /// Refresh list of locations from server each time page appears
        /// </summary>
        protected override async void OnAppearing()
        {
            try
            {
                await ViewModel.LoadLocations();
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "Unable to load locations from server. " + e.Message, "Sadness");
            }
        }
        /// <summary>
        /// For now, do nothing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void LocationPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (LocationPicker.SelectedItem != null)
            {
                await ViewModel.LoadExits(LocationPicker.SelectedItem as ViewModelLocation);
                ViewModel.LocationSelected = true;
            }
        }
        /// <summary>
        /// Switch to exit editor after exit is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExitPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ExitPicker.SelectedItem != null)
            {
                var itemId = ViewModel.VMSelectedExit.ItemId;
                MessagingCenter.Send<ContentPage, Guid>(this, "ExitEditorPage:Load", itemId);
                ((TabbedPage)this.Parent).CurrentPage = ((TabbedPage)this.Parent).Children
                    .Where(x => x.GetType() == typeof(ExitEditorPage))
                    .First();
            }
        }
        /// <summary>
        /// Update location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonUpdate_Clicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.UpdateSelectedLocation();
                await DisplayAlert("Success", "Location has been saved", "Good!");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Unsable to save location: " + ex.Message, "Sigh...");
            }
        }
        /// <summary>
        /// Load current item into json editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonJson_Clicked(object sender, EventArgs e)
        {
            Guid.TryParse(ViewModel.VMSelectedLocation.ItemId, out Guid itemId);
            MessagingCenter.Send<ContentPage, Guid>(this, "CRUDEditorPage:Load", itemId);
            ((TabbedPage)this.Parent).CurrentPage = ((TabbedPage)this.Parent).Children
                .Where(x => x.GetType() == typeof(JsonEditorPage))
                .First();
        }
        /// <summary>
        /// Add a default location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonAddLocation_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.CreateFromSelectedLocation();
                await DisplayAlert("Success", "Done!", "OK, Thank You");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Unable to create location", "OK, But Why?");
            }
        }
        /// <summary>
        /// Delete currently selected location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            IsBusy = true;
            try
            {
                await ViewModel.DeleteSelectedLocation();
                await DisplayAlert("Success", "Done!", "OK, Thank You");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Unable to delete location", "OK, But Why?");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}