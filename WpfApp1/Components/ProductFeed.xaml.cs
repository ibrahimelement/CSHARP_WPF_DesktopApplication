using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml;
using Windows.UI.Notifications;

namespace Bermuda.Components
{
    /// <summary>
    /// Interaction logic for ProductFeed.xaml
    /// </summary>
    public partial class ProductFeed : UserControl
    {

        
        public ProductFeed()
        {
            InitializeComponent();
            this._PopulateProductList();
            this.DataContext = this;
        }

        public class ProductEntry
        {
            public string ProductName { get; set; }
            public string ProductID { get; set; }
            public string RetailPrice { get; set; }
            public string MonitorStatus { get; set; }
            
        }

        public Collection<ProductEntry> ProductList { get; set; }

        private void _PopulateProductList()
        {

            this.ProductList = new Collection<ProductEntry>
            {
                new ProductEntry
                {
                    ProductName = "Yeezy Boost 350 V2",
                    ProductID = "GW3773",
                    RetailPrice = "$220",
                    MonitorStatus = "Monitor"
                }
            };

            ProductMonitorFeed.ItemsSource = this.ProductList;
        }

        public void DoSomething()
        {
            int x = 1000;
        }

        private void MonitorButton_Click(object sender, RoutedEventArgs e)
        {

            new ToastContentBuilder()
           .AddArgument("action", "viewConversation")
           .AddArgument("conversationId", 9814)
           .AddText("Restock Alert")
           .AddText("This feature is still under development.")
           .Show(); // 

        }

    }
}
