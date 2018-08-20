using Autofac;
using BeforeOurTime.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private LoginPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
		public LoginPage (IContainer container)
		{
			InitializeComponent ();
            Container = container;
            BindingContext = ViewModel = new LoginPageViewModel(Container);
		}
        /// <summary>
        /// Create account on server with the credentials provided by the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonRegister_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.RegisterAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        /// <summary>
        /// Authenticate to the server with the credentials provided by the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonLogIn_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.LoginAsync();
            }
            catch (AuthenticationDeniedException)
            {
                await DisplayAlert("Error", "Bad Username or Password", "OK, I'll Try Again");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Luke, I am Your Father!");
            }
        }
        /// <summary>
        /// Log account out of server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonLogOut_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.LogoutAsync();
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Failed to logout. Forced logout!", "OK, But Don't Let It Happen Again");
            }
        }
        /// <summary>
        /// Exit back to server page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonDepart_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        /// <summary>
        /// Navigate to the character selection page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonChooseCharacter_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Accounts.AccountPage(Container));
        }
    }
}