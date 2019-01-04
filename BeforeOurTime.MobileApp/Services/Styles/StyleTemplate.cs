using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Services.Styles
{
    /// <summary>
    /// A single cohesive style for all controls
    /// </summary>
    public class StyleTemplate
    {
        public StylePage PagePrimary { set; get; }
        public StyleButton ButtonPrimary { set; get; }
        public StyleButton ButtonWarning { set; get; }
        public StyleButton ButtonDanger { set; get; }
        public StyleEntry EntryPrimary { set; get; }
        public StyleEditor EditorPrimary { set; get; }
    }
}
