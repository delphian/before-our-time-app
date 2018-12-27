﻿using Autofac;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.Exit;
using BeforeOurTime.MobileApp.Pages.Admin.Editor.Location;
using BeforeOurTime.MobileApp.Services.Messages;
using BeforeOurTime.Models.Messages.Responses.List;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeforeOurTime.MobileApp.Pages.Admin.Editor.CRUD
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JsonEditorPage : ContentPage
	{
        /// <summary>
        /// Dependency injection container
        /// </summary>
        protected IContainer Container { set; get; }
        /// <summary>
        /// View model
        /// </summary>
        public JsonEditorPageViewModel ViewModel { set; get; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Dependency injection container</param>
        /// <param name="itemId">When provided automatically load this item for editing</param>
		public JsonEditorPage (IContainer container, Guid? itemId = null)
		{
			InitializeComponent ();
            Container = container;
            BindingContext = ViewModel = new JsonEditorPageViewModel(container, itemId);
            // Allow other page to load items into the json editor
            MessagingCenter.Subscribe<ContentPage, Guid>(this, "CRUDEditorPage:Load", async (sender, guid) =>
            {
                try
                {
                    ViewModel.ItemId = guid.ToString();
                    await ViewModel.ReadItem();
                }
                catch (Exception e)
                {
                    await DisplayAlert("Error", e.Message, "OK, Maybe Tomorrow");
                }
            });
        }
        public JsonEditorPage(IContainer container) : this(container, null) { }
        /// <summary>
        /// Load an item if one has been pre-selected
        /// </summary>
        protected override async void OnAppearing()
        {
            try
            {
                if (ViewModel.ItemId != null)
                {
                    await ViewModel.ReadItem();
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Error", "Unable to load item: " + e.Message, "Sadness");
            }
        }
        /// <summary>
        /// Read an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemRead_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.ReadItem();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK, Maybe Tomorrow");
            }
        }
        /// <summary>
        /// Cancel and close page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemCancel_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        /// <summary>
        /// Read an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemCreate_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.CreateItem();
                await DisplayAlert("Success", "Items have been created", "Yay!");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK, Maybe Tomorrow");
            }
        }
        /// <summary>
        /// Update an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemUpdate_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.UpdateItem();
                await DisplayAlert("Success", "Item has been saved", "Very Well");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "I Did Not Consent!");
            }
        }
        /// <summary>
        /// Delete an item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonItemDelete_OnClicked(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.DeleteItem();
                await DisplayAlert("Success", "Item has been deleted", "Very Well");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "I Did Not Consent!");
            }
        }
    }
}