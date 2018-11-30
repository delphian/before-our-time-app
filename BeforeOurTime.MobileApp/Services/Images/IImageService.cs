using BeforeOurTime.Models.Primitives.Images;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Items
{
    /// <summary>
    /// Image service
    /// </summary>
    public interface IImageService : IService
    {
        /// <summary>
        /// Read multiple items based on a list of unique item identifiers
        /// </summary>
        /// <param name="imageIds">List of unique image identifiers</param>
        /// <returns></returns>
        Task<List<Image>> ReadAsync(List<Guid> imageIds = null);
    }
}
