using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.AccountEditor.Backup
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountEditorBackupPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        private VMAccountEditorBackupPage ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public AccountEditorBackupPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new VMAccountEditorBackupPage(container);
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
                ViewModel.Working = true;
                await ViewModel.LoadJsonAccounts();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Unable to load item json backup from server. " + ex.Message, "Curses!");
            }
            finally
            {
                ViewModel.Working = false;
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