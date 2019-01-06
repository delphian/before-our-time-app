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
        /// <summary>
        /// Get specified style of button from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StyleButton GetButton(StyleType type)
        {
            var button = new StyleButton();
            if (type == StyleType.Primary)
            {
                button.TextColor = "#00f000";
                button.BorderColor = "#60a060";
                button.BackgroundColor = "#408040";
                button.BorderRadius = 10;
            }
            if (type == StyleType.Warning)
            {
                button.TextColor = "#e0e000";
                button.BorderColor = "#d0d060";
                button.BackgroundColor = "#808040";
                button.BorderRadius = 2;
            }
            if (type == StyleType.Danger)
            {
                button.TextColor = "#f08080";
                button.BorderColor = "#d06060";
                button.BackgroundColor = "#804040";
                button.BorderRadius = 2;
            }
            return button;
        }
        /// <summary>
        /// Get specified style editor from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StyleEditor GetEditor(StyleType type)
        {
            var editor = new StyleEditor();
            if (type == StyleType.Primary)
            {
                editor.BackgroundColor = "#404040";
                editor.TextColor = "#ffffff";
            }
            return editor;
        }
        /// <summary>
        /// Get specified style entry from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StyleEntry GetEntry(StyleType type)
        {
            var entry = new StyleEntry();
            if (type == StyleType.Primary)
            {
                entry.BackgroundColor = "#404040";
                entry.TextColor = "#ffffff";
                entry.PlaceholderColor = "#ffffff";
            }
            return entry;
        }
        /// <summary>
        /// Get specified style page from current template
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StylePage GetPage(StyleType type)
        {
            var page = new StylePage();
            if (type == StyleType.Primary)
            {
                page.BackgroundColor = "#202020";
            }
            return page;
        }
    }
}