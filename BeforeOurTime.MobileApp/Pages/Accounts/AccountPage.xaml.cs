using Autofac;
using BeforeOurTime.MobileApp.Pages.Accounts.Characters.Create;
using BeforeOurTime.MobileApp.Pages.Accounts.Characters.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Accounts
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
            this.Children.Add(new SelectCharacterPage(Container) { Title = "Select Character" });
            this.Children.Add(new CreateCharacterPage(Container) { Title = "Create Character" });
        }
    }
}