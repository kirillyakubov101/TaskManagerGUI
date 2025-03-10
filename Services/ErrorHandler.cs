using System.Windows;
using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.Services;

public class ErrorHandler : IErrorHandler
{
    public void HandleError(string error)
    {
        MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void HandleError(string caller, string error)
    {
        MessageBox.Show($"{caller}: {error}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void HandleError(string error, MessageBoxImage boxImage)
    {
        MessageBox.Show(error, "Error", MessageBoxButton.OK, boxImage);
    }

    public void HandleError(string caller, string error, MessageBoxImage boxImage)
    {
        MessageBox.Show($"{caller}: {error}", "Error", MessageBoxButton.OK, boxImage);
    }
}
