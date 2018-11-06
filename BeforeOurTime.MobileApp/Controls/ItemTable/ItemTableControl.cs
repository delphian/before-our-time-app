﻿using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Controls
{
    public class ItemTableControl : FlexLayout
    {
        /// <summary>
        /// Name of the item
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
                    control.Children.Add(new ItemButtonControl()
                    {
                        ImageDefault = "character",
                        Image = null,
                        Name = visible.Name,
                        Description = visible.Description
                    });
                }
            });
        }
    }
}
