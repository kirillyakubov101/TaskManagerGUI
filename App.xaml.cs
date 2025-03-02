using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Services;
using TaskManagerGUI.ViewModel;
using TaskManagerGUI.Models.Enums;

namespace TaskManagerGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();

            services.AddSingleton<HttpClient>();
            services.AddSingleton<INavigationService,NavigationService>();
            services.AddSingleton<ISignInHandler, SignInHandler>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<IAuthHandler, AuthService>();
            services.AddSingleton<ILoginEnterHandler,LoginEnterHander>();
            services.AddTransient<WindowFactoryService>();

            ServiceProvider = services.BuildServiceProvider();

            var LoginWindow = ServiceProvider.GetRequiredService<WindowFactoryService>().CreateWindow(WindowType.LoginWindow);
            if(LoginWindow != null )
            {
                ServiceProvider.GetRequiredService<INavigationService>().InitMainWindow(LoginWindow);
                ServiceProvider.GetRequiredService<INavigationService>().OpenWindow(WindowType.LoginWindow);
            }

        }
    }

}
