using Autofac;
using BeforeOurTime.MobileApp.Pages.Account.Character;
using BeforeOurTime.MobileApp.Pages.Account.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountPage : TabbedPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public AccountPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            this.Children.Add(new AccountLoginPage(Container) { Title = "Login" });
            this.Children.Add(new AccountCharacterPage(Container) { Title = "Characters" });
        }
    }
}