using Autofac;
using BeforeOurTime.MobileApp.Controls;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Messages.Requests.Go;
using BeforeOurTime.Models.Modules.Core.Messages.UseItem;
using BeforeOurTime.Models.Modules.World.Messages.Location.CreateLocation;
using BeforeOurTime.Models.Modules.World.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Game
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public GamePageViewModel GamePageViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public GamePage (IContainer container)
		{
			InitializeComponent();
            Container = container;
            BindingContext = GamePageViewModel = new GamePageViewModel(Container);
        }
        /// <summary>
        /// Select an exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ScrollListView_ItemSelected(object sender, EventArgs e)
        {
            var exitItem = ((ExitItem)((ScrollListView)sender).SelectedItem);
            var useRequest = new CoreUseItemRequest()
            {
                ItemId = exitItem.Id,
                Use = exitItem.Uses.Uses.First()
            };
            await Container.Resolve<IMessageService>().SendAsync(useRequest);
        }
        /// <summary>
        /// Add a default location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonAddLocation_OnClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await Container.Resolve<IMessageService>()
                    .SendRequestAsync<WorldCreateLocationQuickResponse>(new WorldCreateLocationQuickRequest()
                    {
                    });
                if (!result.IsSuccess())
                {
                    throw new Exception("Unable to create location");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Unable to create location", "OK, But Why?");
            }
        }
        /// <summary>
        /// Select an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void CharactersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
        }
    }
}