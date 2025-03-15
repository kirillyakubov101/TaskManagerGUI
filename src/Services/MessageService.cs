using System.Windows;
using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.Services;

public class MessageService : IMessageService
{
    public void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}
