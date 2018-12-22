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
//            var _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
//            _timer.Tick += ExamineControllerState;
            // When ExplorePage appears then attach to the ui core keydown event
            Loaded += (object sender, RoutedEventArgs e) =>
            {
                Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown += HandleKeyDown;
//                _timer.Start();
            };
            // When ExplorePage disappears then detach from the ui core keydown event
            Unloaded += (object sender, RoutedEventArgs e) =>
            {
                Windows.UI.Core.CoreWindow.GetForCurrentThread().KeyDown -= HandleKeyDown;
//                _timer.Stop();
            };
        }
        /// <summary>
        /// Forward a controller movement to the keypress handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExamineControllerState<TEventArgs>(object sender, TEventArgs e)
        {
            var gamepad = Windows.Gaming.Input.Gamepad.Gamepads.FirstOrDefault();
            if (gamepad != null)
            {
                var currentState = gamepad.GetCurrentReading();
                if (currentState.LeftThumbstickY >= 0.5)
                {
                    await HandleKeyInput("n");
                }
                if (currentState.LeftThumbstickY <= -0.5)
                {
                    await HandleKeyInput("s");
                }
                if (currentState.LeftThumbstickX >= 0.5)
                {
                    await HandleKeyInput("e");
                }
                if (currentState.LeftThumbstickX <= -0.5)
                {
                    await HandleKeyInput("w");
                }
            }
        }
        /// <summary>
        /// Forward keyboard key press to the key input handler
        /// </summary>
        /// <param name="window"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public async void HandleKeyDown(Windows.UI.Core.CoreWindow window, Windows.UI.Core.KeyEventArgs e)
        {
            var key = "";
            switch (e.VirtualKey)
            {
                case Windows.System.VirtualKey.N:
                case Windows.System.VirtualKey.GamepadDPadUp:
                    key = "N";
                    break;
                case Windows.System.VirtualKey.S:
                case Windows.System.VirtualKey.GamepadDPadDown:
                    key = "S";
                    break;
                case Windows.System.VirtualKey.E:
                case Windows.System.VirtualKey.GamepadDPadRight:
                    key = "E";
                    break;
                case Windows.System.VirtualKey.W:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                    key = "W";
                    break;
            }
            await HandleKeyInput(key);
        }
        /// <summary>
        /// Forward a keypress to an ExplorePage method
        /// </summary>
        /// <param name="key"></param>
        public async Task HandleKeyInput(string key)
        {
            await (Element as ExplorePage).KeyPressed(Element, new KeyEventArgs { Key = key });
        }
    }
}
