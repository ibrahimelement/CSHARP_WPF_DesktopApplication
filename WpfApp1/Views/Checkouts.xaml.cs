using Bermuda.ViewModels;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace Bermuda.Views
{
    /// <summary>
    /// Interaction logic for Checkouts.xaml
    /// </summary>
    public partial class Checkouts : Page
    {

        public CheckoutViewModel CheckoutView { get; } = (Application.Current as App).Container.GetService<CheckoutViewModel>();

        public Checkouts()
        {
            InitializeComponent();
        }

    }
}
