using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Account.Login.Update
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateLoginPage : ContentPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public UpdateLoginPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="account">Account to update</param>
        public UpdateLoginPage(
            IContainer container,
            BeforeOurTime.Models.Modules.Account.Models.Account account)
        {
            InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new UpdateLoginPageViewModel(container, account);
        }
        /// <summary>
        /// Register for permanent account and close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                ViewModel.Working = true;
                await ViewModel.Register();
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to register: {ex.Message}", "Heck Darn!");
            }
            finally
            {
                ViewModel.Working = false;
            }
        }
        /// <summary>
        /// Update account and close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                ViewModel.Working = true;
                await ViewModel.UpdatePasswordAsync();
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to update: {ex.Message}", "Heck Darn!");
            }
            finally
            {
                ViewModel.Working = false;
            }
        }
        /// <summary>
        /// Close modal without saving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            ViewModel.Cancel();
            await Navigation.PopModalAsync();
        }
    }
}
