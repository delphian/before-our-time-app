using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Controls
{
    public class ItemTableControl : FlexLayout
    {
        /// <summary>
        /// Callback when button is clicked
        /// </summary>
        public ICommand OnClicked
        {
            get => (ICommand)GetValue(OnClickedProperty);
            set => SetValue(OnClickedProperty, value);
        }
        public static readonly BindableProperty OnClickedProperty = BindableProperty.Create(
            nameof(OnClicked), typeof(ICommand), typeof(ItemTableControl), null);
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
        }
        private static void ItemsPropertyChanged(
            BindableObject bindable,
            object oldvalue,
            object newvalue)
        {
            var control = (ItemTableControl)bindable;
            control.Items = (List<Item>)newvalue;
            control.Children.Clear();
            control.Items.ForEach(item =>
            {
                var visible = item.GetProperty<VisibleProperty>();
                if (visible != null)
                {
                    control.Children.Add(new ItemIconButtonControl()
                    {
                        OnClicked = control.OnClicked,
                        Item = item,
                        ImageDefault = (item.Type == ItemType.Exit) ? "location" : "character",
                        Image = null,
                        Name = visible.Name,
                        HeightRequest = 60,
                        WidthRequest = 82
                    });
                }
            });
        }
    }
}
