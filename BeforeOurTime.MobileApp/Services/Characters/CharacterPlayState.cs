using System;
using System.Collections.Generic;
using System.Text;

namespace BeforeOurTime.MobileApp.Services.Characters
{
    public enum CharacterPlayState : int
    {
        /// <summary>
        /// Unknown or not available
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Requesting permission to play character from server
        /// </summary>
        Requesting = 10,
        /// <summary>
        /// Currently playing character
        /// </summary>
        Playing = 20
    }
}
