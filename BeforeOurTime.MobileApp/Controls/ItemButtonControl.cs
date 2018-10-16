using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Controls
{
    public class ItemButtonControl : Frame
    {
        private readonly Grid _grid = new Grid();
        private readonly Label _name = new Label();
        private readonly Label _description = new Label();
        private readonly IconControl _icon = new IconControl();
        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name
        {
            get => (string)GetValue(NameProperty);
            set
            {
                SetValue(NameProperty, value);
                _name.Text = value;
            }
        }
        public static readonly BindableProperty NameProperty = BindableProperty.Create(
            nameof(Name),
            typeof(string),
            typeof(ItemButtonControl),
            default(string),
            propertyChanged: NamePropertyChanged);
        /// <summary>
        /// Name of the item
        /// </summary>
        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set
            {
                SetValue(DescriptionProperty, value);
                _description.Text = value;
            }
        }
        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
            nameof(Description),
            typeof(string),
            typeof(ItemButtonControl),
            default(string),
            propertyChanged: DescriptionPropertyChanged);
        /// <summary>
        /// Image data (svg image xml, gzipped and base64 encoded)
        /// </summary>
        public string Image
        {
            get => (string)GetValue(ImageProperty);
            set
            {
                SetValue(ImageProperty, value);
                _icon.Source = value;
            }
        }
        public static readonly BindableProperty ImageProperty = BindableProperty.Create(
            nameof(Image),
            typeof(string),
            typeof(ItemButtonControl),
            default(string),
            propertyChanged: ImagePropertyChanged);
        /// <summary>
        /// Default image to use if none is provided
        /// </summary>
        public string ImageDefault
        {
            get => (string)GetValue(ImageDefaultProperty);
            set
            {
                SetValue(ImageDefaultProperty, value);
                _icon.Default = value;
            }
        }
        public static readonly BindableProperty ImageDefaultProperty = BindableProperty.Create(
            nameof(ImageDefault),
            typeof(string),
            typeof(ItemButtonControl),
            default(string),
            propertyChanged: ImageDefaultPropertyChanged);
        public ItemButtonControl()
        {
            this.Padding = 1;
            _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Auto) });
            _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) });
            _grid.Padding = 1;
            _grid.VerticalOptions = LayoutOptions.FillAndExpand;
            _grid.HorizontalOptions = LayoutOptions.Fill;
            _name.Margin = new Thickness(0, 0, 2, 0);
            _name.HorizontalOptions = LayoutOptions.Start;
            _description.FontSize = _name.FontSize - 2;
            _description.Margin = new Thickness(0, _name.FontSize + 2, 2, 0);
            _description.HorizontalOptions = LayoutOptions.Start;
            _icon.Margin = 0;
            _icon.ForceMaxHeight = true;
            _grid.Children.Add(_icon, 0, 0);
            _grid.Children.Add(_name, 1, 0);
            _grid.Children.Add(_description, 1, 0);
            Content = _grid;
            BorderColor = Color.LightGray;
        }
        private static void NamePropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemButtonControl)bindable;
            control.Name = newvalue.ToString();
        }
        private static void DescriptionPropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemButtonControl)bindable;
            control.Description = newvalue.ToString();
        }
        private static void ImagePropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemButtonControl)bindable;
            control.Image = newvalue.ToString();
        }
        private static void ImageDefaultPropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemButtonControl)bindable;
            control.ImageDefault = newvalue.ToString();
        }
    }
}
