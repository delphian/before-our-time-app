using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.AccountEditor.Backup;
using BeforeOurTime.MobileApp.Pages.Admin.AccountEditor.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.AccountEditor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountEditorPage : TabbedPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public AccountEditorPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            this.Children.Add(new AccountEditorListPage(Container) { Title = "List" });
            this.Children.Add(new AccountEditorBackupPage(Container) { Title = "Backup" });
        }
    }
}