using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.AccountEditor.List
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountEditorListPage : ContentPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        private VMAccountEditorListPage ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
		public AccountEditorListPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new VMAccountEditorListPage(container);
        }
        /// <summary>
        /// Refresh list of accounts from server each time page appears
        /// </summary>
        protected override async void OnAppearing()
        {
            try
            {
                ViewModel.Working = true;
                await ViewModel.ReadAccounts();
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "Unable to load accounts server. " + e.Message, "Sadness");
            }
            finally
            {
                ViewModel.Working = false;
            }
        }
        /// <summary>
        /// Select a different account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void AccountListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (((ListView)sender).SelectedItem != null)
                {
                    ViewModel.AccountList.ForEach(character =>
                    {
                        character.IsSelected = false;
                    });
                    ((VMAccountEntry)AccountListView.SelectedItem).IsSelected = true;
                    ((ListView)sender).SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        /// <summary>
        /// Delete an account and all associated characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                ViewModel.Working = true;
                await ViewModel.DeleteSelectedAccount();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                ViewModel.Working = false;
            }
        }
    }
}