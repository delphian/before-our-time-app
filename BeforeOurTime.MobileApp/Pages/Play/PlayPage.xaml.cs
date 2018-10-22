using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin;
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
        }
        /// <summary>
        /// Naviate to new page when menu item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PlayPageMenuItem;
            if (item == null)
                return;
            if (item.TargetType == null)
            {
                await Navigation.PopAsync();
            }
            else
            {
                var page = (Page)Activator.CreateInstance(item.TargetType, Container);
                Title = item.Title;
                Detail = new NavigationPage(page);
                IsPresented = false;
            }
            MasterPage.ListView.SelectedItem = null;
        }
    }
}