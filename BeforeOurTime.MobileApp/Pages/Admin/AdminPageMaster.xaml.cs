using BeforeOurTime.MobileApp.Pages.Admin.Debug;
using BeforeOurTime.MobileApp.Pages.Admin.Editor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPageMaster : ContentPage
    {
        public ListView ListView;

        public AdminPageMaster()
        {
            InitializeComponent();

            BindingContext = new AdminPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class AdminPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<AdminPageMenuItem> MenuItems { get; set; }
            
            public AdminPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<AdminPageMenuItem>(new[]
                {
                    new AdminPageMenuItem { Id = 0, Title = "Item Editor", TargetType = typeof(EditorPage) },
                    new AdminPageMenuItem { Id = 1, Title = "Logs", TargetType = typeof(DebugPage) },
                    new AdminPageMenuItem { Id = 2, Title = "Exit", TargetType = null }
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}