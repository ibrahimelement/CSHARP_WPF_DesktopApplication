using Bermuda.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Bermuda.ViewModels;
using System.Windows;
using System.Runtime.CompilerServices;
using System;
using Bermuda.Enums;
using System.Configuration;
using MaterialDesignThemes.Wpf;

namespace Bermuda.Popups
{
    /// <summary>
    /// Interaction logic for AddTaskGroup.xaml
    /// </summary>
    public partial class AddTaskGroup : UserControl
    {

        private string[] ShoeSizes =
        {
            "03.5", "04.0", "04.5", "05.0", "05.5", "06.0", "06.5",
            "07.0","07.5","08.0","08.5","09.0","09.5","10.0","10.5",
            "11.0","11.5","12.0","12.5","13.0","14.0","15.0","16.0","17.0","18.0"
        };

        public ObservableCollection<ProfileGroup> profileGroupList { get; set; }
        public ObservableCollection<ProxyGroup> proxyGroupList { get; set; }
        public ListView sizeSelectionList { get; set; }

        public bool saveSelected { get; set; } = false;

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
            }
        }

        public string GroupName { get; set; }

        public TaskOverviewViewModel TaskView { get; } = (Application.Current as App).Container.GetService<TaskOverviewViewModel>();

        public AddTaskGroup()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {

            // Add proxy groups
            Collection<ProxyGroup> pGroups = TaskView.GetProxyGroups();
            this.proxyGroupList = new ObservableCollection<ProxyGroup>();
            foreach(ProxyGroup pGroup in pGroups)
            {
                this.proxyGroupList.Add(pGroup);
            }

            // Add profile groups
            Collection<ProfileGroup> profileGroups = TaskView.GetProfileGroups();
            this.profileGroupList = new ObservableCollection<ProfileGroup>();
            foreach(ProfileGroup pGroup in profileGroups)
            {
                this.profileGroupList.Add(pGroup);
            }

            // Add product sizes
            ListView sizeSelectionList = new ListView();
            this.sizeSelectionList = sizeSelectionList;
            sizeSelectionList.HorizontalContentAlignment = HorizontalAlignment.Center;

            sizeSelectionExpander.Content = this.sizeSelectionList;

            for (int x = 0; x < this.ShoeSizes.Length; x += 4)
            {
                StackPanel sizeEntryGroup = new StackPanel();
                sizeEntryGroup.Orientation = Orientation.Horizontal;

                for (int i = 0; i < 4 && i + x < this.ShoeSizes.Length; i++)
                {
                    CheckBox sizeEntry = new CheckBox();
                    
                    Thickness m;
                    m.Left = 8;
                    sizeEntry.Margin = m;

                    sizeEntry.Checked += (object nSender, RoutedEventArgs ev) =>
                    {
                        sizeSelectionExpander.Header = "Custom";
                    };

                    sizeEntry.Content = $"{this.ShoeSizes[i + x]}";
                    sizeEntryGroup.Children.Add(sizeEntry);
                }

                this.sizeSelectionList.Items.Add(sizeEntryGroup);
            }

            // Add proxy groups and profile groups
            Combo_ProxyGroupSelection.ItemsSource = this.proxyGroupList;
            Combo_ProfileGroupSelection.ItemsSource = this.profileGroupList;

            // Add supported sites
            ObservableCollection<string> siteSelectionList = new ObservableCollection<string>();

            string[] supportedSites = Enum.GetNames(typeof(SupportedSitesEnum));
            foreach (string supportedSite in supportedSites)
            {
                siteSelectionList.Add(supportedSite);
            }

            Combo_SiteSelection.ItemsSource = supportedSites;

        }

        private void NumBrowsersSld_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int newVal = (int)e.NewValue;
            numBrowsers = newVal;

            txtTaskCount.Text = newVal.ToString();
        }

        private void CreateNewTaskGroup_Click(object sender, RoutedEventArgs e)
        {
            this.saveSelected = true;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

    }
}
