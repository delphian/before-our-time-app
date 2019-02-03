using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using BeforeOurTime.Models.Modules.World.ItemProperties.Characters;
using BeforeOurTime.Models.Modules.World.ItemProperties.Exits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    ///     - ItemShowCommands          In
    ///     - ItemShowDescriptions      In
    ///     - ItemOnSelect              In
    ///     - ItemOnCommand             In
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
            set => SetValue(ServicesProperty, value);
        }
        public static readonly BindableProperty ServicesProperty = BindableProperty.Create(
            nameof(Services), typeof(IContainer), typeof(BotItemDescriptionsControl), default(IContainer),
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
                control.ImageService = control.Services.Resolve<IImageService>();
                control.AccountService = control.Services.Resolve<IAccountService>();
            });
        /// <summary>
        /// Image service
        /// </summary>
        public IImageService ImageService { set; get; }
        /// <summary>
        /// Account service
        /// </summary>
        public IAccountService AccountService { set; get; }
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
            default(List<Item>), BindingMode.OneWayToSource,
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
        /// Show item commands when item is selected
        /// </summary>
        public bool ItemShowCommands
        {
            get => (bool)GetValue(ItemShowCommandsProperty);
            set => SetValue(ItemShowCommandsProperty, value);
        }
        public static readonly BindableProperty ItemShowCommandsProperty = BindableProperty.Create(
            nameof(ItemShowCommands), typeof(bool), typeof(BotItemDescriptionsControl), false,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDescriptionsControl)bindable;
                BuildAll(control);
            });
        /// <summary>
        /// Show item descriptions even when item is not selected
        /// </summary>
        public bool ItemShowDescriptions
        {
            get => (bool)GetValue(ItemShowDescriptionsProperty);
            set => SetValue(ItemShowDescriptionsProperty, value);
        }
        public static readonly BindableProperty ItemShowDescriptionsProperty = BindableProperty.Create(
            nameof(ItemShowDescriptions), typeof(bool), typeof(BotItemDescriptionsControl), false,
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
        /// Callback when item command is clicked
        /// </summary>
        public ICommand ItemOnCommand
        {
            get => (ICommand)GetValue(ItemOnCommandProperty);
            set => SetValue(ItemOnCommandProperty, value);
        }
        public static readonly BindableProperty ItemOnCommandProperty = BindableProperty.Create(
            nameof(ItemOnCommand), typeof(ICommand), typeof(BotItemDescriptionsControl), default(ICommand), BindingMode.OneWay,
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
        /// Last clicked command
        /// </summary>
        public ItemCommand ItemCommand { set; get; }
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
                    control.BuildDescriptionSpans(itemEntry).ForEach((Span span) =>
                    {
                        paragraph.Spans.Add(span);
                    });
                }
                var build = (((List<string>)control.ItemIncludes).Count() == 0);
                ((List<string>)control.ItemIncludes)?.ForEach((filter) =>
                {
                    if (!build && potentialItem.Properties.Any(x => x.Key.ToString().Contains(filter)))
                    {
                        build = true;
                    }
                });
                if (build)
                {
                    buildItem(potentialItem);
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
            itemEntry.SpanNameId = span.Id;
            UpdateNameSpan(span, itemEntry);
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
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UpdateNameSpan(span, itemEntry);
                        UpdateDescriptionSpans(itemEntry);
                    });
                    // Invoke item select callback
                    if (ItemOnSelect == null) return;
                    if (ItemOnSelect.CanExecute(this))
                    {
                        ItemOnSelect.Execute(this);
                    }
                })
            });
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
            var defaultColor = Color.White;
                defaultColor = (itemEntry.Item.HasProperty<ExitItemProperty>()) ? Color.Green : defaultColor;
                defaultColor = (itemEntry.Item.HasProperty<CharacterItemProperty>()) ? Color.Red : defaultColor;
            var name = itemEntry.Item.GetProperty<VisibleItemProperty>()?.Name ?? "**Unknown**";
            if (itemEntry.Item.GetProperty<ExitItemProperty>() is ExitItemProperty exitProperty)
            {
                var direction = "";
                    direction = (exitProperty.Direction == ExitDirection.North) ? "N" : direction;
                    direction = (exitProperty.Direction == ExitDirection.South) ? "S" : direction;
                    direction = (exitProperty.Direction == ExitDirection.East) ? "E" : direction;
                    direction = (exitProperty.Direction == ExitDirection.West) ? "W" : direction;
                    direction = (exitProperty.Direction == ExitDirection.Up) ? "U" : direction;
                    direction = (exitProperty.Direction == ExitDirection.Down) ? "D" : direction;
                name = $"[{direction}] {name}";
            }
            span.Text = name;
            span.TextDecorations = TextDecorations.Underline;
            span.TextColor = (itemEntry.Selected) ? Color.Orange : defaultColor;
            span.FontAttributes = (itemEntry.Selected) ? FontAttributes.Bold : FontAttributes.None;
            return span;
        }
        /// <summary>
        /// Build description spans based on item data
        /// </summary>
        /// <remarks>
        /// Will always remove all trailing periods
        /// </remarks>
        /// <param name="control"></param>
        public List<Span> BuildDescriptionSpans(ItemEntry itemEntry)
        {
            var admin = AccountService.GetAccount().Admin;
            var descriptionSpans = new List<Span>();
            void BuildPrefix()
            {
                var prefixSpan = new Span() { Text = ": " };
                itemEntry.SpanDescriptionIds.Add(prefixSpan.Id);
                descriptionSpans.Add(prefixSpan);
            }
            Span BuildCommandSpan(ItemCommand itemCommand, ICommand callback)
            {
                var commandSpan = new Span() {
                    Text = $"[{itemCommand.Name}]",
                    Style = App.Current.Resources["botSpanPrimary"] as Xamarin.Forms.Style
                };
                if (callback != null)
                {
                    commandSpan.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        CommandParameter = itemCommand,
                        Command = new Command<ItemCommand>((commandParam) =>
                        {
                            if (callback == null) return;
                            if (callback.CanExecute(commandParam))
                            {
                                callback.Execute(commandParam);
                            }
                        }),
                    });
                }
                return commandSpan;
            }
            void BuildCommands()
            {
                var lastIndex = itemEntry.Item.GetProperty<CommandItemProperty>()?.Commands?.Count();
                if (lastIndex != null || admin)
                {
                    BuildPrefix();
                    if (admin)
                    {
                        var commandSpan = BuildCommandSpan(new ItemCommand()
                        {
                            Id = new Guid("0db8f80d-0ea8-4cbf-80ba-b37cab739391"),
                            ItemId = itemEntry.Item.Id,
                            Name = "JSON"
                        }, ItemOnCommand);
                        commandSpan.TextColor = Color.FromHex("fff0ff");
                        commandSpan.FontAttributes = FontAttributes.Italic;
                        itemEntry.SpanDescriptionIds.Add(commandSpan.Id);
                        descriptionSpans.Add(commandSpan);
                        if (lastIndex > 0)
                        {
                            var spacerSpan = new Span() { Text = ", " };
                            itemEntry.SpanDescriptionIds.Add(spacerSpan.Id);
                            descriptionSpans.Add(spacerSpan);
                        }
                    }
                    itemEntry.Item.GetProperty<CommandItemProperty>()?.Commands?.ForEach((command) =>
                    {
                        var commandSpan = BuildCommandSpan(command, ItemOnCommand);
                        itemEntry.SpanDescriptionIds.Add(commandSpan.Id);
                        descriptionSpans.Add(commandSpan);
                        lastIndex--;
                        if (lastIndex > 0)
                        {
                            var spacerSpan = new Span() { Text = ", " };
                            itemEntry.SpanDescriptionIds.Add(spacerSpan.Id);
                            descriptionSpans.Add(spacerSpan);
                        }
                    });
                }
            }
            void BuildDescription()
            {
                BuildPrefix();
                var textSpan = new Span
                {
                    Text = (itemEntry.Item.GetProperty<VisibleItemProperty>()?.Description ??
                            "**A dark fog obscures your vision**").TrimEnd(new char[] { '.' }),
                    TextDecorations = TextDecorations.None,
                    TextColor = (itemEntry.Selected) ? Color.White : Color.White,
                    FontAttributes = (itemEntry.Selected) ? FontAttributes.Bold : FontAttributes.None
                };
                itemEntry.SpanDescriptionIds.Add(textSpan.Id);
                descriptionSpans.Add(textSpan);
            }
            void BuildSuffix()
            {
                var suffixSpan = new Span() { Text = (ItemNewLine) ? "\n" : ". " };
                itemEntry.SpanDescriptionIds.Add(suffixSpan.Id);
                descriptionSpans.Add(suffixSpan);
            }
            if (itemEntry.Selected && ItemShowCommands)
            {
                BuildCommands();
            }
            if (itemEntry.Selected || ItemShowDescriptions)
            {
                BuildDescription();
            }
            BuildSuffix();
            return descriptionSpans;
        }
        /// <summary>
        /// Update existing item description spans based on it's selected status
        /// </summary>
        /// <param name="itemEntry">Item and meta data associated with spans</param>
        /// <returns></returns>
        public List<Span> UpdateDescriptionSpans(ItemEntry itemEntry)
        {
            Paragraph.FormattedText.Spans
                .Where(x => itemEntry.SpanDescriptionIds.Contains(x.Id)).ToList().ForEach((span) =>
            {
                Paragraph.FormattedText.Spans.Remove(span);
            });
            var itemSpan = Paragraph.FormattedText.Spans.Where(x => x.Id == itemEntry.SpanNameId).FirstOrDefault();
            var itemSpanIndex = Paragraph.FormattedText.Spans.IndexOf(itemSpan) + 1;
            var spans = BuildDescriptionSpans(itemEntry);
            spans.ForEach((span) =>
            {
                Paragraph.FormattedText.Spans.Insert(itemSpanIndex++, span);
            });
            return spans;
        }
    }
    /// <summary>
    /// Wrapper to add meta data to a list of items
    /// </summary>
    public class ItemEntry
    {
        public Item Item { set; get; }
        public bool Selected { set; get; }
        /// <summary>
        /// Formatted string span gui id of name span
        /// </summary>
        public Guid SpanNameId { set; get; }
        public List<Guid> SpanDescriptionIds { set; get; } = new List<Guid>();
    }
}
