﻿using Autofac;
using BeforeOurTime.MobileApp.Pages.Account.Login.Update;
using BeforeOurTime.MobileApp.Services.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Account.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountLoginPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public AccountLoginPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
		public AccountLoginPage (IContainer container)
		{
			InitializeComponent ();
            Container = container;
            BackgroundColor = Color.FromHex(Container.Resolve<IStyleService>().GetTemplate().GetPage(StyleType.Primary).BackgroundColor);
            BindingContext = ViewModel = new AccountLoginPageViewModel(container);
        }
        /// <summary>
        /// Load list of cached accounts when page appears
        /// </summary>
        protected override async void OnAppearing()
        {
            ViewModel.Working = true;
            await GetAccounts(true);
            ViewModel.Working = false;
        }
        /// <summary>
        /// Get all cached accounts
        /// </summary>
        /// <param name="force">Force update from server</param>
        public async Task GetAccounts(bool force = false)
        {
            try
            {
                await ViewModel.GetAccounts(force);
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", e.Message, "OK");
            }
        }
        /// <summary>
        /// Select a new account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void AccountListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (((ListView)sender).SelectedItem != null)
                {
                    ViewModel.Accounts.ForEach(accountEntry =>
                    {
                        accountEntry.IsSelected = false;
                    });
                    ((AccountListEntryVM)AccountListView.SelectedItem).IsSelected = true;
                    ((ListView)sender).SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        /// <summary>
        /// Open account update modal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            var updateLoginPage = new UpdateLoginPage(
                Container,
                ViewModel.Accounts.Where(x => x.IsSelected == true).Select(x => x.Account).First());
            await Navigation.PushModalAsync(updateLoginPage);
        }
    }
}