using Bermuda.Enums;
using Bermuda.Interfaces;
using Bermuda.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Bermuda.Popups;

namespace Bermuda.ViewModels
{
    public class ProfileViewModel : BindableBase
    {

        public ObservableCollection<ProfileGroup> ProfileGroups { get; set; }
        public ProfileGroup SelectedProfileGroup { get; set; }

        public ProfileViewModel(IDataService dataService)
        {
            _dataService = dataService;
            //_navigationService.SetCleanupRoutine(this.ManualCleanup);
            LoadConfiguration();
        }

        public bool ManualCleanup()
        {
            this.ProfileGroups = null;
            this.SelectedProfileGroup = null;
            GC.Collect();
            return true;
        }

        /*
         * Read data from the data service
         */
        bool LoadConfiguration()
        {

            ProfileGroups = new ObservableCollection<ProfileGroup>();
            SelectedProfileGroup = new ProfileGroup();
            SelectedProfileGroup.profiles = new ObservableCollection<Profile>();
            Collection<ProfileGroup> importedProfiles = this._dataService.LoadProfileGroups();

            foreach (ProfileGroup profileGroup in importedProfiles)
            {
                profileGroup.ProfileCount = profileGroup.profiles.Count;
                ProfileGroups.Add(profileGroup);
            }
           
            return true;
        }

        public bool SelectGroup(string groupName)
        {

            for (int x = 0; x < ProfileGroups.Count; x++)
            {
                if (ProfileGroups[x].GroupName == groupName)
                {
                    
                    SelectedProfileGroup.GroupName = groupName;
                    if (SelectedProfileGroup.profiles == null) break;

                    SelectedProfileGroup.profiles.Clear();
                    for (int i = 0; i < ProfileGroups[x].profiles?.Count; i++)
                    {
                        SelectedProfileGroup.profiles.Add(
                            ProfileGroups[x].profiles[i]
                        );
                    }
                    return true;
                }
            }

            return false;
        }

        public bool RemoveGroup(ProfileGroup profileGroup)
        {
            this._dataService.RemoveProfileGroup(profileGroup);
            this.SelectedProfileGroup.profiles.Clear();
            this.ProfileGroups.Remove(profileGroup);
            return true;
        }

        public async Task<bool> RemoveProfile(string profileName)
        {
            
            this._dataService.RemoveProfileFromGroup(
                profileName,
                this.SelectedProfileGroup.GroupName
            );

            Profile selectedProfile = null;
            foreach(Profile profile in this.SelectedProfileGroup.profiles)
            {
                if (profile.personalInfo.profileName == profileName)
                {
                    selectedProfile = profile;
                    break;
                }
            }

            if (selectedProfile == null) return false;

            // Remove profile from local container copy
            this.SelectedProfileGroup.profiles.Remove(selectedProfile);

            return true;
        }

        public async Task<bool> AddProfile(string profileName, AddProfile profileDialog)
        {

            Profile profile = new Profile();
            profile.billingInfo = new Billing();
            profile.deliveryInfo = new Delivery();
            profile.digitalInfo = new Digital();
            profile.personalInfo = new Personal();

            // Personal
            profile.personalInfo.profileName = profileDialog.ProfileName;
            profile.personalInfo.firstName = profileDialog.FirstName;
            profile.personalInfo.lastName = profileDialog.LastName;

            // Digital
            profile.digitalInfo.email = profileDialog.Email;
            profile.digitalInfo.phone = profileDialog.PhoneNumber;

            // Delivery
            profile.deliveryInfo.deliveryAddress1 = profileDialog.Address1;
            profile.deliveryInfo.deliveryAddress2 = profileDialog.Address2;
            profile.deliveryInfo.deliveryCity = profileDialog.City;
            profile.deliveryInfo.deliveryState = profileDialog.State;
            profile.deliveryInfo.deliveryStateCode = profileDialog.StateCode;
            profile.deliveryInfo.deliveryZip = profileDialog.ZipCode;

            // Billing
            profile.billingInfo.billingAddress1 = profileDialog.Address1;
            profile.billingInfo.billingAddress2 = profileDialog.Address2;
            profile.billingInfo.billingCity = profileDialog.City;
            profile.billingInfo.billingState = profileDialog.State;
            profile.billingInfo.billingStateCode = profileDialog.StateCode;
            profile.billingInfo.billingZip = profileDialog.ZipCode;
            profile.billingInfo.cardExpMonth = profileDialog.CardExpMonth;
            profile.billingInfo.cardExpYear = profileDialog.CardExpYear;
            profile.billingInfo.cardCVV = profileDialog.CardCVC;
            profile.billingInfo.cardNumber = profileDialog.CardNumber;

            // Verify integrity of profile
            this.VerifyProfileData(profile);

            // Add to data-service
            this._dataService.AddProfileToGroup(
                profile,
                this.SelectedProfileGroup.GroupName
            );

            this.SelectedProfileGroup.profiles.Add(profile);

            return true;
        }

