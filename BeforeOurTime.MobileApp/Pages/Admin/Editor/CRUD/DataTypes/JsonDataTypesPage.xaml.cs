using Plugin.Clipboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD.DataTypes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JsonDataTypesPage : ContentPage
    {
        public string ExitItemData { set; get; } = @"{
    ""dataType"": ""BeforeOurTime.Models.Modules.World.ItemProperties.Exits.ExitItemData"",
    ""destinationLocationId"": ""779a3a47-3048-4528-df3c-08d63af92e75"",
    ""time"": 0,
    ""effort"": 0,
    ""direction"": 5
}";
        public string VisibleItemData { set; get; } = @"{
    ""dataType"": ""BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles.VisibleItemData"",
    ""name"": ""A Bright Light"",
    ""description"": ""The warm glow encircles your feet"",
    ""icon"": null
}";
        public string LocationItemData { set; get; } = @"{
    ""dataType"": ""BeforeOurTime.Models.Modules.World.ItemProperties.Locations.LocationItemData"",
}";
        public string PhysicalItemData { set; get; } = @"{
    ""dataType"": ""BeforeOurTime.Models.Modules.World.ItemProperties.Physicals.PhysicalItemData"",
    ""mobile"": true,
    ""weight"": 0
}";
        public string GeneratorItemData { set; get; } = @"{
    ""dataType"": ""BeforeOurTime.Models.Modules.World.ItemProperties.Generators.GeneratorItemData"",
    ""interval"": 10,
    ""maximum"": 1,
    ""json"": """"
}";
        public string ItemData { set; get; } = @"{
    ""parentId"": ""f4212bfe-ef65-4632-df2b-08d63af92e75"",
    ""data"": []
}";
        /// <summary>
        /// Constructor
        /// </summary>
        public JsonDataTypesPage()
        {
            InitializeComponent();
            ItemDataLabel.Text = ItemData;
            ExitItemDataLabel.Text = ExitItemData;
            VisibleItemDataLabel.Text = VisibleItemData;
            PhysicalItemDataLabel.Text = PhysicalItemData;
            LocationItemDataLabel.Text = LocationItemData;
            GeneratorItemDataLabel.Text = GeneratorItemData;
        }
        /// <summary>
        /// Copy desired code snippet onto the system clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ItemData_OnClicked(object sender, EventArgs e)
        {
            var control = (Button)sender;
            if (control.Text == "Copy Item Data:")
            {
                CrossClipboard.Current.SetText(ItemData);
                await DisplayAlert("Ok", "Copied to clipboard", "Ok");
            }
            if (control.Text == "Copy Visible Item Data:")
            {
                CrossClipboard.Current.SetText(VisibleItemData);
                await DisplayAlert("Ok", "Copied to clipboard", "Ok");
            }
            if (control.Text == "Copy Exit Item Data:")
            {
                CrossClipboard.Current.SetText(ExitItemData);
                await DisplayAlert("Ok", "Copied to clipboard", "Ok");
            }
            if (control.Text == "Copy Location Item Data:")
            {
                CrossClipboard.Current.SetText(LocationItemData);
                await DisplayAlert("Ok", "Copied to clipboard", "Ok");
            }
            if (control.Text == "Copy Physical Item Data:")
            {
                CrossClipboard.Current.SetText(PhysicalItemData);
                await DisplayAlert("Ok", "Copied to clipboard", "Ok");
            }
            if (control.Text == "Copy Generator Item Data:")
            {
                CrossClipboard.Current.SetText(GeneratorItemData);
                await DisplayAlert("Ok", "Copied to clipboard", "Ok");
            }
        }
        public async void ButtonCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
	}
}