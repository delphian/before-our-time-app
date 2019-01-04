using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Styles
{
    /// <summary>
    /// Style service
    /// </summary>
    public interface IStyleService : IService
    {
        /// <summary>
        /// Get a template that contains styles for every control type
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        StyleTemplate GetTemplate(Guid? templateId = null);
    }
}
