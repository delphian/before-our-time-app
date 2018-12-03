using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Controls
{
    public class ItemTableControl : FlexLayout
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        public IContainer Services
        {
            get => (IContainer)GetValue(ServicesProperty);
            set => SetValue(ServicesProperty, value);
        }
        public static readonly BindableProperty ServicesProperty = BindableProperty.Create(
            nameof(Services),
            typeof(IContainer),
            typeof(ItemTableControl),
            default(IContainer),
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (ItemTableControl)bindable;
                control.Services = (IContainer)newvalue;
            });
        /// <summary>
        /// Callback to parent when button is clicked
        /// </summary>
        public ICommand OnClicked
        {
            get => (ICommand)GetValue(OnClickedProperty);
            set => SetValue(OnClickedProperty, value);
        }
        public static readonly BindableProperty OnClickedProperty = BindableProperty.Create(
            nameof(OnClicked), typeof(ICommand), typeof(ItemTableControl), null);
        /// <summary>
        /// Local callback when item is selected
        /// </summary>
        public ICommand TrackOnClicked { set; get; }
        /// <summary>
        /// Currently selected item
        /// </summary>
        public Item SelectedItem { set; get; }
        /// <summary>
        /// Items source
        /// </summary>
        public List<Item> Items
        {
            get => (List<Item>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
            nameof(Items),
            typeof(List<Item>),
            typeof(ItemTableControl),
            default(List<Item>),
            BindingMode.TwoWay,
            propertyChanged: ItemsPropertyChanged);
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemTableControl()
        {
            TrackOnClicked = new Xamarin.Forms.Command((object item) =>
            {
                if (SelectedItem?.Id == ((Item)item)?.Id)
                {
                    item = null;
                }
                SelectedItem = (Item)item;
                if (OnClicked == null) return;
                if (OnClicked.CanExecute(this))
                {
                    OnClicked.Execute(this);
                }
            });
        }
        private static void ItemsPropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemTableControl)bindable;
            var imageService = control.Services.Resolve<IImageService>();
            control.SelectedItem = null;
            control.Items = (List<Item>)newvalue;
            control.Children.Clear();
            control.Items.ForEach(item =>
            {
                var visible = item.GetProperty<VisibleItemProperty>();
                if (visible != null)
                {
                    control.Children.Add(new ItemIconButtonControl()
                    {
                        Services = control.Services,
                        BorderColor = Color.Transparent,
                        BackgroundColor = Color.Transparent,
                        Padding = 0,
                        Margin = new Thickness(0, 0, 2, 0),
                        OnClicked = control.TrackOnClicked,
                        Item = item,
                        ImageDefault = null,
                        Name = visible.Name,
                        HeightRequest = 68,
                        WidthRequest = 68
                    });
                }
            });
        }
        /// <summary>
        /// Draw a border highlight around the currently selected item
        /// </summary>
        /// <param name="item"></param>
        public void SetHighlight(Item item)
        {
            // Remove highlight from all
            Children
                .Where(x => x.GetType() == typeof(ItemIconButtonControl))
                .Where(x => ((ItemIconButtonControl)x).BorderColor != Color.Transparent)
                .ToList()
                .ForEach(button =>
                {
                    ((ItemIconButtonControl)button).BorderColor = Color.Transparent;
                });
            if (item != null)
            {
                // Highlight selected item
                var button = (ItemIconButtonControl)Children
                    .Where(x => x.GetType() == typeof(ItemIconButtonControl))
                    .Where(x => ((ItemIconButtonControl)x).Item.Id == item.Id)
                    .FirstOrDefault();
                button.BorderColor = Color.LightBlue;
            }
        }
    }
}
