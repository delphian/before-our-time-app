﻿using BeforeOurTime.MobileApp.Services.Accounts;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Items;
using BeforeOurTime.Models.Messages.Requests.Create;
using BeforeOurTime.Models.Messages.Requests.List;
using BeforeOurTime.Models.Messages.Requests.Login;
using BeforeOurTime.Models.Messages.Responses.Create;
using BeforeOurTime.Models.Messages.Responses.List;
using BeforeOurTime.Models.Messages.Responses.Login;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BeforeOurTime.MobileApp.Services.Characters
{
    /// <summary>
    /// Structure that an account loginState change event handler must implement
    /// </summary>
    public delegate void RecieveStateChange(CharacterPlayState playState);
    /// <summary>
    /// Character related operations
    /// </summary>
    public class CharacterService : ICharacterService
    {
        /// <summary>
        /// Events that will fire when the character play state changes
        /// </summary>
        public event RecieveStateChange OnPlayStateChange;
        /// <summary>
        /// Account related operations
        /// </summary>
        private IAccountService AccountService { set; get; }
        /// <summary>
        /// Manage IMessage messages between client and server
        /// </summary>
        private IMessageService MessageService { set; get; }
        /// <summary>
        /// Describe user's current state concerning playing a character
        /// </summary>
        private CharacterPlayState PlayState { set; get; } = CharacterPlayState.Unknown;
        /// <summary>
        /// Account character currently being played
        /// </summary>
        private Item Character { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        public CharacterService(IAccountService accountService, IMessageService messageService)
        {
            AccountService = accountService;
            MessageService = messageService;
            AccountService.OnStateChange += (loginState) => {
                if (!AccountService.IsLoggedIn())
                {
                    Character = null;
                    SetPlayState(CharacterPlayState.Unknown);
                }
            };
        }
        /// <summary>
        /// Get all characters for an account
        /// </summary>
        /// <param name="accountId">Unique account identifier</param>
        /// <param name="force">Bypass cache and force load from server</param>
        /// <returns></returns>
        public async Task<List<Item>> GetAccountCharactersAsync(Guid accountId, bool force = false)
        {
            var characters = new List<Item>();
            var key = $"account_{accountId}_characters";
            force = true;
            if (Application.Current.Properties.ContainsKey(key) && !force)
            {
                characters = JsonConvert.DeserializeObject<List<Item>>(Application.Current.Properties[key] as string);
            }
            else
            {
                var listCharactersResponse = await MessageService.SendRequestAsync<ListAccountCharactersResponse>(
                new ListAccountCharactersRequest()
                {
                    AccountId = accountId
                });
                if (!listCharactersResponse.IsSuccess())
                {
                    throw new Exception("Can't retrieve character list for account");
                }
                characters = listCharactersResponse.AccountCharacters;
                if (characters.Count > 0)
                {
                    Application.Current.Properties[key] = JsonConvert.SerializeObject(characters);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            return characters;
        }
        /// <summary>
        /// Create a new account character
        /// </summary>
        /// <param name="accountId">Unique account identifier for which character should be created</param>
        /// <param name="name">Name of character</param>
        /// <returns></returns>
        public async Task CreateAccountCharacterAsync(Guid accountId, string name)
        {
            var createAccountCharacterResponse = await MessageService.SendRequestAsync<CreateAccountCharacterResponse>(
                new CreateAccountCharacterRequest()
                {
                    Name = name
                });
            if (!createAccountCharacterResponse.IsSuccess())
            {
                throw new Exception("Can't create character");
            }
            // Bust cache
            await GetAccountCharactersAsync(accountId, true);
        }
        /// <summary>
        /// Choose an account character to play
        /// </summary>
        /// <param name="character">Item of account character that should be played</param>
        /// <returns>True on success, false on failure</returns>
        public async Task PlayAccountCharacterAsync(Item character)
        {
            if (!AccountService.IsLoggedIn())
            {
                throw new Exception("Must be first logged in before playing a character");
            }
            try
            {
                SetPlayState(CharacterPlayState.Requesting);
                var loginAccountCharacter = await MessageService.SendRequestAsync<LoginAccountCharacterResponse>(
                    new LoginAccountCharacterRequest()
                    {
                        ItemId = character.Id
                    });
                if (!loginAccountCharacter.IsSuccess())
                {
                    throw new Exception("Unable to play character");
                }
                Character = character;
                SetPlayState(CharacterPlayState.Playing);
                Application.Current.Properties["AccountCharacter"] = JsonConvert.SerializeObject(Character);
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception e)
            {
                SetPlayState(CharacterPlayState.Unknown);
                throw e;
            }
        }
        /// <summary>
        /// Set the user's current character play state and raise state change event
        /// </summary>
        /// <param name="playState">New value of character play state</param>
        protected void SetPlayState(CharacterPlayState playState)
        {
            if (playState != PlayState)
            {
                PlayState = playState;
                OnPlayStateChange?.Invoke(PlayState);
            }
        }
        /// <summary>
        /// Get the user's current character play state
        /// </summary>
        /// <returns></returns>
        public CharacterPlayState GetPlayState()
        {
            return PlayState;
        }
        /// <summary>
        /// Get the account character currently being played
        /// </summary>
        /// <returns></returns>
        public Item GetCharacter()
        {
            return Character;
        }
        /// <summary>
        /// Report if account is currently playing an account character
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return (PlayState == CharacterPlayState.Playing);
        }
        /// <summary>
        /// Clear any caches the service may be using
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            Character = null;
            Application.Current.Properties.Remove("AccountCharacter");
            await Application.Current.SavePropertiesAsync();
        }
    }
}
