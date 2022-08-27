using Bermuda.ViewModels;
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
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using Bermuda.Models;

namespace Bermuda.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {

        public TaskOverviewViewModel TaskView { get; } = (Application.Current as App).Container.GetService<TaskOverviewViewModel>();
        public CheckoutViewModel CheckoutView { get; } = (Application.Current as App).Container.GetService<CheckoutViewModel>();

        public string NumCheckouts { get; set; } = "1";

        public Dashboard()
        {
            this.DataContext = this;
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {

            CheckoutStatCard.CardValue = NumCheckouts;

        }

    }
}
