using Bermuda.Interfaces;
using Bermuda.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Threading;

namespace Bermuda.ViewModels
{
    public class CheckoutViewModel : BindableBase
    {

        private Dispatcher mainDispatcher;
        public ObservableCollection<CheckoutModel> checkouts { get; set; }
        private EventHandler<CheckoutUpdate> checkoutEventHandler;

        public CheckoutViewModel(IDataService dataService, IBackendService backendService)
        {
            _dataService = dataService;
            _backendService = backendService;
            PopulateItems();
        }

        private bool ManualCleanup()
        {
            this.UnsubscribeCheckouts();
            this.checkouts = null;
            return true;
        }

        private async void _ApplyTaskEvent(CheckoutUpdate update)
        {

            // https://stackoverflow.com/questions/19341591/the-application-called-an-interface-that-was-marshalled-for-a-different-thread
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            this.mainDispatcher.InvokeAsync(() =>
            {

                CheckoutModel uCheckout = new CheckoutModel
                {
                    ProfileName = update.ProfileName,
                    Size = update.ProductSize,
                    SKU = update.ProductSKU,
                    ProductImage = update.ProductImage,
                    ProductSite = update.ProductSite
                };
                // Backend service will automatically save it, just need to update the local collection
                this.checkouts.Insert(0, uCheckout);

            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            
        }

        private bool SubscribeCheckouts()
        {
            this.mainDispatcher = Dispatcher.CurrentDispatcher;
            this.checkoutEventHandler = (object Sender, CheckoutUpdate update) =>
            {
                this._ApplyTaskEvent(update);
            };

            _backendService.ForwardCheckoutUpdates(
                this.checkoutEventHandler
            );

            return true;
        }

        private bool UnsubscribeCheckouts()
        {
            if (this.checkoutEventHandler != null)
            {
                _backendService.UnsubscribeCheckoutUpdates(
                    this.checkoutEventHandler
                );
                this.checkoutEventHandler = null;
            }
            return true;
        }

        private void PopulateItems()
        {

            this.checkouts = new ObservableCollection<CheckoutModel>();
            Collection<CheckoutModel> savedCheckouts = _dataService.GetCheckouts();

            foreach (CheckoutModel checkout in savedCheckouts)
            {
                //this.checkouts.Add(checkou)
                this.checkouts.Insert(0, checkout);
            }

            SubscribeCheckouts();

        }

    }
}
