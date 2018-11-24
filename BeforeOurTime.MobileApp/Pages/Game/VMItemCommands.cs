using Autofac;
using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Exceptions;
using BeforeOurTime.Models.Modules.Core.Messages.UseItem;
using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Core.Models.Properties;
using BeforeOurTime.Models.Modules.World.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Pages.Game
{

    /// <summary>
    /// View model and logic for sending emotes
    /// </summary>
    public class VMItemCommands : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Depedency injection container
        /// </summary>
        private IContainer Container { set; get; }
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// List of possible emotes
        /// </summary>
        public List<VMItemCommand> ItemCommands
        {
            get { return _itemCommands; }
            set { _itemCommands = value; NotifyPropertyChanged("ItemCommands"); }
        }
        private List<VMItemCommand> _itemCommands { set; get; } = new List<VMItemCommand>();
        /// <summary>
        /// Selected emote from list
        /// </summary>
        public VMItemCommand SelectedCommand
        {
            get { return _selectedCommand; }
            set { _selectedCommand = value; NotifyPropertyChanged("SelectedCommand"); }
        }
        private VMItemCommand _selectedCommand { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        public VMItemCommands(IContainer container)
        {
            Container = container;
            MessageService = Container.Resolve<IMessageService>();
        }
        /// <summary>
        /// Remove any item specific commands
        /// </summary>
        public void ClearItemCommands()
        {
            ItemCommands.RemoveAll(x => x.Item != null);
        }
        /// <summary>
        /// Add all commands from an item to list of commands
        /// </summary>
        /// <param name="item"></param>
        public void AddItemCommands(Item item)
        {
            if (item != null)
            {
                if (Container.Resolve<IAccountService>().GetAccount().Admin)
                {
                    ItemCommands.Add(new VMItemCommand()
                    {
                        Name = "* Edit JSON",
                        Item = item
                    });
                }
                item?.GetProperty<CommandProperty>()?.Commands?.ForEach(command =>
                {
                    ItemCommands.Add(new VMItemCommand()
                    {
                        Name = command.Name,
                        Item = item,
                        Command = command
                    });
                });
                ItemCommands = ItemCommands.ToList();
            }
        }
        /// <summary>
        /// Perform the selected item command
        /// </summary>
        /// <returns></returns>
        public async Task PerformSelectedCommand()
        {
            if (SelectedCommand != null)
            {
                var command = SelectedCommand.Command;
                var useRequest = new CoreUseItemRequest()
                {
                    ItemId = SelectedCommand.Item.Id,
                    Use = command
                };
                var result = await MessageService.SendRequestAsync<CoreUseItemResponse>(useRequest);
                if (!result.IsSuccess())
                {
                    throw new BeforeOurTimeException(result._responseMessage);
                }
                SelectedCommand = null;
            }
        }
    }
    /// <summary>
    /// A single item command
    /// </summary>
    public class VMItemCommand : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Name of command
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; NotifyPropertyChanged("Name"); }
        }
        private string _name { set; get; }
        /// <summary>
        /// Item that command is associated with
        /// </summary>
        public Item Item
        {
            get { return _item; }
            set { _item = value; NotifyPropertyChanged("Item"); }
        }
        private Item _item { set; get; }
        /// <summary>
        /// An item command
        /// </summary>
        public Command Command
        {
            get { return _command; }
            set { _command = value; NotifyPropertyChanged("Command"); }
        }
        private Command _command { set; get; }
    }
}
