using Bermuda.Interfaces;
using Bermuda.Models;
using System.Collections.ObjectModel;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;

namespace Bermuda.ViewModels
{
    public class SettingsViewModel : BindableBase
    {

        private int _numBrowsers = 10;
        public int numBrowsers
        {
            get
            {
                return _numBrowsers;
            }
            set
            {
                _numBrowsers = value;
                OnPropertyChanged();
            }
        }

        private bool _shareCheckouts = false;
        public bool shareCheckouts
        {
            get
            {
                return _shareCheckouts;
            }
            set
            {
                _shareCheckouts = value;
                OnPropertyChanged();
            }
        }

        string _proxyGroupName = "Select Group";
        public string proxyGroupName
        {
            get
            {
                return _proxyGroupName;
            }
            set
            {
                _proxyGroupName = value;
                OnPropertyChanged();
            }
        }

        private string _twoCaptchaApiKey;
        public string twoCaptchaApiKey
        {
            get
            {
                return _twoCaptchaApiKey;
            }
            set
            {
                _twoCaptchaApiKey = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ProxyGroup> proxyGroups { get; set; }

        private ProxyGroup _chosenProxyGroup { get; set; } = null;
        public ProxyGroup chosenProxyGroup
        {
            get {
                return _chosenProxyGroup;
            }
            set
            {
                _chosenProxyGroup = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel(IDataService dataService, IBackendService backendService)
        {
            _dataService = dataService;
            _backendService = backendService;
            LoadConfiguration();
        }

        private bool LoadConfiguration()
        {

            this.proxyGroups = new ObservableCollection<ProxyGroup>();
            Collection<ProxyGroup> proxyGroups = _dataService.LoadProxyGroups();
            SettingsModel settings = _dataService.GetSettings();
            this.numBrowsers = settings.numBrowser == 0 ? 10 : settings.numBrowser;
            this.twoCaptchaApiKey = settings.twoCaptchaApiKey;
            shareCheckouts = settings.shareCheckouts;
            if (settings.browserProxies != null)
            {
                this.proxyGroupName = settings.browserProxies.GroupName;
                this.chosenProxyGroup = settings.browserProxies;
            }
            if (proxyGroups != null)
            {
                foreach (ProxyGroup pg in proxyGroups)
                {
                    this.proxyGroups.Add(pg);
                }
            }

            return true;
        }

        public bool SaveConfiguration()
        {

            SettingsModel settings = new SettingsModel();
            settings.isConfigured = true;
            settings.numBrowser = (int)numBrowsers;
            settings.browserProxies = chosenProxyGroup;
            settings.shareCheckouts = this.shareCheckouts;
            settings.twoCaptchaApiKey = this.twoCaptchaApiKey;

            string strJson = JsonSerializer.Serialize<SettingsModel>(settings);

            _dataService.SaveSettings(settings);
            _backendService.ConfigureSettings(strJson);

            return true;

        }

    }
}
