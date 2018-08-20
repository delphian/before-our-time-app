using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : TabbedPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public EditorPage (IContainer container)
        {
            InitializeComponent();
            Container = container;
            this.Children.Add(new LocationEditorPage(Container) { Title = "Locations" });
            this.Children.Add(new CRUDEditorPage(Container) { Title = "JSON Editor" });
        }
    }
}