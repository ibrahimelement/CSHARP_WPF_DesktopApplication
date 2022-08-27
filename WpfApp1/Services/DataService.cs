using Bermuda.Interfaces;
using Bermuda.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Bermuda.Services
{
    public class DataService : IDataService
    {

        public Collection<TaskGroup> taskGroupStates { get; set; }
        public UserDataModel userData { get; set; }
        private event EventHandler<bool> licensedEventHandler;
        
        public DataService()
        {
            ImportUserData();
        }

        private string GetInstallationFolder()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BORO_Bermuda");
            if (!Directory.Exists(fileName))
            {
                Directory.CreateDirectory(fileName);
            }
            return fileName;
        }


        private string GetEnvironmentFolderLocation()
        {
            var fileName = Path.Combine(GetInstallationFolder(), "BermudaUserData.json");
            return fileName;
        }

        public bool ImportUserData()
        {
            userData = new UserDataModel();
            userData.profileGroups = new Collection<ProfileGroup>();
            userData.proxyGroups = new Collection<ProxyGroup>();
            userData.taskGroups = new Collection<TaskGroup>();
            userData.checkouts = new Collection<CheckoutModel>();
            userData.settings = new SettingsModel();
            userData.licenseKey = "";
            userData.currentBackendVersion = "";
            // Runtime state
            taskGroupStates = new Collection<TaskGroup>();

            //StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            string configLocation = GetEnvironmentFolderLocation();

            if (!File.Exists(configLocation))
            {
                FileStream file = File.Create(configLocation);
                file.Close();
                string emptyData = JsonSerializer.Serialize<UserDataModel>(userData);
                File.WriteAllText(configLocation, emptyData);
            }

            string userConfiguration = File.ReadAllText(configLocation);
            userData = JsonSerializer.Deserialize<UserDataModel>(userConfiguration);

            // Need to take into consideration initial missing data.

            return true;
        }


        public bool SaveUserData()
        {
            // Now I should be linking this to the data service via the Add/Remove keywords

            try
            {
                string jsonString = JsonSerializer.Serialize<UserDataModel>(this.userData);
                File.WriteAllText(GetEnvironmentFolderLocation(), jsonString);
            }
            catch(Exception err)
            {
                Debug.WriteLine(err.Message);
            }
            
            return true;
        }

        public bool LoadConfiguration()
        {

            return true;
        }

        // Proxy
        public bool SaveProxyGroup(ProxyGroup group)
        {
            this.userData.proxyGroups.Add(group);
            this.SaveUserData();
            return true;
        }

        public bool RemoveProxyGroup(ProxyGroup group)
        {
            this.userData.proxyGroups.Remove(group);
            this.SaveUserData();
            return true;
        }

        public Collection<ProxyGroup> LoadProxyGroups()
        {
            return this.userData.proxyGroups;
        }

        // Profile
        public bool SaveProfileGroup(ProfileGroup group){
            this.userData.profileGroups.Add(group);
            this.SaveUserData();
            return true;
        }

        public bool RemoveProfileGroup(ProfileGroup group){
            this.userData.profileGroups.Remove(group);
            this.SaveUserData();
            return true;
        }

        public bool AddProfileToGroup(Profile profile, string groupName)
        {

            foreach(ProfileGroup pg in this.userData.profileGroups)
            {
                if (pg.GroupName == groupName)
                {
                    Debug.WriteLine("Adding a new profile to group");
                    pg.profiles.Add(profile);
                    break;
                }
            }

            this.SaveUserData();

            return true;
        }

        public bool RemoveProfileFromGroup(string profileName, string groupName)
        {

            // Super sexy code ;)
            bool removed = false;
            for(int x = 0; x < this.userData.profileGroups.Count && !removed; x++)
            {
                if (this.userData.profileGroups[x].GroupName == groupName)
                {
                    foreach(Profile pr in this.userData.profileGroups[x].profiles)
                    {
                        this.userData.profileGroups[x].profiles.Remove(pr);
                        removed = true;
                        break;
                    }
                }
            }

            this.SaveUserData();

            return true;
        }

        public Collection<ProfileGroup> LoadProfileGroups()
        {
            return this.userData.profileGroups;
        }

        // Task
        public Collection<TaskGroup> LoadTaskGroups()
        {
            return this.userData.taskGroups;
        }

        public bool SaveTaskGroups(TaskGroup group){
            this.userData.taskGroups.Add(group);
            this.SaveUserData();
            return true;
        }

        public bool RemoveTaskGroup(TaskGroup group)
        {
            this.userData.taskGroups.Remove(group);
            this.SaveUserData();
            return true;
        }

        // Task states (offline)
        public void UpdateTaskGroupState(Collection<TaskGroup> stateCollection)
        {
            this.taskGroupStates.Clear();
            this.taskGroupStates = stateCollection;
        }

        public Collection<TaskGroup> GetTaskGroupStates()
        {
            return this.taskGroupStates;
        }

        public Collection<CheckoutModel> GetCheckouts()
        {
            return this.userData.checkouts;
        }

        public void SaveCheckout(CheckoutModel checkout)
        {
            this.userData.checkouts.Add(checkout);
            this.SaveUserData();
        }

        public void SubscribeLicensed(EventHandler<bool> eventHandler)
        {
            licensedEventHandler += eventHandler;
        }

        public bool SaveLicense(string licenseKey)
        {
            this.userData.licenseKey = licenseKey;
            this.SaveUserData();
            licensedEventHandler?.Invoke(this, true);
            return true;
        }

        public string GetLicense()
        {
            return this.userData.licenseKey;
        }

        public bool SaveUpdateRecord(string update)
        {
            this.userData.currentBackendVersion = update;
            this.SaveUserData();
            return true;
        }

        public string GetUpdateRecord()
        {
            return this.userData.currentBackendVersion;
        }

        public bool SaveSettings(SettingsModel settings)
        {
            this.userData.settings = settings;
            this.SaveUserData();
            return true;
        }

        public SettingsModel GetSettings()
        {
            return this.userData.settings;
        }

    }
}
