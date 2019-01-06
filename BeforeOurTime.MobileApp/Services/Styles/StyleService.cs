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
            return new StyleTemplate();
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
