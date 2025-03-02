using System.Windows;
using TaskManagerGUI.ViewModel;

namespace TaskManagerGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;

            this.Deactivated += MainWindow_Deactivated;
            this.Closed += MainWindow_Closed;
            
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindow_Deactivated(object? sender, EventArgs e)
        {
            if(this.IsFocused)
            (DataContext as MainViewModel).ResetFields();
        }
    }
}
