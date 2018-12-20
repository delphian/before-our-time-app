using BeforeOurTime.MobileApp.Pages.Explore;
using BeforeOurTime.MobileApp.UWP.Pages.Explore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ExplorePage), typeof(CustomExplorePageRenderer))]
namespace BeforeOurTime.MobileApp.UWP.Pages.Explore
{
    public class CustomExplorePageRenderer : PageRenderer
    {
        /// <summary>
        /// Monitor windows core ui keypress when ExplorePage is showing
        /// </summary>
        public CustomExplorePageRenderer() : base()
        {
            // When ExplorePage appears then attach to the ui core keydown event
            Loaded += (object sender, RoutedEventArgs e) =>
            {
                Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown += HandleKeyDown;
            };
            // When ExplorePage disappears then detach from the ui core keydown event
            Unloaded += (object sender, RoutedEventArgs e) =>
            {
                Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown -= HandleKeyDown;
            };
        }
        /// <summary>
        /// Forward a keypress to an ExplorePage method
        /// </summary>
        /// <param name="window"></param>
        /// <param name="e"></param>
        public void HandleKeyDown(Windows.UI.Core.CoreWindow window, Windows.UI.Core.KeyEventArgs e)
        {
            (Element as ExplorePage).KeyPressed(Element, new KeyEventArgs { Key = e.VirtualKey.ToString() });
        }
    }
}
