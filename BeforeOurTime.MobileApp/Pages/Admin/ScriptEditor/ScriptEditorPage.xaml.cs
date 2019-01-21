using Autofac;
using BeforeOurTime.MobileApp.Services.Styles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.ScriptEditor
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScriptEditorPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public VMScriptEditorPage ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public ScriptEditorPage (IContainer container)
		{
            InitializeComponent();
            Container = container;
            BackgroundColor = Color.FromHex(Container.Resolve<IStyleService>().GetTemplate().GetPage(StyleType.Primary).BackgroundColor);
            BindingContext = ViewModel = new VMScriptEditorPage(container);
        }
        private void UpdateButton_Clicked(object sender, EventArgs e)
        {

        }
        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        /// <summary>
        /// For now, do nothing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CodePicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            ViewModel.InsertSnippet(ViewModel.CodeName);
        }
    }
}