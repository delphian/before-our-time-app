﻿using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Debug;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Characters;
using BeforeOurTime.MobileApp.Services.Games;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.MobileApp.Services.WebSockets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Server
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ServerPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public ServerPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public ServerPage (IContainer container)
		{
			InitializeComponent ();
            Container = container;
            BindingContext = ViewModel = new ServerPageViewModel(Container);
        }
        /// <summary>
        /// Automatically connect when page appearing event fires
        /// </summary>
        protected override async void OnAppearing()
        {
            try
            {
                ViewModel.Working = true;
                await ViewModel.OnAppearing();
                //await ViewModel.ConnectAsync();
                //await ViewModel.LoginAsync();
                //await ViewModel.SelectCharacterAsync();
                //await Navigation.PushAsync(new Play.PlayPage(Container));
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", $"Can't connect to server: {e.Message}", "Fail!");
            }
            finally
            {
                ViewModel.Working = false;
            }
        }
        /// <summary>
        /// For now, do nothing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ServerPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // For now, do nothing
        }
        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonConnect_OnClicked(object sender, EventArgs e)
        {
            try
            {
                ViewModel.Working = true;
                var connectionString = ServerPicker.Items[ServerPicker.SelectedIndex];
                if (connectionString != null)
                {
                    await ViewModel.ConnectAsync(connectionString);
                    await ViewModel.LoginAsync();
                    await ViewModel.SelectCharacterAsync();
                    var masterPage = new Play.PlayPage(Container)
                    {
                        Detail = new NavigationPage(new Game.GamePage(Container)),
                        IsPresented = false
                    };
                    await Navigation.PushAsync(masterPage);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Can't connect to server: {ex.Message}", "Fail!");
            }
            finally
            {
                ViewModel.Working = false;
            }
        }
        /// <summary>
        /// Navigate to the account login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonExplore_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login.LoginPage(Container));
        }
        /// <summary>
        /// Disconnect from server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonDisonnect_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.DisconnectAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK, but sad...");
            }
        }
        /// <summary>
        /// Navigate to the error log page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonErrorLog_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DebugPage(Container));
        }
        /// <summary>
        /// Clear all cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonClearCache_OnClicked(object sender, EventArgs e)
        {
            await Container.Resolve<IWebSocketService>().Clear();
            await Container.Resolve<IMessageService>().Clear();
            await Container.Resolve<IAccountService>().Clear();
            await Container.Resolve<ICharacterService>().Clear();
            await Container.Resolve<IGameService>().Clear();
            Application.Current.Properties.Clear();
            await Application.Current.SavePropertiesAsync();
            ViewModel.Settings = new Models.Settings();
            Application.Current.Properties.Add("Settings", JsonConvert.SerializeObject(ViewModel.Settings));
            await Application.Current.SavePropertiesAsync();

            await DisplayAlert("Warning", "Cache has been reset", "OK");
        }
    }
}