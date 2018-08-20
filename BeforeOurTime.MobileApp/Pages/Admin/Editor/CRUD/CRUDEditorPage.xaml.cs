using Autofac;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Messages.Requests.LocationAttributes;
using BeforeOurTime.Models.Messages.Responses.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CRUDEditorPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public CRUDEditorPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="itemId">When provided automatically load this item for editing</param>
		public CRUDEditorPage (IContainer container, Guid? itemId = null)
		{
			InitializeComponent ();
            Container = container;
            BindingContext = ViewModel = new CRUDEditorPageViewModel(container, itemId);
            // Allow other page to load items into the json editor
            MessagingCenter.Subscribe<ContentPage, string>(this, "Load", async (sender, guid) =>
            {
                ViewModel.ItemId = guid;
                await ViewModel.ReadItem();
            });
		}
        public CRUDEditorPage(IContainer container) : this(container, null) { }
        /// <summary>
        /// Read an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemRead_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.ReadItem();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK, Maybe Tomorrow");
            }
        }
        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemUpdate_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.UpdateItem();
                await DisplayAlert("Success", "Item has been saved", "Very Well");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "I Did Not Consent!");
            }
        }
        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemDelete_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.DeleteItem();
                await DisplayAlert("Success", "Item has been deleted", "Very Well");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "I Did Not Consent!");
            }
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
                var result = await Container.Resolve<IMessageService>().SendRequestAsync<ListLocationResponse>(
                    new CreateLocationQuickRequest()
                    {
                    });
                if (!result.IsSuccess())
                {
                    throw new Exception("Unable to create location");
                }
                await DisplayAlert("Success", "Done!", "OK, Thank You");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Unable to create location", "OK, But Why?");
            }
        }
    }
}