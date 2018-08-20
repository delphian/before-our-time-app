using Autofac;
using BeforeOurTime.MobileApp.Services.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Accounts.Characters.Create
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateCharacterPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public CreateCharacterViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
		public CreateCharacterPage (IContainer container)
		{
            InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new CreateCharacterViewModel(Container);
		}
        /// <summary>
        /// Create a character
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonCreate_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.CreateAccountCharacterAsync();
                await Navigation.PopAsync();
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}