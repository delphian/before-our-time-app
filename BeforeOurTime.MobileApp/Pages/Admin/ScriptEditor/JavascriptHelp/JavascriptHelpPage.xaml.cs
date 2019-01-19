using Autofac;
using BeforeOurTime.MobileApp.Services.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.ScriptEditor.JavascriptHelp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JavascriptHelpPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container"></param>
        public JavascriptHelpPage (IContainer container)
		{
			InitializeComponent ();
            Container = container;
            BackgroundColor = Color.FromHex(Container.Resolve<IStyleService>().GetTemplate().GetPage(StyleType.Primary).BackgroundColor);
        }
        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}