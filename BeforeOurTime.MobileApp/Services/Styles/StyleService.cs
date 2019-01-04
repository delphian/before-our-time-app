using BeforeOurTime.Models.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Styles
{
    /// <summary>
    /// Style service
    /// </summary>
    public class StyleService : IStyleService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StyleService()
        {
        }
        /// <summary>
        /// Get a template that contains styles for every control type
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public StyleTemplate GetTemplate(Guid? templateId = null)
        {
            return new StyleTemplate()
            {
                PagePrimary = new StylePage()
                {
                    BackgroundColor = "#202020"
                },
                ButtonPrimary = new StyleButton()
                {
                    TextColor = "#00f000",
                    BorderColor = "#60a060",
                    BackgroundColor = "#408040",
                    BorderRadius = 10
                },
                ButtonWarning = new StyleButton()
                {
                    TextColor = "#e0e000",
                    BorderColor = "#d0d060",
                    BackgroundColor = "#808040",
                    BorderRadius = 2
                },
                ButtonDanger = new StyleButton()
                {
                    TextColor = "#f08080",
                    BorderColor = "#d06060",
                    BackgroundColor = "#804040",
                    BorderRadius = 2
                },
                EntryPrimary = new StyleEntry()
                {
                    BackgroundColor = "#404040",
                    TextColor = "#ffffff",
                    PlaceholderColor = "#ffffff"
                },
                EditorPrimary = new StyleEditor()
                {
                    BackgroundColor = "#404040",
                    TextColor = "#ffffff"
                }
            };
        }
        /// <summary>
        /// Clear any caches the service may be using
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            await Task.Delay(0);
        }
    }
}
