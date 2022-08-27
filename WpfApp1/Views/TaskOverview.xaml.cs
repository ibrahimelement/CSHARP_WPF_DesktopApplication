using Bermuda.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Bermuda.Popups;
using Bermuda.Models;
using System.Collections.ObjectModel;
using Bermuda.Enums;
using System.Diagnostics;

namespace Bermuda.Views
{
    /// <summary>
    /// Interaction logic for TaskOverview.xaml
    /// </summary>
    public partial class TaskOverview : Page
    {

        public TaskOverviewViewModel TaskView { get; } = (Application.Current as App).Container.GetService<TaskOverviewViewModel>();


        public TaskOverview()
        {
            InitializeComponent();
        }

        private string[] ShoeSizes =
        {
            "03.5", "04.0", "04.5", "05.0", "05.5", "06.0", "06.5",
            "07.0","07.5","08.0","08.5","09.0","09.5","10.0","10.5",
            "11.0","11.5","12.0","12.5","13.0","14.0","15.0","16.0","17.0","18.0"
        };

        private async void TaskGroupAdd_btn_ClickAsync(object sender, RoutedEventArgs e)
        {

            var dialogContent = new AddTaskGroup();
            await MaterialDesignThemes.Wpf.DialogHost.Show(dialogContent);
            
            if (!dialogContent.saveSelected)
            {
                return;
            }

            try
            {

                Collection<ProxyGroup> savedProxyGroups = TaskView.GetProxyGroups();
                Collection<ProfileGroup> savedProfileGroups = TaskView.GetProfileGroups();

                string taskGroupName = dialogContent.GroupName;
                string chosenSite = dialogContent.Combo_SiteSelection.SelectedItem.ToString();
                string taskProductId = dialogContent.txtProductSKU.Text.ToString();
                ProxyGroup selectedProxyGroup = dialogContent.Combo_ProxyGroupSelection.SelectedItem as ProxyGroup;
                ProfileGroup selectedProfileGroup = dialogContent.Combo_ProfileGroupSelection.SelectedItem as ProfileGroup;
                string sizeSelectionHeader = dialogContent.sizeSelectionExpander.Header.ToString();
                int numTaskThreads = dialogContent.numBrowsers;
                ListBox sizeSelectionList = dialogContent.sizeSelectionList;

                int supportedSiteEnumIndex = Array.IndexOf(Enum.GetNames(typeof(SupportedSitesEnum)), chosenSite);

                TaskGroup taskGroup = new TaskGroup();
                taskGroup.groupName = taskGroupName;
                taskGroup.numThreads =  numTaskThreads;
                taskGroup.productLink = taskProductId;
                taskGroup.chosenSite = (SupportedSitesEnum)supportedSiteEnumIndex;

                // Get proxy group
                foreach (ProxyGroup proxyGroup in savedProxyGroups)
                {
                    if (proxyGroup.GroupName == selectedProxyGroup.GroupName)
                    {
                        taskGroup.proxyGroup = proxyGroup;
                        break;
                    }
                }

                // Get profile group
                foreach (ProfileGroup profileGroup in savedProfileGroups)
                {
                    if (profileGroup.GroupName == selectedProfileGroup.GroupName)
                    {
                        taskGroup.profileGroup = profileGroup;
                        break;
                    }
                }

                // Get sizes
                bool hasCustomSizes = false;
                Collection<string> chosenSizes = new Collection<string>();
                if (sizeSelectionHeader == "Custom")
                {
                    foreach (StackPanel sizeGroup in sizeSelectionList.Items)
                    {
                        foreach (CheckBox sizeSelection in sizeGroup.Children)
                        {
                            if ((bool)sizeSelection.IsChecked)
                            {
                                hasCustomSizes = true;
                                chosenSizes.Add(sizeSelection.Content.ToString());
                            }
                        }
                    }
                }

                if (hasCustomSizes)
                {
                    taskGroup.chosenSizes = chosenSizes;
                }
                else
                {
                    taskGroup.chosenSizes = new Collection<string>();
                    foreach (string shoeSize in ShoeSizes)
                    {
                        taskGroup.chosenSizes.Add(shoeSize);
                    }
                }

                this.TaskView.AddGroup(taskGroup);

            }
            catch (Exception err)
            {

            }

        }

        private void TaskGroupRemove_btn_Click(object sender, RoutedEventArgs e)
        {

            TaskGroup taskGroup = (TaskGroupList.SelectedItem as TaskGroup);
            this.TaskView.RemoveGroup(taskGroup);
        }

        private void TaskGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count == 0)
            {
                return;
            }

            TaskGroup tGroup = e.AddedItems[0] as TaskGroup;

            string taskGroupName = tGroup.groupName;
            TaskView.SelectGroup(taskGroupName);
            this.UpdateTaskState();
            
        }

        private void UpdateTaskState()
        {
            StartTaskGroupBtn.IsEnabled = !this.TaskView.SelectedGroup.groupState.isGroupActive;
            StopTaskGroupBtn.IsEnabled = this.TaskView.SelectedGroup.groupState.isGroupActive;
        }

        private void StartTaskGroup_Click(object sender, RoutedEventArgs e)
        {
            TaskGroup taskGroup = (TaskGroupList.SelectedItem as TaskGroup);
            try
            {
                this.TaskView.StartTaskGroup(taskGroup);
                UpdateTaskState();
            }
            catch(Exception err)
            {
                Debug.WriteLine("Error starting task group: " + err.Message);
            }
        }

        private void StopTaskGroup_Click(object sender, RoutedEventArgs e)
        {
            TaskGroup taskGroup = (TaskGroupList.SelectedItem as TaskGroup);
            this.TaskView.StopTaskGroup(taskGroup);
            UpdateTaskState();
        }
    }
}
