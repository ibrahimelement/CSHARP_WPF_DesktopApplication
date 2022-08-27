using Bermuda.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Media;
using System;
using System.Threading;
using System.Windows.Threading;

namespace Bermuda.Views
{
    /// <summary>
    /// Interaction logic for License.xaml
    /// </summary>
    public partial class License : Page
    {

        private Dispatcher uiDispatcher;

        public LicenseViewModel LicenseView { get; } = (Application.Current as App).Container.GetService<LicenseViewModel>();
        public License()
        {
            InitializeComponent();
            InitTasks();
        }

        private async void InitTasks()
        {

            this.uiDispatcher = Dispatcher.CurrentDispatcher;

            ThreadStart work = async () =>
            {
                await LicenseView._HandleUpdate();

                _ShowDependenciesWait();
                await LicenseView._CheckDependenciesInstalled();

                _ShowBackendWait();

                LicenseView.SetDispatcher(
                    this.uiDispatcher
                );

               await LicenseView._SpawnBackend();
                _ShowLicense();
                _LoadUserData();
            };

            Thread thread = new Thread(work);
            thread.Start();

            return;

        }

        private void _ShowDependenciesWait()
        {

            this.uiDispatcher.Invoke(() =>
            {
                UpdateForm_DownloadProgress.Visibility = Visibility.Collapsed;
                UpdateForm_JobStatus.Text = "Installing Dependencies";
                UpdateForm_BackendProgress.Visibility = Visibility.Visible;
            });

        }

        private void _ShowBackendWait()
        {

            this.uiDispatcher.Invoke(() =>
            {
                UpdateForm_DownloadProgress.Visibility = Visibility.Collapsed;
                UpdateForm_JobStatus.Text = "Launching BermudaAIO";
                UpdateForm_BackendProgress.Visibility = Visibility.Visible;
            });

        }

        private void _ShowLicense()
        {

            this.uiDispatcher.Invoke(() =>
            {
                UpdatingForm.Visibility = Visibility.Collapsed;
                LicenseForm.Visibility = Visibility.Visible;
            });

        }

        private void _LoadUserData()
        {

            this.uiDispatcher.Invoke(() =>
            {
                if (LicenseView.hasLicenseKey)
                {
                    TxtLicenseKey.Text = LicenseView.storedLicenseKey;
                    _TriggerValidation();
                }
            });

        }

        private async void LicenseBindBtn_Click(object sender, RoutedEventArgs e)
        {
            _TriggerValidation();
        }

        private async void _TriggerValidation()
        {
            string userLicenseKey = TxtLicenseKey.Text;

            if (userLicenseKey.Length > 0)
            {

                LicenseBindBtn.IsEnabled = false;
                TxtLicenseKey.Foreground = new SolidColorBrush(Colors.Lime);
                TxtLicenseKey.Text = "Validating";
                TxtLicenseKey.IsReadOnly = true;

                bool isValid = false;
                try
                {
                    isValid = await LicenseView.ValidateLicense(userLicenseKey);
                }
                catch (Exception error)
                { }

                if (!isValid)
                {
                    TxtLicenseKey.IsReadOnly = false;
                    TxtLicenseKey.Foreground = new SolidColorBrush(Colors.Red);
                    TxtLicenseKey.Text = "Invalid License... try again";
                    TxtLicenseKey.IsReadOnly = true;

                    // Release controls
                    TxtLicenseKey.Foreground = new SolidColorBrush(Colors.White);
                    TxtLicenseKey.IsReadOnly = false;
                    LicenseBindBtn.IsEnabled = true;
                }
                else
                {
                    LicenseView._DevelopmentSaveLicense(userLicenseKey);
                }

            }

        }

    }
}
