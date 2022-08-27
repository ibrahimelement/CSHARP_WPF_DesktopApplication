using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for AddProfileGroup.xaml
    /// </summary>
    public partial class AddProfileGroup : UserControl
    {

        public string GroupName { get; set; } = "";
        public string GroupEntires { get; set; } = "";
        public bool SaveRequested { get; set; } = false;

        public AddProfileGroup()
        {
            InitializeComponent();
        }

        private void CreateNewProfileGroup_Click(object sender, RoutedEventArgs e)
        {
            SaveRequested = true;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

    }
}
