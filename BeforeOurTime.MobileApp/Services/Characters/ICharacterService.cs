﻿using BeforeOurTime.Models.Modules.Core.Models.Items;
using BeforeOurTime.Models.Modules.World.ItemProperties.Characters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeforeOurTime.MobileApp.Services.Characters
{
    /// <summary>
    /// Character related operations
    /// </summary>
    public interface ICharacterService : IService
    {
        /// <summary>
        /// Events that will fire when the character play state changes
        /// </summary>
        event RecieveStateChange OnPlayStateChange;
        /// <summary>
        /// Get all characters for an account
        /// </summary>
        /// <param name="accountId">Unique account identifier</param>
        /// <param name="force">Bypass cache and force load from server</param>
        /// <returns></returns>
        Task<List<Item>> GetAccountCharactersAsync(Guid accountId, bool force = false);
        /// <summary>
        /// Create a new account character
        /// </summary>
        /// <param name="accountId">Unique account identifier for which character should be created</param>
        /// <param name="name">Name of character</param>
        /// <param name="temporary">Account character is for trial purpose only</param>
        /// <returns>Item guid of new character item</returns>
        Task<Guid> CreateAccountCharacterAsync(
            Guid accountId,
            string name,
            bool temporary = false);
        /// <summary>
        /// Register a character from temporary to permenant
        /// </summary>
        /// <param name="characterId">Unique item identifier of character</param>
        /// <param name="name">Name of character</param>
        /// <returns>Item guid of new character item</returns>
        Task<Item> RegisterCharacterAsync(
            Guid characterId,
            string name);
        /// <summary>
        /// Choose an account character to play
        /// </summary>
        /// <param name="character">Item of account character that should be played</param>
        /// <returns>True on success, false on failure</returns>
        Task PlayAccountCharacterAsync(Item character);
        /// <summary>
        /// Get the user's current character play state
        /// </summary>
        /// <returns></returns>
        CharacterPlayState GetPlayState();
        /// <summary>
        /// Get the account character currently being played
        /// </summary>
        /// <returns></returns>
        Item GetCharacter();
        /// <summary>
        /// Report if account is currently playing an account character
        /// </summary>
        /// <returns></returns>
        bool IsPlaying();
    }
}
