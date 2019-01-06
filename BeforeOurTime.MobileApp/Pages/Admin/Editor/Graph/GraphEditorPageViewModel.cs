using Autofac;
using BeforeOurTime.MobileApp.Controls.TreeView;
using BeforeOurTime.MobileApp.Pages.Admin.JsonEditor;
using BeforeOurTime.MobileApp.Services.Items;
using BeforeOurTime.Models.Modules.Core.Messages.ItemGraph;
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
        /// Code behind page that invoked this view model
        /// </summary>
        private ContentPage CodeBehind { set; get; }
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
        /// Callback from TreeViewControl for on clicked event
        /// </summary>
        public ICommand TreeViewControl_OnClicked
        {
            set { _treeViewControl_OnClicked = value; NotifyPropertyChanged("TreeViewControl_OnClicked"); }
            get { return _treeViewControl_OnClicked; }
        }
        private ICommand _treeViewControl_OnClicked { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public GraphEditorPageViewModel(IContainer container, ContentPage codeBehind) : base(container)
        {
            CodeBehind = codeBehind;
            ItemService = container.Resolve<IItemService>();
            FormatItemTitle = new Command((commandParameter) =>
            {
                var customTitle = commandParameter as TreeViewCustomTitle;
                customTitle.Title = (customTitle.Obj as ItemGraph).Name + 
                                    " (" + (customTitle.Obj as ItemGraph).Id.ToString() + ")";
            });
            TreeViewControl_OnClicked = new Command((commandParameter) =>
            {
                var itemGraph = commandParameter as ItemGraph;
                var itemId = itemGraph.Id;
                MessagingCenter.Send<ContentPage, Guid>(CodeBehind, "CRUDEditorPage:Load", itemId);
                ((TabbedPage)CodeBehind.Parent).CurrentPage = ((TabbedPage)CodeBehind.Parent).Children
                    .Where(x => x.GetType() == typeof(JsonEditorPage))
                    .First();
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
