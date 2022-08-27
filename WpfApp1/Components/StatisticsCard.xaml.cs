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

namespace Bermuda.Components
{
    /// <summary>
    /// Interaction logic for StatisticsCard.xaml
    /// </summary>
    public partial class StatisticsCard : UserControl
    {

        public StatisticsCard()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        private string _GraphVisibility { get; set; } = "Visible";
        public string GraphVisibility
        {

            get
            {
                return _GraphVisibility;
            }
            set
            {
                _GraphVisibility = value;
            }
        }

        public double TitleFontSize { get; set; } = 16;
        public int TitleColumnSpan { get; set; } = 1;

        public int CardValueFontSize { get; set; } = 18;

        private string _TitleIcon { get; set; } = "ArrowTopRightThick";
        public string TitleIcon {
            get
            {
                return _TitleIcon;
            }
            set
            {
                _TitleIcon = value;
            }
        }

        public bool NoGraph
        {
            set
            {
                if (value == true)
                {
                    TitleColumnSpan = 2;
                    TitleFontSize = 16;
                    CardValueFontSize = 26;
                    GraphVisibility = "Collapsed";
                }
            }
        }



        public string GradientStart { get; set; } = "#222D3D";
        public string GradientEnd { get; set; } = "#0D1117";

        public string CardTitle { get; set; } = "Resell Value";

        public string CardValue { get; set; } = "$30";

    }
}
