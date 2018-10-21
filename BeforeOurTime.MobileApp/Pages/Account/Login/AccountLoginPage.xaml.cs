using Autofac;
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
            BindingContext = ViewModel = new AccountLoginPageViewModel(container);
        }


    }
}