using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

namespace BeforeOurTime.MobileApp.Controls
{
    public class IconControl : Frame
    {
        public static Dictionary<string, string> DefaultIcons = new Dictionary<string, string>()
        {
            { "character", "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" },
            { "location", "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" }
        }; 
        private readonly SKCanvasView _canvasView = new SKCanvasView();
        public static readonly BindableProperty SourceProperty = 
            BindableProperty.Create(
                nameof(Source), 
                typeof(string), 
                typeof(IconControl), 
                default(string), 
                propertyChanged: RedrawCanvas);
        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        public static readonly BindableProperty DefaultProperty =
            BindableProperty.Create(
                nameof(Default),
                typeof(string),
                typeof(IconControl),
                default(string),
                propertyChanged: RedrawCanvas);
        public string Default
        {
            get => (string)GetValue(DefaultProperty);
            set => SetValue(DefaultProperty, value);
        }
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
            nameof(ForceMaxHeight),
            typeof(bool),
            typeof(IconControl),
            default(bool),
            propertyChanged: RedrawCanvas);
        /// <summary>
        /// Constructor
        /// </summary>
        public IconControl()
        {
            Padding = new Thickness(0);
            Margin = new Thickness(0);
            BackgroundColor = Color.Transparent;
            HasShadow = false;
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
            IconControl svgIcon = bindable as IconControl;
            svgIcon?._canvasView.InvalidateSurface();
        }
        private void CanvasViewOnPaintSurface(
            object sender, 
            SKPaintSurfaceEventArgs args)
        {
            string rawImage = Source;
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();
            if (string.IsNullOrEmpty(rawImage))
                rawImage = DefaultIcons[Default];
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