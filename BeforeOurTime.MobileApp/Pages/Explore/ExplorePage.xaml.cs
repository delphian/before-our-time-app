using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Explore
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExplorePage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public VMExplorePage ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public ExplorePage (IContainer container)
		{
			InitializeComponent();
            Container = container;
            BindingContext = ViewModel = new VMExplorePage(Container, this);
        }
        /// <summary>
        /// Key has been pressed (N,S,E,W, etc...)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async Task KeyPressed(object sender, KeyEventArgs e)
        {
            if (new List<String>() { "n", "s", "e", "w" }.Contains(e.Key.ToLower()))
            {
                await ViewModel.UseExitByDirection(e.Key);
            }
        }
        /// <summary>
        /// Perform an emote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void EmotePicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.VMEmotes.PerformSelectedEmote();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
    }
    public class KeyEventArgs : EventArgs
    {
        public string Key { get; set; }
    }
}