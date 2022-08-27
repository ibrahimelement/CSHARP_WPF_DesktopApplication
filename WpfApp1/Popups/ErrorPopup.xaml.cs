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

namespace Bermuda.Popups
{
    /// <summary>
    /// Interaction logic for ErrorPopup.xaml
    /// </summary>
    public partial class ErrorPopup : UserControl
    {

        public string ErrorMessage { get; set; }

        public ErrorPopup(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
            InitializeComponent();
        }
    }
}
