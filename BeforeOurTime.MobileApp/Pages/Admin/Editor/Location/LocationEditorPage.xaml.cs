using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD;
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
        public void LocationPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // For now, do nothing
        }
        /// <summary>
        /// Copy item id to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Clicked(object sender, EventArgs e)
        {
            CrossClipboard.Current.SetText(ViewModel.VMSelectedLocation.ItemId);
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
            MessagingCenter.Send<ContentPage, string>(this, "Load", ViewModel.VMSelectedLocation.ItemId);
            var masterPage = (TabbedPage)this.Parent;
            masterPage.CurrentPage = masterPage.Children.Where(x => x.GetType() == typeof(CRUDEditorPage)).First();
        }
    }
}