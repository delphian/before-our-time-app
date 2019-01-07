using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.JsonEditor.DataTypes;
using BeforeOurTime.MobileApp.Pages.Admin.ScriptEditor;
using BeforeOurTime.MobileApp.Services.Styles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Script.ItemProperties.Javascripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.JsonEditor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JsonEditorPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public VMJsonEditorPage ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="itemId">When provided automatically load this item for editing</param>
		public JsonEditorPage (IContainer container, Guid? itemId = null)
		{
			InitializeComponent ();
            Container = container;
            BackgroundColor = Color.FromHex(Container.Resolve<IStyleService>().GetTemplate().GetPage(StyleType.Primary).BackgroundColor);
            BindingContext = ViewModel = new VMJsonEditorPage(container, itemId);
            // Allow other page to load items into the json editor
            MessagingCenter.Subscribe<ContentPage, Guid>(this, "CRUDEditorPage:Load", async (sender, guid) =>
            {
                try
                {
                    ViewModel.ItemId = guid.ToString();
                    await ViewModel.ReadItem();
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error", e.Message, "OK, Maybe Tomorrow");
                }
            });
        }
        public JsonEditorPage(IContainer container) : this(container, null) { }
        /// <summary>
        /// Load an item if one has been pre-selected
        /// </summary>
        protected override async void OnAppearing()
        {
            try
            {
                if (ViewModel.PreLoad && ViewModel.ItemId != null)
                {
                    await ViewModel.ReadItem();
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "Unable to load item: " + e.Message, "Sadness");
            }
        }
        /// <summary>
        /// Load script editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonScriptEditor_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var item = JsonConvert.DeserializeObject<Item>(ViewModel.ItemJson);
                if (item.GetData<JavascriptItemData>() is JavascriptItemData data)
                {
                    var scriptEditorPage = new ScriptEditorPage(Container);
                    scriptEditorPage.ViewModel.SetScript(data.Script);
                    scriptEditorPage.Disappearing += (disSender, disE) =>
                    {
                        data.Script = scriptEditorPage.ViewModel.GetScript();
                        ViewModel.CoreItemJson.JSON = ViewModel.ItemJson = JsonConvert.SerializeObject(item, Formatting.Indented);
                    };
                    ViewModel.PreLoad = false;
                    await Navigation.PushModalAsync(scriptEditorPage);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK, Maybe Tomorrow");
            }
        }
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
        /// Cancel and close page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        /// <summary>
        /// Read an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemCreate_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.CreateItem();
                await DisplayAlert("Success", "Items have been created", "Yay!");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK, Maybe Tomorrow");
            }
        }
        /// <summary>
        /// Open new page containing snippets of data type json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonDataTypes_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new JsonDataTypesPage());
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
    }
}