using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bermuda.Enums;
using Bermuda.ViewModels;
using System.Windows.Media;
using System.Text.Json;


namespace Bermuda.Models
{


    public class TaskGroupState : BindableBase
    {
        public string groupName;
        public Collection<bool> threadActivationList;
        private bool _isGroupActive = false;
        public bool isGroupActive
        {
            get
            {
                return _isGroupActive;
            }
            set
            {
                _isGroupActive = value;
                OnPropertyChanged();
            }
        }
    }

    public class TaskGroup : BindableBase
    {
        public TaskGroupState groupState;
        public SupportedSitesEnum chosenSite { get; set; }
        public ProxyGroup proxyGroup { get; set; }
        public ProfileGroup profileGroup { get; set; }
        public Collection<string> chosenSizes { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<Task> taskList { get; set; }

        public string groupName { get; set; }
        public string productLink { get; set; }
        public int numThreads { get; set; }
    }

    public class Task : BindableBase
    {
        
        public SupportedSitesEnum chosenSite { get; set; }
        public string strChosenSite { get; set; }

        private StatusEnum status;

        [System.Text.Json.Serialization.JsonIgnore]
        public string PlaceholderSize { get; set; } = "Custom";

        public string productLink { get; set; }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private System.Windows.Media.Brush _statusColor;
        public System.Windows.Media.Brush StatusColor
        {
            get
            {
                return _statusColor;
            }
            set
            {
                _statusColor = value;
                OnPropertyChanged();
            }
        }


    }


}
