using Autofac;
using BeforeOurTime.MobileApp.Pages.Account;
using BeforeOurTime.MobileApp.Pages.Admin;
using BeforeOurTime.MobileApp.Pages.Admin.AccountEditor;
using BeforeOurTime.MobileApp.Pages.Admin.AccountEditor.Backup;
using BeforeOurTime.MobileApp.Pages.Admin.Debug;
using BeforeOurTime.MobileApp.Pages.Admin.Editor;
using BeforeOurTime.MobileApp.Pages.Game;
using BeforeOurTime.MobileApp.Services.Accounts;
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
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected Autofac.IContainer Container { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection controller</param>
        public PlayPageMaster(Autofac.IContainer container)
        {
            InitializeComponent();
            Container = container;
            BindingContext = new PlayPageMasterViewModel(Container);
            ListView = MenuItemsListView;
        }

        class PlayPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<PlayPageMenuItem> MenuItems { get; set; }

            public PlayPageMasterViewModel(Autofac.IContainer container)
            {
                var account = container.Resolve<IAccountService>().GetAccount();
                var menuItemsArray = Enumerable.Empty<PlayPageMenuItem>();
                menuItemsArray = menuItemsArray.Append(new PlayPageMenuItem()
                {
                    Id = 0,
                    Title = "Play",
                    TargetType = typeof(GamePage)
                });
                menuItemsArray = menuItemsArray.Append(new PlayPageMenuItem()
                {
                    Id = 1,
                    Title = "Account",
                    TargetType = typeof(AccountPage)
                });
                if (account.Admin)
                {
                    menuItemsArray = menuItemsArray.Append(new PlayPageMenuItem()
                    {
                        Id = 5,
                        Title = "Account Editor",
                        TargetType = typeof(AccountEditorPage)
                    });
                    menuItemsArray = menuItemsArray.Append(new PlayPageMenuItem()
                    {
                        Id = 2,
                        Title = "Item Editor",
                        TargetType = typeof(EditorPage)
                    });
                    menuItemsArray = menuItemsArray.Append(new PlayPageMenuItem()
                    {
                        Id = 3,
                        Title = "Error Messages",
                        TargetType = typeof(DebugPage)
                    });
                }
                menuItemsArray = menuItemsArray.Append(new PlayPageMenuItem
                {
                    Id = 4,
                    Title = "Exit",
                    TargetType = null
                });
                MenuItems = new ObservableCollection<PlayPageMenuItem>(menuItemsArray);
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