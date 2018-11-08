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
            control.Items = (List<Item>)newvalue;
            control.Children.Clear();
            control.Items.ForEach(item =>
            {
                var visible = item.GetProperty<VisibleProperty>();
                if (visible != null)
                {
                    control.Children.Add(new ItemIconButtonControl()
                    {
                        OnClicked = control.TrackOnClicked,
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
