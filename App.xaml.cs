using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Services;
using TaskManagerGUI.ViewModel;
using TaskManagerGUI.Models.Enums;
using TaskManagerGUI.Middleware;

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
            services.AddSingleton<ISignUpHandler, SignUpHandler>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<IAuthHandler, AuthService>();
            services.AddSingleton<ILoginEnterHandler,LoginEnterHander>();
            services.AddSingleton<ExceptionHandlerService>();

            services.AddTransient<WindowFactoryService>();
            services.AddTransient<ICreateNewTaskHandler, CreateNewTaskHandler>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<IDeleteTaskHander, DeleteTaskHandler>();
            services.AddTransient<IEditTaskHandler, EditTaskHandler>();

            ServiceProvider = services.BuildServiceProvider();

            // Global exception handling setup
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            var LoginWindow = ServiceProvider.GetRequiredService<WindowFactoryService>().CreateWindow(WindowType.LoginWindow);
            if(LoginWindow != null )
            {
                ServiceProvider.GetRequiredService<INavigationService>().InitMainWindow(LoginWindow);
                ServiceProvider.GetRequiredService<INavigationService>().OpenWindow(WindowType.LoginWindow);
            }

        }

        // Global UI Thread Exception Handler
        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;  // Prevent app from crashing
            var handler = ServiceProvider.GetRequiredService<ExceptionHandlerService>();
            handler.HandleException(e.Exception);
        }

        // Global Non-UI Thread Exception Handler
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                var handler = ServiceProvider.GetRequiredService<ExceptionHandlerService>();
                handler.HandleException(ex);
            }
        }
    }

}
