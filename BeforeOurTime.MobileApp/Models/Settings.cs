using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Models
{
    /// <summary>
    /// Locally cached values
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Last played character
        /// </summary>
        public Guid? CharacterId { set; get; }
        /// <summary>
        /// Last successful connection string (server)
        /// </summary>
        public string ConnectionString { set; get; }
    }
}
