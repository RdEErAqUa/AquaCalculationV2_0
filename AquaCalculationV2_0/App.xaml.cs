using AquaCalculationV2_0.Servises;
using AquaCalculationV2_0.Servises.Integrals;
using AquaCalculationV2_0.Servises.Integrals.Interfaces;
using AquaCalculationV2_0.ViewModels;
using AquaCalculationV2_0.ViewModels.Lab1;
using AquaCalculationV2_0.ViewModels.Lab2;
using AquaCalculationV2_0.ViewModels.Lab3;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AquaCalculationV2_0
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();

        private readonly ServiceProvider _serviceProvider;
        public App()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();

            displayRootRegistry.RegisterWindowType<MainWindowViewModel, MainWindow>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Lab1ViewModel>();
            services.AddSingleton<Lab2ViewModel>();
            services.AddSingleton<Lab3ViewModel>();

            services.AddSingleton<MainWindowViewModel>();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            await displayRootRegistry.ShowModalPresentation(_serviceProvider.GetService<MainWindowViewModel>());

            Shutdown();
        }
    }
}
