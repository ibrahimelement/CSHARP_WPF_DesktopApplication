using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bermuda.ViewModels;
using Bermuda.Views;
using Microsoft.Extensions.DependencyInjection;


namespace Bermuda
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainViewModel MainView { get; } = (Application.Current as App).Container.GetService<MainViewModel>();
        private object currentPage { get; set; }
        private string currentPageType = "";
        private bool licensedSession;

        public MainWindow()
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
            InitNavigation();
        }

        private void InitNavigation()
        {

            
            /*
            NavigationTabControl.IsEnabled = true;
            this.licensedSession = true;
            */

            
            LicensePressed();

            EventHandler<bool> eventHandler = (Object sender, bool data) =>
            {
                this.licensedSession = true;
                NavigationTabControl.IsEnabled = true;
                this.TaskOverviewSelected();
            };

            MainView.SubscribeLicenseEvent(eventHandler);
            
            //this.Settings.IsSelected = false;
            
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            try
            {

                if (!this.licensedSession) return;

                this.CleanPrevious();
                   
                string tabItem = (((sender as TabControl)).SelectedItem as TabItem).Name.ToString();
                currentPageType = tabItem;

                switch (tabItem)
                {
                    case "Profiles":
                        MainView.pageTitle = "Profiles";
                        this.ProfilesSelected();
                        break;
                    case "Proxies":
                        MainView.pageTitle = "Proxies";
                        this.ProxiesSelected();
                        break;
                    case "Settings":
                        MainView.pageTitle = "Settings";
                        this.SettingsSelected();
                        break;
                    case "Checkouts":
                        MainView.pageTitle = "Checkouts";
                        this.CheckoutsPressed();
                        break;
                    case "TaskOverview":
                        MainView.pageTitle = "Task Overview";
                        this.TaskOverviewSelected();
                        break;
                    case "DashOverview":
                        MainView.pageTitle = "Dash Overview";
                        this.DashOverviewPressed();
                        break;
                    case "License":
                        MainView.pageTitle = "License";
                        this.LicensePressed();
                        break;
                }
            }
            catch(Exception err)
            {
                Debug.WriteLine("Error", err.Message);
            }

        }

        private void CleanPrevious()
        {
            switch (this.currentPageType)
            {
                case "Profiles":
                    (this.currentPage as Profiles).ProfileView.ManualCleanup();
                    break;
                case "Proxies":
                    (this.currentPage as Proxies).ProxyView.ManualClean();
                    break;
                case "Settings":
                    //(this.currentPage as Settings).SettingsView.
                    break;
                case "Checkouts":
                    //(this.currentPage as Checkouts).CheckoutView.
                    break;
                case "TaskOverview":
                    (this.currentPage as TaskOverview).TaskView.ManualClean();
                    break;
            }
        }

        private void ProfilesSelected()
        {
            this.currentPage = new Profiles();
            this.PageFrame.Navigate(this.currentPage);
        }

        private void ProxiesSelected()
        {
            this.currentPage = new Proxies();
            this.PageFrame.Navigate(this.currentPage);
        }

        private void TaskOverviewSelected()
        {
            this.currentPage = new TaskOverview();
            this.PageFrame.Navigate(this.currentPage);
        }

        private void CheckoutsPressed()
        {
            this.currentPage = new Checkouts();
            this.PageFrame.Navigate(this.currentPage);
        }

        private void DashOverviewPressed()
        {
            this.currentPage = new Dashboard();
            this.PageFrame.Navigate(this.currentPage);
        }

        private void SettingsSelected()
        {
            this.currentPage = new Settings();
            this.PageFrame.Navigate(this.currentPage);
        }

        private void LicensePressed()
        {
            this.currentPage = new License();
            this.PageFrame.Navigate(this.currentPage);
        }

        // Top bar

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            //SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
            System.Environment.Exit(0);
        }

        // State change
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }

    }
}
