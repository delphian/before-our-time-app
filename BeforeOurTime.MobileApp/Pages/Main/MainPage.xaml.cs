using Autofac;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Main
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        private IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public MainPageViewModel MainPageViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public MainPage(IContainer container)
        {
            InitializeComponent();
            Container = container;
            BindingContext = MainPageViewModel = new MainPageViewModel(Container);
        }
        public async void OnTappedPlay(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Game.GamePage(Container));
        }
        public async void OnTappedAccount(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Account.AccountPage(Container));
        }
        public async void OnTappedServer(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Server.ServerPage(Container));
        }
        //private void OnPaintSample(object sender, SKPaintSurfaceEventArgs e)
        //{
        //    var svgGzipBase64 = "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";
        //    var svgText = Item.Decompress(svgGzipBase64);
        //    byte[] bytes = Encoding.UTF8.GetBytes(svgText);
        //    MemoryStream stream = new MemoryStream(bytes);
        //    SKSvg svg = new SKSvg();
        //    svg.Load(stream);
        //    SKCanvas canvas = e.Surface.Canvas;
        //    using (SKPaint paint = new SKPaint())
        //    {
        //        canvas.Clear(Color.Black.ToSKColor());  //paint it black
        //        canvas.DrawPicture(svg.Picture, paint);
        //    }
        //}
    }
}