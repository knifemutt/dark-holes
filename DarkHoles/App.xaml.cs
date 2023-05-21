using DarkHoles.Model.BPlug;
using DarkHoles.Model.MemoryUtilities;
using DarkHoles.UI;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System;

namespace DarkHoles
{
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;
        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IMainWindowView, MainWindowView>();
            services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<IDarkSoulsIIIMemoryUtilities, DarkSoulsIIIMemoryUtilities>();
            services.AddSingleton<IVibeAdmin, VibeAdmin>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                var mainWindow = serviceProvider.GetRequiredService<IMainWindowView>();
                mainWindow.Show();

            } 
            catch(Exception ex)
            {

            }
        }
    }
}
