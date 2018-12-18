using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace BeforeOurTime.MobileApp.Controls
{
    /// <summary>
    /// Display an Images.Image
    /// </summary>
    public class BotImageControl : Frame
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        public IContainer Services
        {
            get => (IContainer)GetValue(ServicesProperty);
            set
            {
                SetValue(ServicesProperty, value);
                ImageService = Services.Resolve<IImageService>();
            }
        }
        public static readonly BindableProperty ServicesProperty = BindableProperty.Create(
            nameof(Services), typeof(IContainer), typeof(ItemIconButtonControl), default(IContainer),
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (ItemIconButtonControl)bindable;
                control.Services = (IContainer)newvalue;
            });
        /// <summary>
        /// Image service
        /// </summary>
        private IImageService ImageService { set; get; }
        private SKCanvasView _canvasView { set; get; } = new SKCanvasView();
        /// <summary>
        /// Image to display
        /// </summary>
        public BeforeOurTime.Models.Primitives.Images.Image Image
        {
            get => (BeforeOurTime.Models.Primitives.Images.Image)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
            nameof(Image), typeof(BeforeOurTime.Models.Primitives.Images.Image), typeof(BotImageControl), 
            default(BeforeOurTime.Models.Primitives.Images.Image), 
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
        {
            var control = (BotImageControl)bindable;
            control.Image = (BeforeOurTime.Models.Primitives.Images.Image)newvalue;
            control?._canvasView.InvalidateSurface();
        });
        /// <summary>
        /// Force icon to assume maximum height allowed by container and maintain ratio regardless of clip
        /// </summary>
        /// <remarks>
        /// Dynamically adjusts WidthRequest and MinimumWidthRequest to a value that maintains the image's
        /// correct ratio based on the the control's maximum expandable height
        /// </remarks>
        public bool ForceMaxHeight
        {
            get => (bool)GetValue(ForceMaxHeightProperty);
            set {
                SetValue(ForceMaxHeightProperty, value);
                if (value == true)
                {
                    VerticalOptions = LayoutOptions.FillAndExpand;
                    HorizontalOptions = LayoutOptions.FillAndExpand;
                }
            }
        }
        public static readonly BindableProperty ForceMaxHeightProperty = BindableProperty.Create(
            nameof(ForceMaxHeight), typeof(bool), typeof(IconControl), default(bool),
            propertyChanged: RedrawCanvas);
        /// <summary>
        /// Constructor
        /// </summary>
        public BotImageControl()
        {
            Padding = new Thickness(0);
            Margin = new Thickness(0);
            if (ForceMaxHeight == true)
            {
                VerticalOptions = LayoutOptions.FillAndExpand;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            _canvasView.Margin = new Thickness(0);
            Content = _canvasView;
            _canvasView.PaintSurface += CanvasViewOnPaintSurface;
        }
        private static void RedrawCanvas(
            BindableObject bindable, 
            object oldvalue, 
            object newvalue)
        {
            var control = (BotImageControl)bindable;
            control?._canvasView.InvalidateSurface();
        }
        private void CanvasViewOnPaintSurface(
            object sender, 
            SKPaintSurfaceEventArgs args)
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();
            if (Image != null)
            {
                string rawImage = Image.Value;
                var svgText = BeforeOurTime.Models.Model.Decompress(rawImage);
                using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(svgText)))
                {
                    SKSvg svg = new SKSvg();
                    svg.Load(stream);
                    SKImageInfo info = args.Info;
                    canvas.Translate(info.Width / 2f, info.Height / 2f);
                    SKRect bounds = svg.ViewBox;
                    float xRatio = info.Width / bounds.Width;
                    float yRatio = info.Height / bounds.Height;
                    float ratio;
                    if (ForceMaxHeight)
                    {
                        ratio = yRatio;
                        _canvasView.MinimumWidthRequest = Height * (bounds.Width / bounds.Height);
                        _canvasView.WidthRequest = Height * (bounds.Width / bounds.Height);
                    }
                    else
                    {
                        ratio = Math.Min(xRatio, yRatio);
                    }
                    canvas.Scale(ratio);
                    canvas.Translate(-bounds.MidX, -bounds.MidY);
                    canvas.DrawPicture(svg.Picture);
                }
            }
        }
    }
}