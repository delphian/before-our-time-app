using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
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
        /// <summary>
        /// Callback for each tree view item to format it's name
        /// </summary>
        public static readonly BindableProperty FormatterProperty =
            BindableProperty.Create(
                "Formatter",
                typeof(ICommand),
                typeof(TreeViewControl),
                default(ICommand),
                BindingMode.TwoWay,
                propertyChanged: FormatterChanged);
        public ICommand Formatter
        {
            get => (ICommand)GetValue(FormatterProperty);
            set => SetValue(FormatterProperty, value);
        }
        /// <summary>
        /// Property to display as the title for each tree view object
        /// </summary>
        public static readonly BindableProperty TitlePropertyProperty =
            BindableProperty.Create(
                "TitleProperty",
                typeof(string),
                typeof(TreeViewControl),
                default(string),
                BindingMode.TwoWay,
                propertyChanged: TitlePropertyChanged);
        public string TitleProperty
        {
            get => (string)GetValue(TitlePropertyProperty);
            set => SetValue(TitlePropertyProperty, value);
        }
        public StackLayout TreeView { set; get; } = new StackLayout();
        /// <summary>
        /// Constructor
        /// </summary>
        public TreeViewControl()
        {
            Padding = new Thickness(0);
            BackgroundColor = Color.Transparent;
            HasShadow = false;
            BuildTreeView(Source, TreeView, TitleProperty);
            Content = TreeView;
        }
        /// <summary>
        /// Build a tree view based on an enumerable
        /// </summary>
        /// <param name="propertyName">Property name that should be used as label content</param>
        public void BuildTreeView(
            IEnumerable<object> source,
            StackLayout treeView,
            string propertyName)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    Type myType = item.GetType();
                    item.GetType().GetProperties()
                        .Where(x => x.Name == propertyName)
                        .ToList().ForEach((property) =>
                        {
                            var customTitle = new TreeViewCustomTitle()
                            {
                                Obj = item,
                                Title = property.GetValue(item, null) as string
                            };
                            Formatter.Execute(customTitle);
                            var label = new Label() { Text = customTitle.Title };
                            label.GestureRecognizers.Add(new TapGestureRecognizer()
                            {
                                Command = new Command(() => {
                                    var x = item;
                                    var z = 1;
                                })
                            });
                            treeView.Children.Add(label);
                        });
                    item.GetType().GetProperties()
                        .Where(x => x.PropertyType.IsGenericType && 
                                    x.PropertyType.GetGenericArguments()[0] == myType)
                        .ToList().ForEach((property) =>
                        {
                            var nextLevelEnumerable = property.GetValue(item, null) 
                                as IEnumerable<object>;
                            var nextTreeView = new StackLayout()
                            {
                                Padding = new Thickness(20, 0, 0, 0)
                            };
                            treeView.Children.Add(nextTreeView);
                            BuildTreeView(nextLevelEnumerable, nextTreeView, propertyName);
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
            var treeViewControl = bindable as TreeViewControl;
            treeViewControl.Source = newValue as IEnumerable<object>;
            treeViewControl.Content = new StackLayout();
            treeViewControl.BuildTreeView(
                treeViewControl.Source, 
                treeViewControl.Content as StackLayout,
                treeViewControl.TitleProperty);
        }
        /// <summary>
        /// Update bindable property
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void FormatterChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var treeViewControl = bindable as TreeViewControl;
            treeViewControl.Formatter = newValue as ICommand;
            treeViewControl.TreeView = new StackLayout();
            treeViewControl.BuildTreeView(
                treeViewControl.Source,
                treeViewControl.Content as StackLayout,
                treeViewControl.TitleProperty);
        }
        /// <summary>
        /// Update bindable property
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void TitlePropertyChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var treeViewControl = bindable as TreeViewControl;
            treeViewControl.TitleProperty = newValue as string;
            treeViewControl.TreeView = new StackLayout();
            treeViewControl.BuildTreeView(
                treeViewControl.Source,
                treeViewControl.Content as StackLayout,
                treeViewControl.TitleProperty);
        }
    }
}