        private void VerifyProfileData(Profile profile)
        {

            if (
                profile.personalInfo.profileName.Length == 0 ||
                profile.personalInfo.firstName.Length == 0 ||
                profile.personalInfo.lastName.Length == 0 ||
                profile.digitalInfo.email.Length == 0 ||
                profile.digitalInfo.phone.Length == 0 ||
                profile.deliveryInfo.deliveryAddress1.Length == 0 ||
                profile.deliveryInfo.deliveryCity.Length == 0 ||
                profile.deliveryInfo.deliveryState.Length == 0 ||
                profile.deliveryInfo.deliveryStateCode.Length == 0 ||
                profile.deliveryInfo.deliveryZip.Length == 0 ||
                profile.billingInfo.billingAddress1.Length == 0 ||
                profile.billingInfo.billingCity.Length == 0 ||
                profile.billingInfo.billingState.Length == 0 ||
                profile.billingInfo.billingStateCode.Length == 0 ||
                profile.billingInfo.billingZip.Length == 0 ||
                profile.billingInfo.cardExpMonth.Length == 0 ||
                profile.billingInfo.cardExpYear.Length == 0 ||
                profile.billingInfo.cardCVV.Length == 0 ||
                profile.billingInfo.cardNumber.Length == 0
            )
            {
                throw new Exception("Missing fields");
            }

        }

        public async Task<int> ImportProfiles(string groupName, string profileDump)
        {

            ProfileGroup profileGroup = new ProfileGroup();
            profileGroup.GroupName = groupName;

            profileGroup.profiles = new ObservableCollection<Profile>();

            this._dataService.SaveProfileGroup(profileGroup);
            this.ProfileGroups.Add(profileGroup);

            return 0;
        }

        private void DummyPopulate()
        {
            
           for (int x = 0; x < 3; x++)
           {

               ProfileGroup profileGroup = new ProfileGroup();
               profileGroup.profiles = new ObservableCollection<Profile>();
               profileGroup.GroupName = $"Group - {x}";

               string[] Names = { "Alex", "John", "King", "Ahmed" };
               string[] Addresses = { "Bottom Ave", "Right Cir", "Newport Drive", "Kingsway" };
               string[] States = { "California", "Texas", "Gerogia", "New Virginia" };

               var rand = new Random();
               for (int i = 0; i < 10; i++)
               {

                   Profile profile = new Profile();
                   profile.billingInfo = new Billing();
                   profile.deliveryInfo = new Delivery();
                   profile.personalInfo = new Personal();
                   profile.digitalInfo = new Digital();

                   // Random data generation
                   string randomAddress = $"{rand.Next(1000, 9000)} {Addresses[rand.Next(0, Addresses.Length)]}";
                   string randomFName = Names[rand.Next(0, Names.Length)];
                   string randomLName = Names[rand.Next(0, Names.Length)];
                   string randomUsername = randomLName + $"{rand.Next(1000, 9000)}";
                   string randomState = $"{States[rand.Next(0, States.Length)]}";
                   string randomCity = $"ExampleCity-{i}";
                   string randomZip = $"{rand.Next(10000, 90000)}";
                   string randomEmail = $"{randomUsername}@gmail.com";

                   // Billing
                   profile.billingInfo.billingAddress1 = randomAddress;
                   profile.billingInfo.billingCity = randomCity;
                   profile.billingInfo.billingState = randomState;
                   profile.billingInfo.billingZip = randomZip;
                   profile.billingInfo.cardExpMonth = $"{rand.Next(0, 12)}";
                   profile.billingInfo.cardExpYear = $"{rand.Next(2021, 2025)}";
                   profile.billingInfo.cardNumber = $"***********{rand.Next(1000, 9999)}";
                   profile.billingInfo.billingAddress2 = "Empty";

                   // Digital
                   profile.digitalInfo.email = randomEmail;
                   profile.digitalInfo.password = "*******";
                   profile.digitalInfo.username = randomUsername;

                   // Delivery
                   profile.deliveryInfo.deliveryAddress1 = randomAddress;
                   profile.deliveryInfo.deliveryAddress2 = "Empty";
                   profile.deliveryInfo.deliveryCity = randomCity;
                   profile.deliveryInfo.deliveryState = randomState;
                   profile.deliveryInfo.deliveryZip = randomZip;

                   // Personal
                   profile.personalInfo.profileName = $"NikeProfile-{x+i}";
                   profile.personalInfo.firstName = randomFName;
                   profile.personalInfo.lastName = randomLName;

                   // Add to the list
                   profileGroup.profiles.Add(profile);

               }

               //this._dataService.SaveProfileGroup(profileGroup);
               ProfileGroups.Add(profileGroup);
           }
           
        }


    }
}
