using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Graph
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphEditorPage : ContentPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public GraphEditorPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public GraphEditorPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new GraphEditorPageViewModel(container, this);
        }
        /// <summary>
        /// Refresh list of locations from server each time page appears
        /// </summary>
        protected override async void OnAppearing()
        {
            try
            {
                await ViewModel.LoadItemGraphAsync();
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "Unable to load item graph. " + e.Message, "Sadness");
            }
        }
    }
}