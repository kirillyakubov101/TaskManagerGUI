using System.Windows;
using TaskManagerGUI.Models.Enums;

namespace TaskManagerGUI.Interfaces
{
    public interface INavigationService
    {
        void OpenWindow(WindowType windowType);
        void CloseWindow(WindowType windowType);
        void InitMainWindow(Window window);
    }
}
