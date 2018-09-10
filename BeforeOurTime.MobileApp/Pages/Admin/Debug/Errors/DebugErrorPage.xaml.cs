using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Debug.Errors
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DebugErrorPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public DebugErrorPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public DebugErrorPage (IContainer container)
		{
			InitializeComponent ();
            Container = container;
            BindingContext = ViewModel = new DebugErrorPageViewModel(container);
		}
        /// <summary>
        /// Refresh list of locations from server each time page appears
        /// </summary>
        protected override async void OnAppearing()
        {
            try
            {
                await ViewModel.LoadLogs();
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "Unable to load logs. " + e.Message, "Dispair!");
            }
        }

    }
}