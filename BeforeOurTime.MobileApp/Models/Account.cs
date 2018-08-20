using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Models
{
    /// <summary>
    /// Last successful login credentials
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Unique account identifier returned after successful login
        /// </summary>
        public Guid AccountId { set; get; }
        /// <summary>
        /// Login name
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// Login password
        /// </summary>
        public string Password { set; get; }
    }
}
