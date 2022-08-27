using Bermuda.Interfaces;
using System;

namespace Bermuda.ViewModels
{
    public class MainViewModel : BindableBase
    {

        private string _pageTitle;
        public string pageTitle {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void SubscribeLicenseEvent(EventHandler<bool> evHandler)
        {
            // This is only triggered when we decide to save the license, which is always right after processing
            // and successful validation (every reboot).
            
            _dataService.SubscribeLicensed(evHandler);

        }

    }
}
