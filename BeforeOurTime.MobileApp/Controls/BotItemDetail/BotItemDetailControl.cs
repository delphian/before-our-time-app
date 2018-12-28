using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
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
    /// <summary>
    /// Display an item image, description, and commands
    /// </summary>
    public class BotItemDetailControl : Frame
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
            nameof(Services), typeof(IContainer), typeof(BotItemDetailControl), default(IContainer),
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDetailControl)bindable;
                control.Services = (IContainer)newvalue;
            });
        /// <summary>
        /// Callback when item command is clicked
        /// </summary>
        public ICommand OnCommand
        {
            get => (ICommand)GetValue(OnCommandProperty);
            set => SetValue(OnCommandProperty, value);
        }
        public static readonly BindableProperty OnCommandProperty = BindableProperty.Create(
            nameof(OnCommand), typeof(ICommand), typeof(BotItemDetailControl), default(ICommand), BindingMode.TwoWay,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDetailControl)bindable;
                control.OnCommand = (ICommand)newvalue;
            });
        /// <summary>
        /// Callback when item detail window is closed
        /// </summary>
        public ICommand OnClose
        {
            get => (ICommand)GetValue(OnCloseProperty);
            set => SetValue(OnCloseProperty, value);
        }
        public static readonly BindableProperty OnCloseProperty = BindableProperty.Create(
            nameof(OnClose), typeof(ICommand), typeof(BotItemDetailControl), default(ICommand), BindingMode.TwoWay,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDetailControl)bindable;
                control.OnClose = (ICommand)newvalue;
            });
        /// <summary>
        /// Item to display details of
        /// </summary>
        public Item Item
        {
            get => (Item)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public static readonly BindableProperty ItemProperty = BindableProperty.Create(
            nameof(Item), typeof(Item), typeof(BotItemDetailControl), null, BindingMode.TwoWay,
            propertyChanged: (BindableObject bindable, object oldvalue, object newvalue) =>
            {
                var control = (BotItemDetailControl)bindable;
                control.Item = (Item)newvalue;
                BuildAll(control);
            });
        /// <summary>
        /// Image service
        /// </summary>
        public IImageService ImageService { set; get; }
        /// <summary>
        /// Last clicked command
        /// </summary>
        public ItemCommand ItemCommand { set; get; }
        /// <summary>
        /// Child controls
        /// </summary>
        private readonly StackLayout _stackLayout = new StackLayout();
        private readonly BotImageControl _icon = new BotImageControl();
        private readonly Label _name = new Label();
        private readonly Label _description = new Label();
        private readonly Label _commands = new Label();
        /// <summary>
        /// Constructor
        /// </summary>
        public BotItemDetailControl()
        {
            this.Padding = 1;
            this.Margin = 1;
            _stackLayout.Margin = 0;
            _stackLayout.Padding = 0;
            _stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            _stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            // Absolute
            var _absoluteLayout = new AbsoluteLayout()
            {
                Margin = 0,
                Padding = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            var _button = new Button();
            _button.Text = "X";
            _button.TextColor = Color.Red;
            _button.FontAttributes = FontAttributes.Bold;
            _button.CornerRadius = 10;
            _button.Clicked += (object sender, EventArgs e) =>
            {
                if (OnClose == null) return;
                if (OnClose.CanExecute(this))
                {
                    OnClose.Execute(this);
                }
            };
            AbsoluteLayout.SetLayoutBounds(_button, new Rectangle(1.1, -0.1, .25, .25));
            AbsoluteLayout.SetLayoutFlags(_button, AbsoluteLayoutFlags.All);
            _absoluteLayout.Children.Add(_button);
            _absoluteLayout.Children.Add(_stackLayout);
            // Top
            _stackLayout.Children.Add(_name);
            _name.FontSize = 18;
            _name.FontAttributes = FontAttributes.Bold;
            _name.Margin = new Thickness(0, 0, 0, 0);
            _name.HorizontalOptions = LayoutOptions.Start;
            _name.HorizontalTextAlignment = TextAlignment.Start;
            var _flexLayout = new FlexLayout();
            _stackLayout.Children.Add(_flexLayout);
            _flexLayout.Margin = 0;
            _flexLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            _flexLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            _flexLayout.Direction = FlexDirection.Row;
            FlexLayout.SetBasis(_icon, new FlexBasis(0.5f, true));
            _flexLayout.Children.Add(_icon);
            // Left side
            var _stackLayoutLeft = new StackLayout();
            FlexLayout.SetBasis(_stackLayoutLeft, new FlexBasis(0.5f, true));
            _flexLayout.Children.Add(_stackLayoutLeft);
            _stackLayoutLeft.Margin = 0;
            _stackLayoutLeft.Padding = 0;
            _stackLayoutLeft.VerticalOptions = LayoutOptions.FillAndExpand;
            _stackLayoutLeft.HorizontalOptions = LayoutOptions.FillAndExpand;
            _stackLayoutLeft.Children.Add(_icon);
            _icon.CornerRadius = 10;
            _icon.Margin = new Thickness(0, 0, 6, 0);
            _icon.BackgroundColor = Color.White;
            _icon.ForceMaxHeight = true;
            _icon.VerticalOptions = LayoutOptions.FillAndExpand;
            _icon.HorizontalOptions = LayoutOptions.FillAndExpand;
            // Right side
            var _stackLayoutRight = new StackLayout();
            FlexLayout.SetBasis(_stackLayoutRight, new FlexBasis(0.5f, true));
            _flexLayout.Children.Add(_stackLayoutRight);
            _stackLayoutRight.Margin = 0;
            _stackLayoutRight.Padding = 0;
            _stackLayoutRight.Children.Add(_commands);
            _commands.FontSize = 12;
            _commands.HorizontalOptions = LayoutOptions.Start;
            _commands.HorizontalTextAlignment = TextAlignment.Start;
            // Bottom
            _stackLayout.Children.Add(_description);
            _description.Margin = 0;
            _description.VerticalOptions = LayoutOptions.FillAndExpand;
            _description.HorizontalOptions = LayoutOptions.FillAndExpand;
            _description.MinimumHeightRequest = 36;
            Content = _absoluteLayout;
        }
        /// <summary>
        /// Build entire view based on item data
        /// </summary>
        /// <param name="control"></param>
        public static void BuildAll(BotItemDetailControl control)
        {
            BuildIcon(control);
            BuildName(control);
            BuildDescription(control);
            BuildCommands(control);
        }
        /// <summary>
        /// Build icon view based on current item data
        /// </summary>
        /// <param name="control"></param>
        public static void BuildIcon(BotItemDetailControl control)
        {
            if (control.Item != null)
            {
                Guid imageGuid;
                var visible = control.Item?.GetProperty<VisibleItemProperty>();
                if (visible?.Icon != null)
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
            else
            {
                control._icon.Image = null;
            }
        }
        /// <summary>
        /// Build name view based on current item data
        /// </summary>
        /// <param name="control"></param>
        public static void BuildName (BotItemDetailControl control)
        {
            if (control.Item != null)
            {
                var visible = control.Item?.GetProperty<VisibleItemProperty>();
                control._name.Text = visible?.Name;
            }
            else
            {
                control._name.Text = "";
            }
        }
        /// <summary>
        /// Build description view based on current item data
        /// </summary>
        /// <param name="control"></param>
        public static void BuildDescription (BotItemDetailControl control)
        {
            if (control.Item != null)
            {
                var visible = control.Item?.GetProperty<VisibleItemProperty>();
                control._description.Text = visible?.Description;
            }
            else
            {
                control._description.Text = "";
            }
        }
        /// <summary>
        /// Build commands view based on current item data
        /// </summary>
        /// <param name="control"></param>
        public static void BuildCommands (BotItemDetailControl control)
        {
            if (control.Item != null)
            {
                var commands = control.Item.GetProperty<CommandItemProperty>()?.Commands;
                var formattedCommands = new FormattedString();
                if (control.Services.Resolve<IAccountService>().GetAccount().Admin)
                {
                    formattedCommands.Spans.Add(CreateSpanCommand(control, "Edit JSON\n", Color.Yellow, 
                        new ItemCommand() { Name = ">> Edit JSON" }));
                    formattedCommands.Spans.Add(CreateSpanCommand(control, "Edit Location\n", Color.Yellow,
                        new ItemCommand() { Name = ">> Edit Location" }));
                    formattedCommands.Spans.Add(CreateSpanCommand(control, "Create Location\n", Color.Yellow,
                        new ItemCommand() { Name = ">> Create Location" }));
                    formattedCommands.Spans.Add(CreateSpanCommand(control, "Create Item\n\n", Color.Yellow,
                        new ItemCommand() { Name = ">> Create Item" }));
                }
                commands?.ForEach(command =>
                {
                    formattedCommands.Spans.Add(CreateSpanCommand(control, $"[{command.Name}] ", Color.LightBlue, command));
                });
                control._commands.FormattedText = formattedCommands;
            }
            else
            {
                control._commands.FormattedText = new FormattedString();
            }
        }
        /// <summary>
        /// Create a formatted string span with a command callback
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public static Span CreateSpanCommand(
            BotItemDetailControl control, 
            string text, 
            Color color, 
            ItemCommand command = null)
        {
            var span = new Span()
            {
                Text = text,
                TextColor = color
            };
            span.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                CommandParameter = control,
                Command = new Command<BotItemDetailControl>((commandParam) =>
                {
                    control.ItemCommand = command;
                    if (control.OnCommand == null) return;
                    if (control.OnCommand.CanExecute(commandParam))
                    {
                        control.OnCommand.Execute(commandParam);
                    }
                }),
            });
            return span;
        }
    }
}
