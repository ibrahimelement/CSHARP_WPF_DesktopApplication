using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bermuda.Models;
using Bermuda.Popups;
using Bermuda.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Bermuda.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {

        public SettingsViewModel SettingsView { get; } = (Application.Current as App).Container.GetService<SettingsViewModel>();

        public Settings()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (SettingsView.chosenProxyGroup != null)
            {
                combo_browserProxyGroupSelection.SelectedItem = SettingsView.chosenProxyGroup;
            }
        }

        private void SaveSettingsBtn_Click(object sender, RoutedEventArgs e)
        {


            if (combo_browserProxyGroupSelection.SelectedItem != null)
            {
                SettingsView.chosenProxyGroup = combo_browserProxyGroupSelection.SelectedItem as ProxyGroup;
            }

            if (SettingsView.chosenProxyGroup != null)
            {
                SettingsView.SaveConfiguration();
                //NotifyUser(true, "Saved", "Updated settings");
                return;
            }

            SendError("Error saving settings, please verify that browser proxies have been configured.");

            //NotifyUser(false, "Error", "Please setup or select a proxy group");

        }

        private async void SendError(string message)
        {
            var errorDialog = new ErrorPopup(message);
            await MaterialDesignThemes.Wpf.DialogHost.Show(errorDialog);
        }

    }
}
