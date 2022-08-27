using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace Bermuda.Popups
{
    /// <summary>
    /// Interaction logic for AddProfile.xaml
    /// </summary>
    public partial class AddProfile : UserControl
    {

        public bool SaveRequested { get; set; }

        public string ProfileName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; } = "";
        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string ZipCode { get; set; }
        public string CardNumber { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public string CardCVC { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public AddProfile()
        {
            InitializeComponent();
        }

        public void SaveNewProfile_Click(object sender, RoutedEventArgs e)
        {
            SaveRequested = true;
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

}
}
