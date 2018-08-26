using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BeforeOurTime.Models.Items;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Controls.TreeView
{
    /// <summary>
    /// Generic tree view control for xamarin forms
    /// </summary>
    public class TreeViewControl : Frame
    {
        /// <summary>
        /// Source is expected to be an IEnumerable composed of objects with
        /// properties that may themselves be further IEnumerables
        /// </summary>
        public static readonly BindableProperty SourceProperty =
            BindableProperty.Create(
                "Source",
                typeof(IEnumerable<object>),
                typeof(TreeViewControl),
                default(IEnumerable<object>),
                BindingMode.TwoWay,
                propertyChanged: SourceChanged);
        public IEnumerable<object> Source
        {
            get => (IEnumerable<object>)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        private static StackLayout _treeView { set; get; } = new StackLayout();
        /// <summary>
        /// Constructor
        /// </summary>
        public TreeViewControl()
        {
            Padding = new Thickness(0);
            BackgroundColor = Color.Transparent;
            HasShadow = false;
            BuildTreeView<object>(Source, _treeView, "Name");
            _treeView.BackgroundColor = Color.Green;
            Content = _treeView;
        }
        /// <summary>
        /// Build a tree view based on an enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectEnumerable"></param>
        /// <param name="treeView"></param>
        /// <param name="propertyName">Property name that should be used as label content</param>
        private static void BuildTreeView<T>(
            IEnumerable<T> objectEnumerable,
            StackLayout treeView,
            string propertyName)
        {
            if (objectEnumerable != null)
            {
                foreach (var item in objectEnumerable)
                {
                    Type myType = item.GetType();
                    item.GetType().GetProperties()
                        .Where(x => x.Name == propertyName)
                        .ToList().ForEach((property) =>
                        {
                            string value = property.GetValue(item, null) as string;
                            var label = new Label() { Text = value };
                            treeView.Children.Add(label);
                        });
                    item.GetType().GetProperties()
                        .Where(x => x.PropertyType.IsGenericType && 
                                    x.PropertyType.GetGenericArguments()[0] == myType)
                        .ToList().ForEach((property) =>
                        {
                            IEnumerable<T> nextLevelEnumerable = property.GetValue(item, null) as IEnumerable<T>;
                            var nextTreeView = new StackLayout()
                            {
                                Padding = new Thickness(20, 0, 0, 0)
                            };
                            treeView.Children.Add(nextTreeView);
                            BuildTreeView<T>(nextLevelEnumerable, nextTreeView, propertyName);
                        });
                }
            }
        }
        /// <summary>
        /// Update bindable property
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void SourceChanged(
            BindableObject bindable, 
            object oldValue, 
            object newValue)
        {
            BuildTreeView<object>(newValue as IEnumerable<object>, _treeView, "Name");
        }
    }
}