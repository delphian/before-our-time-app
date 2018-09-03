using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPageDetail : ContentPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public AdminPageDetail(IContainer container)
        {
            InitializeComponent();
            Container = container;
        }
    }
}