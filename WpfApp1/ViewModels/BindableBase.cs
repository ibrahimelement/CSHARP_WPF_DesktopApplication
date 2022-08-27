using Bermuda.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Bermuda.ViewModels
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected IDataService _dataService;
        protected IBackendService _backendService;
        /*
        protected INavigationService _navigationService;
        */

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T originalValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (Equals(originalValue, newValue))
            {
                return false;
            }
            originalValue = newValue;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
