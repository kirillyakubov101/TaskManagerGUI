using System.Windows;

namespace TaskManagerGUI.Interfaces
{
    public interface IErrorHandler
    {
        void HandleError(string error);
        void HandleError(string caller,string error);
        void HandleError(string error, MessageBoxImage boxImage);
        void HandleError(string caller,string error, MessageBoxImage boxImage);
    }
}
