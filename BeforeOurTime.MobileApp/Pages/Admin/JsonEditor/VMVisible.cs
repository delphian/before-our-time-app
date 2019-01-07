using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.Script.ItemProperties.Javascripts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Pages.Admin.JsonEditor
{
    /// <summary>
    /// Track boolean values of control visibility
    /// </summary>
    public class VMVisible : BotViewModel, System.ComponentModel.INotifyPropertyChanged
    {
        public bool Javascript
        {
            get { return _javascript; }
            set { _javascript = value; NotifyPropertyChanged("Javascript"); }
        }
        private bool _javascript { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public VMVisible()
        {

        }
        /// <summary>
        /// Set all visibility switches based on properties in item json
        /// </summary>
        /// <param name="itemJson"></param>
        public void SetVisibility(string itemJson)
        {
            var item = JsonConvert.DeserializeObject<Item>(itemJson);
            Javascript = item.HasData<JavascriptItemData>();
        }
    }
}
