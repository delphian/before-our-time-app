using Autofac;
using BeforeOurTime.MobileApp.Controls.TreeView;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Messages.CRUD.Items.ReadItemGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.Graph
{
    public class GraphEditorPageViewModel : BotPageVM, System.ComponentModel.INotifyPropertyChanged
    {
        /// <summary>
        /// Item service for CRUD operations
        /// </summary>
        private IItemService ItemService { set; get; }
        /// <summary>
        /// Graph of items
        /// </summary>
        public List<ItemGraph> ItemGraph
        {
            set { _itemGraph = value; NotifyPropertyChanged("ItemGraph"); }
            get { return _itemGraph; }
        }
        private List<ItemGraph> _itemGraph { set; get; } = new List<ItemGraph>();
        /// <summary>
        /// Callback from TreeViewControl to customize displayed item title
        /// </summary>
        public ICommand FormatItemTitle
        {
            set { _formatItemTitle = value; NotifyPropertyChanged("FormatItemTitle"); }
            get { return _formatItemTitle; }
        }
        private ICommand _formatItemTitle { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public GraphEditorPageViewModel(IContainer container) : base(container)
        {
            ItemService = container.Resolve<IItemService>();
            FormatItemTitle = new Command((commandParameter) =>
            {
                var customTitle = commandParameter as TreeViewCustomTitle;
                customTitle.Title = (customTitle.Obj as ItemGraph).Name + 
                                    " (" + (customTitle.Obj as ItemGraph).Id.ToString() + ")";
            });
        }
        /// <summary>
        /// Read item graph
        /// </summary>
        /// <returns></returns>
        public async Task LoadItemGraphAsync()
        {
            Working = true;
            try
            {
                if (ItemGraph.Count() == 0)
                {
                    ItemGraph = new List<ItemGraph>() { await ItemService.ReadGraphAsync() };
                }
            }
            finally
            {
                Working = false;
            }
        }
    }
}
