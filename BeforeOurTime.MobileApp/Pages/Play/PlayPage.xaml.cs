using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Play
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayPage : MasterDetailPage
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public PlayPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(Game.GamePage), Container));
            //Detail = (Page)Activator.CreateInstance(typeof(Game.GamePage), Container);
            //var detailPage = new NavigationPage(new Game.GamePage(Container));
            Detail = new NavigationPage(new Game.GamePage(Container));
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PlayPageMenuItem;
            if (item == null)
                return;
            if (item.Id == 3)
            {
                Navigation.PopAsync();
            } else
            {
                var page = (Page)Activator.CreateInstance(item.TargetType, Container);
                page.Title = item.Title;
                Detail = new NavigationPage(page);
                IsPresented = false;
            }
            MasterPage.ListView.SelectedItem = null;
        }
    }
}