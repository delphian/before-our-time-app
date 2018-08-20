using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services
{
    public interface IService
    {
        /// <summary>
        /// Clear any caches the service may be using
        /// </summary>
        /// <returns></returns>
        Task Clear();
    }
}
