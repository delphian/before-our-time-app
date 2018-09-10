using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Debug.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Debug
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DebugPage : TabbedPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public DebugPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            this.Children.Add(new DebugErrorPage(Container) { Title = "Errors" });
        }
    }
}