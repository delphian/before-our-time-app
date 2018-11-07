using BeforeOurTime.Models.Modules.Core.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Controls
{
    public class ItemIconButtonControl : Frame
    {
        private readonly FlexLayout _flexLayout = new FlexLayout();
        private readonly IconControl _icon = new IconControl();
        private readonly Label _name = new Label();
        /// <summary>
        /// Callback when button is clicked
        /// </summary>
        public ICommand OnClicked
        {
            get => (ICommand)GetValue(OnClickedProperty);
            set => SetValue(OnClickedProperty, value);
        }
        public static readonly BindableProperty OnClickedProperty = BindableProperty.Create(
            nameof(OnClicked), typeof(ICommand), typeof(ItemButtonControl), null);
        /// <summary>
        /// Name of the item
        /// </summary>
        public Item Item
        {
            get => (Item)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public static readonly BindableProperty ItemProperty = BindableProperty.Create(
            nameof(Item), typeof(Item), typeof(ItemButtonControl), default(Item),
            propertyChanged: ItemPropertyChanged);
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
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemIconButtonControl()
        {
            this.Padding = 2;
            this.Margin = 1;
            _flexLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            _flexLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            _flexLayout.Direction = FlexDirection.Column;
            var recognizer = new TapGestureRecognizer();
            recognizer.Tapped += (s, e) =>
            {
                if (OnClicked == null) return;
                if (OnClicked.CanExecute(Item))
                {
                    OnClicked.Execute(Item);
                }
            };
            _flexLayout.GestureRecognizers.Add(recognizer);
            _icon.Margin = 0;
            _icon.ForceMaxHeight = true;
            FlexLayout.SetBasis(_icon, 0.75f);
            FlexLayout.SetGrow(_icon, 1);
            _flexLayout.Children.Add(_icon);
            _name.FontSize = 9;
            _name.FontFamily = "Arial Narrow";
            _name.Margin = new Thickness(0, 0, 0, 0);
            FlexLayout.SetBasis(_name, FlexBasis.Auto);
            FlexLayout.SetAlignSelf(_name, FlexAlignSelf.Center);
            _flexLayout.Children.Add(_name);
            Content = _flexLayout;
        }
        private static void ItemPropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemIconButtonControl)bindable;
            control.Item = (Item)newvalue;
        }
        private static void NamePropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemIconButtonControl)bindable;
            control.Name = newvalue.ToString();
        }
        private static void ImagePropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemIconButtonControl)bindable;
            control.Image = newvalue.ToString();
        }
        private static void ImageDefaultPropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemIconButtonControl)bindable;
            control.ImageDefault = newvalue.ToString();
        }
    }
}
