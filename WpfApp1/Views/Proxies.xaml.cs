using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Bermuda.ViewModels;
using Bermuda.Popups;
using MaterialDesignThemes.Wpf;
using Bermuda.Models;
using System;

namespace Bermuda.Views
{
    /// <summary>
    /// Interaction logic for Proxies.xaml
    /// </summary>
    public partial class Proxies : Page
    {

        public ProxyViewModel ProxyView { get; } = (Application.Current as App).Container.GetService<ProxyViewModel>();

        public Proxies()
        {
            InitializeComponent();
        }

        private async void ProxyGroupAdd_btn_Click(object sender, RoutedEventArgs e)
        {
            var dialogContent = new AddProxyGroup();
            await MaterialDesignThemes.Wpf.DialogHost.Show(dialogContent);
            if (dialogContent.SaveRequested)
            {
                string groupName = dialogContent.GroupName;
                string groupEntires = dialogContent.GroupEntires;
                if (groupName.Length == 0 || groupEntires.Length == 0)
                {
                    // Show error
                    this.SendError("Missing group name or entries.");
                }
                else
                {
                    try
                    {
                        var imported = this.ProxyView.AddGroup(groupName, groupEntires);
                        if (imported == 0)
                        {
                            throw new Exception("Failed to import any profiles");
                        }
                    }catch(Exception err)
                    {
                        this.SendError("Error importing proxies, please make sure to follow import format.");
                    }
                }
            }
        }

        private void ProxyGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count == 0)
            {
                return;
            }
            
            ProxyGroup pGroup = e.AddedItems[0] as ProxyGroup;

            string proxyGroupName = pGroup.GroupName;
            ProxyView.SelectGroup(proxyGroupName); 
            
        }

        private async void SendError(string message)
        {
            var errorDialog = new ErrorPopup(message);
            await MaterialDesignThemes.Wpf.DialogHost.Show(errorDialog);
        }

        private void ProxyGroupRemove_btn_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem = ProxyGroupList.SelectedItem;
            if (selectedItem != null)
            {
                ProxyGroup pGroup = selectedItem as ProxyGroup;
                string proxyGroupName = pGroup.GroupName;
                ProxyView.RemoveGroup(proxyGroupName);
            }
        }
    }
}
