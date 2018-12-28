using Autofac;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Controls
{
    /// <summary>
    /// Present a paragraph of several item names and their descriptions
    /// </summary>
    /// <remarks>
    /// Item names will preceed their descriptions and are selectable.
    /// Exposed properties:
    ///     - Items                     In
    ///     - ItemNewLine               In
    ///     - ItemIncludes              In
    ///     - ItemOnSelect              In
    ///     - ItemsSelected             Out
    ///     - ItemSelectedLast          Out
    ///     - ItemSelectedLastStatus    Out
    /// </remarks>
    public class BotItemDescriptionsControl : Frame
    {
        /// <summary>
        /// Dependency injection container
        /// </summary>
        public IContainer Services
        {
            get => (IContainer)GetValue(ServicesProperty);
            set
            {
                SetValue(ServicesProperty, value);
                ImageService = Services.Resolve<IImageService>();
            }
        }
        public static readonly BindableProperty ServicesProperty = BindableProperty.Create(
            nameof(Services), typeof(IContainer), typeof(BotItemDescriptionsControl), default(IContainer),
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
            });
        /// <summary>
        /// Image service
        /// </summary>
        public IImageService ImageService { set; get; }
        /// <summary>
        /// Items to display descriptions of
        /// </summary>
        public List<Item> Items
        {
            get => (List<Item>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        public static readonly BindableProperty ItemsProperty = BindableProperty.Create(
            nameof(Items), typeof(List<Item>), typeof(BotItemDescriptionsControl),
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
                BuildAll(control);
            });
        /// <summary>
        /// List of currently selected items
        /// </summary>
        public List<Item> ItemsSelected
        {
            get => (List<Item>)GetValue(ItemsSelectedProperty);
            set => SetValue(ItemsSelectedProperty, value);
        }
        public static readonly BindableProperty ItemsSelectedProperty = BindableProperty.Create(
            nameof(ItemsSelected), typeof(List<Item>), typeof(BotItemDescriptionsControl), 
            default(List<Item>), BindingMode.TwoWay,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
            });
        /// <summary>
        /// Last item to have it's selected status toggled
        /// </summary>
        public Item ItemSelectedLast
        {
            get => (Item)GetValue(ItemSelectedLastProperty);
            set => SetValue(ItemSelectedLastProperty, value);
        }
        public static readonly BindableProperty ItemSelectedLastProperty = BindableProperty.Create(
            nameof(ItemSelectedLast), typeof(Item), typeof(BotItemDescriptionsControl),
            default(Item), BindingMode.OneWayToSource,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
            });
        /// <summary>
        /// Current selected status of last item to have it's selected status toggled
        /// </summary>
        public bool ItemSelectedLastStatus
        {
            get => (bool)GetValue(ItemSelectedLastStatusProperty);
            set => SetValue(ItemSelectedLastStatusProperty, value);
        }
        public static readonly BindableProperty ItemSelectedLastStatusProperty = BindableProperty.Create(
            nameof(ItemSelectedLastStatus), typeof(bool), typeof(BotItemDescriptionsControl),
            default(bool), BindingMode.OneWayToSource,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
            });
        /// <summary>
        /// Add new line character after each item
        /// </summary>
        public bool ItemNewLine
        {
            get => (bool)GetValue(ItemNewLineProperty);
            set => SetValue(ItemNewLineProperty, value);
        }
        public static readonly BindableProperty ItemNewLineProperty = BindableProperty.Create(
            nameof(ItemNewLine), typeof(bool), typeof(BotItemDescriptionsControl), false,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
                BuildAll(control);
            });
        /// <summary>
        /// Only display items that contain one or more of the specified properties
        /// </summary>
        /// <remarks>
        /// Either a single string, comma delineated string, or binding to List<string>
        /// code behind variable.
        /// </remarks>
        public object ItemIncludes
        {
            get => (object)GetValue(ItemIncludesProperty);
            set => SetValue(ItemIncludesProperty, value);
        }
        public static readonly BindableProperty ItemIncludesProperty = BindableProperty.Create(
            nameof(ItemIncludes), typeof(object), typeof(BotItemDescriptionsControl),
            coerceValue: (BindableObject bindable, object value) =>
            {
                var wellFormed = new List<string>();
                if (value is List<string> valueAsList)
                {
                    wellFormed = valueAsList;
                }
                if (value is string valueAsString)
                {
                    wellFormed.AddRange(valueAsString.Split(',').ToList());
                }
                return wellFormed;
            },
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
                BuildAll(control);
            });
        /// <summary>
        /// Callback when item is selected
        /// </summary>
        public ICommand ItemOnSelect
        {
            get => (ICommand)GetValue(ItemOnSelectProperty);
            set => SetValue(ItemOnSelectProperty, value);
        }
        public static readonly BindableProperty ItemOnSelectProperty = BindableProperty.Create(
            nameof(ItemOnSelect), typeof(ICommand), typeof(BotItemDescriptionsControl), default(ICommand), BindingMode.OneWay,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
            });
        /// <summary>
        /// Formatted paragraph that includes item names and descriptions
        /// </summary>
        public Label Paragraph { set; get; } = new Label();
        /// <summary>
        /// Tracking item meta information, such as which items are currently selected.
        /// </summary>
        public List<ItemEntry> ItemEntries { set; get; } = new List<ItemEntry>();
        /// <summary>
        /// Constructor
        /// </summary>
        public BotItemDescriptionsControl()
        {
            ItemsSelected = new List<Item>();
            Content = Paragraph;
        }
        /// <summary>
        /// Build a single format aware string comprised of spans elements created from each item
        /// </summary>
        /// <param name="control"></param>
        public static void BuildAll(BotItemDescriptionsControl control)
        {
            var paragraph = new FormattedString();
            control.ItemEntries.Clear();
            control.Items?.ForEach((potentialItem) =>
            {
                void buildItem(Item item)
                {
                    var itemEntry = new ItemEntry()
                    {
                        Item = item,
                        Selected = false
                    };
                    control.ItemEntries.Add(itemEntry);
                    paragraph.Spans.Add(control.BuildNameSpan(itemEntry));
                    paragraph.Spans.Add(new Span() { Text = ": " });
                    paragraph.Spans.Add(control.BuildDescriptionSpan(itemEntry));
                    paragraph.Spans.Add(new Span() { Text = (control.ItemNewLine) ? ".\n" : ". " });
                }
                // When no filter is defined then include all items
                if (((List<string>)control.ItemIncludes).Count() == 0)
                {
                    buildItem(potentialItem);
                }
                // Otherwise only include items that have one or more approved properties
                else
                {
                    ((List<string>)control.ItemIncludes)?.ForEach((filter) =>
                    {
                        if (potentialItem.Properties.Any(x => x.Key.ToString().Contains(filter)))
                        {
                            buildItem(potentialItem);
                        }
                    });
                }
            });
            control.Paragraph.FormattedText = paragraph;
        }
        /// <summary>
        /// Build name span based on item data
        /// </summary>
        /// <param name="control"></param>
        public Span BuildNameSpan(ItemEntry itemEntry)
        {
            var span = new Span();
            UpdateNameSpan(span, itemEntry);
            if (span.GestureRecognizers.Count == 0)
            {
                span.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = new Command(() =>
                    {
                        ItemSelectedLast = itemEntry.Item;
                        itemEntry.Selected = !itemEntry.Selected;
                        ItemSelectedLastStatus = itemEntry.Selected;
                        if (itemEntry.Selected)
                        {
                            ItemsSelected.Add(itemEntry.Item);
                            ItemsSelected = ItemsSelected.ToList();
                        }
                        else
                        {
                            ItemsSelected.RemoveAll(x => x.Id == itemEntry.Item.Id);
                            ItemsSelected = ItemsSelected.ToList();
                        }
                        UpdateNameSpan(span, itemEntry);
                        // Invoke item select callback
                        if (ItemOnSelect == null) return;
                        if (ItemOnSelect.CanExecute(this))
                        {
                            ItemOnSelect.Execute(this);
                        }
                    })
                });
            }
            return span;
        }
        /// <summary>
        /// Update existing item name span based on it's selected status
        /// </summary>
        /// <param name="span">Span that displays an item's name</param>
        /// <param name="itemEntry">Item and meta data associated with span</param>
        /// <returns></returns>
        public Span UpdateNameSpan(Span span, ItemEntry itemEntry)
        {
            // These UI elements may already be displaying
            Device.BeginInvokeOnMainThread(() =>
            {
                span.Text = itemEntry.Item.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
                span.TextDecorations = TextDecorations.Underline;
                span.TextColor = (itemEntry.Selected) ? Color.Orange : Color.White;
                span.FontAttributes = (itemEntry.Selected) ? FontAttributes.Bold : FontAttributes.None;
            });
            return span;
        }
        /// <summary>
        /// Build description span based on item data
        /// </summary>
        /// <remarks>
        /// Will always remove all trailing periods
        /// </remarks>
        /// <param name="control"></param>
        public Span BuildDescriptionSpan(ItemEntry itemEntry)
        {
            var span = new Span
            {
                Text = (itemEntry.Item.GetProperty<VisibleItemProperty>()?.Description ?? 
                        "**A dark fog obscures your vision**").TrimEnd(new char[] { '.' }),
                TextDecorations = TextDecorations.None,
                TextColor = (itemEntry.Selected) ? Color.White : Color.White,
                FontAttributes = (itemEntry.Selected) ? FontAttributes.Bold : FontAttributes.None
            };
            return span;
        }
    }
    /// <summary>
    /// Wrapper to add meta data to a list of items
    /// </summary>
    public class ItemEntry
    {
        public Item Item { set; get; }
        public bool Selected { set; get; }
    }
}
