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
        private ServiceCollection _services;

        public App()
        {
            _services = new ServiceCollection();
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _services.AddSingleton<HttpClient>();
            _services.AddSingleton<INavigationService,NavigationService>();
            _services.AddSingleton<ISignInHandler, SignInHandler>();
            _services.AddSingleton<ISignUpHandler, SignUpHandler>();
            _services.AddSingleton<MainViewModel>();
            _services.AddSingleton<DashboardViewModel>();
            _services.AddSingleton<IAuthHandler, AuthService>();
            _services.AddSingleton<ILoginEnterHandler,LoginEnterHander>();
            _services.AddSingleton<ExceptionHandlerService>();
            _services.AddSingleton<IErrorHandler,ErrorHandler>();

            _services.AddTransient<WindowFactoryService>();
            _services.AddTransient<ICreateNewTaskHandler, CreateNewTaskHandler>();
            _services.AddTransient<SignUpViewModel>();
            _services.AddTransient<IDeleteTaskHander, DeleteTaskHandler>();
            _services.AddTransient<IEditTaskHandler, EditTaskHandler>();

            ServiceProvider = _services.BuildServiceProvider();

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
