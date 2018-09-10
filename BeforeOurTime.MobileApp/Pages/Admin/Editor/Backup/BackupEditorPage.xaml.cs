using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Backup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BackupEditorPage : ContentPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        private BackupEditorPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public BackupEditorPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new BackupEditorPageViewModel(container);
        }
        /// <summary>
        /// Automatic retrieval of all items would be expensive. Use a button instead
        /// </summary>
        protected override void OnAppearing()
        {
        }
        /// <summary>
        /// Retrieve JSON backup of all items from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonLoad_Clicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.LoadJsonItems();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Unable to load item json backup from server. " + ex.Message, "Curses!");
            }
        }
        /// <summary>
        /// Copy json items string into system clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonCopy_Clicked(object sender, EventArgs e)
        {
            ViewModel.CopyToClipboard();
            await DisplayAlert("Success", "Copied to clipboard", "Carry On");
        }
    }
}