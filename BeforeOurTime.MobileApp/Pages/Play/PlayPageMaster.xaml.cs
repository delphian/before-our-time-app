using BeforeOurTime.MobileApp.Pages.Admin.Debug.Errors;
using BeforeOurTime.MobileApp.Pages.Admin.Editor;
using BeforeOurTime.MobileApp.Pages.Game;
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

namespace BeforeOurTime.MobileApp.Pages.Play
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayPageMaster : ContentPage
    {
        public ListView ListView;

        public PlayPageMaster()
        {
            InitializeComponent();

            BindingContext = new PlayPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class PlayPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<PlayPageMenuItem> MenuItems { get; set; }
            
            public PlayPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<PlayPageMenuItem>(new[]
                {
                    new PlayPageMenuItem {
                        Id = 0,
                        Title = "Play",
                        TargetType = typeof(GamePage) },
                    new PlayPageMenuItem {
                        Id = 1,
                        Title = "Item Editor",
                        TargetType = typeof(EditorPage) },
                    new PlayPageMenuItem {
                        Id = 2,
                        Title = "Logs",
                        TargetType = typeof(DebugErrorPage) }
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