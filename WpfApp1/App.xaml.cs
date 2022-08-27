using System;
using System.Windows;
using Bermuda.Interfaces;
using Bermuda.Services;
using Bermuda.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Bermuda
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public IServiceProvider Container { get; private set; }

        public App()
        {
            this.Container = RegisterServices();
        }

        private IServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();
            var dataService = new DataService();
            var backendService = new BackendService(dataService);

            services.AddSingleton<IDataService>(dataService);
            services.AddSingleton<IBackendService>(backendService);

            services.AddTransient<MainViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<ProxyViewModel>();
            services.AddTransient<CheckoutViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<LicenseViewModel>();
            services.AddTransient<TaskOverviewViewModel>();

            return services.BuildServiceProvider();
        }

    }
}
