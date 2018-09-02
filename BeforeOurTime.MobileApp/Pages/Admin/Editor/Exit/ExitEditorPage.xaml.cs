using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Exit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExitEditorPage : ContentPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public ExitEditorPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public ExitEditorPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new ExitEditorPageViewModel(Container);
            // Allow other pages to load items into the editor
            MessagingCenter.Subscribe<ContentPage, Guid>(this, "ExitEditorPage:Load", async (sender, guid) =>
            {
                await ViewModel.ReadItem(guid);
            });
        }
        /// <summary>
        /// Refresh list of exits from server each time page appears
        /// </summary>
        protected override async void OnAppearing()
        {
            try
            {
                await ViewModel.LoadExits();
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "Unable to load exits from server. " + e.Message, "Sadness");
            }
        }
        /// <summary>
        /// For now, do nothing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExitPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // For now, do nothing
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
                await ViewModel.UpdateSelectedExit();
                await DisplayAlert("Success", "Exit has been saved", "Good!");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Unsable to save exit: " + ex.Message, "Sigh...");
            }
        }
        /// <summary>
        /// Load current item into json editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonJson_Clicked(object sender, EventArgs e)
        {
            Guid.TryParse(ViewModel.VMSelectedExit.ItemId, out Guid itemId);
            MessagingCenter.Send<ContentPage, Guid>(this, "CRUDEditorPage:Load", itemId);
            ((TabbedPage)this.Parent).CurrentPage = ((TabbedPage)this.Parent).Children
                .Where(x => x.GetType() == typeof(JsonEditorPage))
                .First();
        }
    }
}