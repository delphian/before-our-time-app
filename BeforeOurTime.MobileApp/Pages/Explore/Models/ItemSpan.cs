using BeforeOurTime.Models.Modules.Core.Models.Items;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Pages.Explore.Models
{
    /// <summary>
    /// Associate a formatted string span with an item
    /// </summary>
    public class ItemSpan
    {
        /// <summary>
        /// Formatted string span
        /// </summary>
        public Span Span { set; get; }
        /// <summary>
        /// Item that formatted string span is representing
        /// </summary>
        public Item Item { set; get; }
        /// <summary>
        /// Span has focus
        /// </summary>
        public bool Selected { set; get; } = false;
    }
}
