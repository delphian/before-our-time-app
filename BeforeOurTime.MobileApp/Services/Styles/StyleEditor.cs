using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Services.Styles
{
    /// <summary>
    /// Complete style configuration for an editor
    /// </summary>
    public class StyleEditor
    {
        public string BackgroundColor { set; get; }
        public string TextColor { set; get; }
        public int FontSize { set; get; }
        public int Margin { set; get; }
    }
}
