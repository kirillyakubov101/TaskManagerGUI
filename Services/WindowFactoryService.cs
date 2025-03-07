using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TaskManagerGUI.Models.Enums;
using TaskManagerGUI.View;
using TaskManagerGUI.ViewModel;

namespace TaskManagerGUI.Services
{
    public class WindowFactoryService(IServiceProvider serviceProvider)
    {
        public Window? CreateWindow(WindowType windowType)
        {
            Window newWindowInst = null;
            switch(windowType)
            {
                case WindowType.Dashboard:
                    newWindowInst = new DashboardWindow(serviceProvider.GetRequiredService<DashboardViewModel>());
                    break;
                case WindowType.LoginWindow:
                    newWindowInst = new MainWindow(serviceProvider.GetRequiredService<MainViewModel>());
                    break;
                case WindowType.SignUpWindow:
                    newWindowInst = new SignUpWindow(serviceProvider.GetRequiredService<SignUpViewModel>());
                    break;
                default:
                    throw new Exception("No such window type!");

            }

            return newWindowInst;
        }
    }
}
