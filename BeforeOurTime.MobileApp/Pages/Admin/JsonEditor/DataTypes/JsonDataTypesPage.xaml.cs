using Plugin.Clipboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.JsonEditor.DataTypes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JsonDataTypesPage : ContentPage
    {
        public string ItemData { set; get; } = @"{
    ""parentId"": ""f4212bfe-ef65-4632-df2b-08d63af92e75"",
    ""typeId"": ""c643456d-8566-4dfe-bd19-7862ed269c7f"",
    ""data"": []
}";
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
    ""json"": ""{
        \""parentId\"": \""6904182a-f7ac-4e49-3b5b-08d64940b4b3\"",
        \""typeId\"": \""f1244f86-db87-47ee-8804-d5a588c1c342\"",
        \""data\"": [
            {
                \""dataType\"": \""BeforeOurTime.Models.Modules.Core.ItemProperties.Visibles.VisibleItemData\"",
                \""name\"": \""A Generated Item\"",
                \""description\"": \""This is a generated item that the maker has deemed undeserving of a description\"",
                \""icon\"": null
            },
            {
                \""dataType\"": \""BeforeOurTime.Models.Modules.World.ItemProperties.Physicals.PhysicalItemData\"",
                \""mobile\"": true,
                \""weight\"": 0
            },
            {
                \""dataType\"": \""BeforeOurTime.Models.Modules.World.ItemProperties.Garbages.GarbageItemData\"",
                \""interval\"": 10
            }
        ]
    }""
}";
        public string GarbageItemData { set; get; } = @"{
    ""dataType"": ""BeforeOurTime.Models.Modules.World.ItemProperties.Garbages.GarbageItemData"",
    ""interval"": 10
}";
        public string JavascriptItemData { set; get; } = @"{
    ""dataType"": ""BeforeOurTime.Models.Modules.Script.ItemProperties.Javascripts.JavascriptItemData"",
    ""script"": null,
    ""dataBag"": null
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
            GarbageItemDataLabel.Text = GarbageItemData;
            JavascriptItemDataLabel.Text = JavascriptItemData;
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
            if (control.Text == "Copy Garbage Item Data:")
            {
                CrossClipboard.Current.SetText(GarbageItemData);
                await DisplayAlert("Ok", "Copied to clipboard", "Ok");
            }
            if (control.Text == "Copy Javascript Item Data:")
            {
                CrossClipboard.Current.SetText(JavascriptItemData);
                await DisplayAlert("Ok", "Copied to clipboard", "Ok");
            }
        }
        public async void ButtonCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
	}
}