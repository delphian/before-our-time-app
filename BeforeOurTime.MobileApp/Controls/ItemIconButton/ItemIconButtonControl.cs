using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using BeforeOurTime.Models.Modules.World.ItemProperties.Exits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Controls
{
    public class ItemIconButtonControl : Frame
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        public IContainer Services
        {
            get => (IContainer)GetValue(ServicesProperty);
            set {
                SetValue(ServicesProperty, value);
                ImageService = Services.Resolve<IImageService>();
            }
        }
        public static readonly BindableProperty ServicesProperty = BindableProperty.Create(
            nameof(Services),
            typeof(IContainer),
            typeof(ItemIconButtonControl),
            default(IContainer),
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (ItemIconButtonControl)bindable;
                control.Services = (IContainer)newvalue;
            });
        /// <summary>
        /// Image service
        /// </summary>
        private IImageService ImageService { set; get; }
        private readonly FlexLayout _flexLayout = new FlexLayout();
        private readonly BotImageControl _icon = new BotImageControl();
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
        /// Default image to use if none is provided
        /// </summary>
        public BeforeOurTime.Models.Primitives.Images.Image ImageDefault
        {
            get => (BeforeOurTime.Models.Primitives.Images.Image)GetValue(ImageDefaultProperty);
            set => SetValue(ImageDefaultProperty, value);
        }
        public static readonly BindableProperty ImageDefaultProperty = BindableProperty.Create(
            nameof(ImageDefault),
            typeof(BeforeOurTime.Models.Primitives.Images.Image),
            typeof(ItemButtonControl),
            default(BeforeOurTime.Models.Primitives.Images.Image),
            propertyChanged: ImageDefaultPropertyChanged);
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemIconButtonControl()
        {
            this.Padding = 1;
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
            FlexLayout.SetBasis(_icon, new FlexBasis(0.7f, true));
            _flexLayout.Children.Add(_icon);
            _name.FontSize = 9;
            _name.FontFamily = "Arial Narrow";
            _name.Margin = new Thickness(0, 0, 0, 0);
            _name.HorizontalOptions = LayoutOptions.Center;
            _name.HorizontalTextAlignment = TextAlignment.Center;
            FlexLayout.SetBasis(_name, new FlexBasis(0.3f, true));
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
            Guid imageGuid;
            if (control.Item?.GetProperty<VisibleItemProperty>()?.Icon != null)
            {
                imageGuid = control.Item.GetProperty<VisibleItemProperty>().Icon.Value;
            }
            else
            {
                imageGuid = (control.Item.GetProperty<ExitItemProperty>() != null) ?
                    new Guid("a15e4ade-5fbe-4eb1-9d62-f1c1e67a207b") :
                    new Guid("97f0c74d-3e50-4164-aeab-cb6561998786");
            }
            var image = control.ImageService.ReadAsync(new List<Guid>() { imageGuid }).Result.First();
            control._icon.Image = image;
        }
        private static void NamePropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemIconButtonControl)bindable;
            control.Name = newvalue.ToString();
        }
        private static void ImageDefaultPropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemIconButtonControl)bindable;
            if (control._icon.Image == null)
            {
                control._icon.Image = (BeforeOurTime.Models.Primitives.Images.Image)newvalue;
            }
        }
    }
}
