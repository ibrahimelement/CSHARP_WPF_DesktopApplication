using Bermuda.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Bermuda.ViewModels
{

    public class LicenseViewModel : BindableBase
    {


        private Dispatcher mainDispatcher { get; set; }
        public string storedLicenseKey;

        private bool _hasLicenseKey = false;
        public bool hasLicenseKey
        {
            get
            {
                return _hasLicenseKey;
            }
            set
            {
                _hasLicenseKey = value;
                OnPropertyChanged();
            }
        }

        private int _updateProgress = 0;
        public int UpdateProgress
        {
            get
            {
                return _updateProgress;
            }
            set
            {
                _updateProgress = value;
                OnPropertyChanged();
            }
        }


        public LicenseViewModel(IDataService dataService, IBackendService backendService)
        {
            _dataService = dataService;
            _backendService = backendService;
            _PopulateData();
        }

        public async Task<Boolean> _HandleUpdate()
        {
            this.mainDispatcher = Dispatcher.CurrentDispatcher;
            bool updateAvailable = await _backendService.CheckUpdate();

            if (updateAvailable)
            {
                EventHandler<int> eventHandler = (Object sender, int update) =>
                {
                    _UpdateDownloadProgress(update);
                };
                await _backendService.DownloadLatestUpdate(eventHandler);
            }

            return true;

        }

        public void SetDispatcher(Dispatcher dispatcher)
        {
            this.mainDispatcher = dispatcher;
        }

        public async Task<Boolean> _CheckDependenciesInstalled()
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            currentDirectory = Path.Join(currentDirectory, "Assets");
            string installIndicator = Path.Join(currentDirectory, "installed.txt");

            if (File.Exists(installIndicator))
            {
                return true;
            }

            string assetsLocation = currentDirectory;
            string installerLocation = Path.Join(assetsLocation, "Install.bat");
            Process proc = new Process();
            proc.StartInfo.WorkingDirectory = assetsLocation;
            proc.StartInfo.FileName = "Install.bat";
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            proc.Start();
            proc.WaitForExit();

            return true;
        }

        public async Task<Boolean> _SpawnBackend()
        {

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            EventHandler<bool> eventHandler = (Object sender, bool update) =>
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                this.mainDispatcher.InvokeAsync(() =>
                {
                    tcs.SetResult(update);
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed                
            };
            _backendService.BackendOnlineEvent(eventHandler);

            _backendService.LaunchBackend();

            /*
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
            */

            await tcs.Task;
            

            return true;
        }

        private void _UpdateDownloadProgress(int progress)
        {
            
            // https://stackoverflow.com/questions/19341591/the-application-called-an-interface-that-was-marshalled-for-a-different-thread
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.mainDispatcher.InvokeAsync(() =>
            {
                UpdateProgress = progress;
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

        }

        public async Task<bool> ValidateLicense(string licenseKey)
        {

            int statusCode = await _backendService.ValidateLicense(licenseKey);
            if (statusCode == 200)
            {
                _dataService.SaveLicense(licenseKey);
                return true;
            }

            return false;

        }

        public void _DevelopmentSaveLicense(string licenseKey)
        {
            _dataService.SaveLicense(licenseKey);
        }

        private void _PopulateData()
        {

            string licenseKey = _dataService.GetLicense();
            if (licenseKey != null && licenseKey.Length > 0)
            {
                hasLicenseKey = true;
                storedLicenseKey = licenseKey;
            }

        }

    }

}
