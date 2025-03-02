using System.Windows;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Enums;


namespace TaskManagerGUI.Services
{
    public class NavigationService : INavigationService
    {
        private readonly WindowFactoryService _windowFactoryService;
        public NavigationService(WindowFactoryService windowFactoryService)
        {
            _windowFactoryService = windowFactoryService;
        }

        private Dictionary<WindowType, Window> _windows = new();
        public void CloseWindow(WindowType windowType)
        {
            if(_windows.TryGetValue(windowType, out var window))
            {
                window.Hide();
            }
            else
            {
                throw new InvalidOperationException("No WindowType found in the dictionary");
            }
        }

        public void InitMainWindow(Window window)
        {
            _windows.Add(WindowType.LoginWindow, window);
        }

        public void OpenWindow(WindowType windowType)
        {
            if (!_windows.ContainsKey(windowType))
            {
                var newWindowInst = _windowFactoryService.CreateWindow(windowType);
                if (newWindowInst != null)
                {
                    _windows.Add(windowType, newWindowInst);

                }
            }

            _windows[windowType].Show();
        }
    }
}
