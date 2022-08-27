using Bermuda.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MaterialDesignThemes;
using MaterialDesignThemes.Wpf;
using Bermuda.Popups;
using System.Diagnostics;
using Bermuda.Models;
using System;

namespace Bermuda.Views
{
    /// <summary>
    /// Interaction logic for Profiles.xaml
    /// </summary>
    public partial class Profiles : Page
    {

        public ProfileViewModel ProfileView { get; } = (Application.Current as App).Container.GetService<ProfileViewModel>();


        public Profiles()
        {
            InitializeComponent();
            //this.DataContext = this;
        }

        public async void ProxyGroupList_Add_Click(object sender, RoutedEventArgs e)
        {

            /*
            if (successfulInput)
            {
                try
                {
                    totalImported = this.ProxyView.AddGroup(strGroupName, strProxies);

                }
                catch (Exception err) { }
            }

            // Inform user of importation status
            if (successfulInput && totalImported > 0)
            {
                NotifyUser(successfulInput, "Add Proxy", $"Sucessfully imported {totalImported} into new Group");
            }
            else
            {
                NotifyUser(successfulInput, "Add Proxy", "Failed to add group, please ensure that data is valid!");
            }
            */

        

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void ProfileGroupList_Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem = ProfileGroupList.SelectedItem;
            if (selectedItem != null)
            {
                ProfileGroup pGroup = selectedItem as ProfileGroup;
                string proxyGroupName = pGroup.GroupName;
                ProfileView.RemoveGroup(pGroup);
            }
        }

        private async void ProfileGroupList_Add_Button_Click(object sender, RoutedEventArgs e)
        {

            var dialogContent = new AddProfileGroup();
            await MaterialDesignThemes.Wpf.DialogHost.Show(dialogContent);

            if (dialogContent.SaveRequested)
            {
                string groupName = dialogContent.GroupName;
                
                if (groupName.Length == 0)
                {
                    // Show error
                    this.SendError("No profile group name provided");
                }
                else
                {
                    this.ProfileView.ImportProfiles(groupName, "");
                }
            }

        }

        private async void AddProfile_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialogContent = new AddProfile();
            await MaterialDesignThemes.Wpf.DialogHost.Show(dialogContent);

            if (dialogContent.SaveRequested)
            {

                try
                {
                    await ProfileView.AddProfile(
                        dialogContent.ProfileName,
                        dialogContent
                    );
                    RemoveProfileBtn.IsEnabled = false;
                }
                catch (Exception err)
                {
                    this.SendError("Error creating new profile, missing critical fields.");
                }
                
            }

        }

        private async void RemoveProfile_Button_Click(object sender, RoutedEventArgs e)
        {

            object selectedItem = ItemsDataGrid.SelectedItem;

            if (selectedItem == null)
            {
                return;
            }

            Profile profile = selectedItem as Profile;

            // Remove from data container
            ProfileView.RemoveProfile(profile.personalInfo.profileName);
            
            if (ProfileView.SelectedProfileGroup.profiles.Count == 0)
            {
                RemoveProfileBtn.IsEnabled = false;
            }

            return;

        }

        private void ProfileGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                AddProfileBtn.IsEnabled = false;
                RemoveProfileBtn.IsEnabled = false;
                return;
            }

            ProfileGroup pGroup = e.AddedItems[0] as ProfileGroup;
            string proxyGroupName = pGroup.GroupName;
            ProfileView.SelectGroup(proxyGroupName);

            AddProfileBtn.IsEnabled = true;

            if (ProfileView.SelectedProfileGroup.profiles.Count == 0)
            {
                RemoveProfileBtn.IsEnabled = false;
            }else
            {
                RemoveProfileBtn.IsEnabled = true;
            }

        }

        private async void SendError(string message)
        {
            var errorDialog = new ErrorPopup(message);
            await MaterialDesignThemes.Wpf.DialogHost.Show(errorDialog);
        }

    }
}
