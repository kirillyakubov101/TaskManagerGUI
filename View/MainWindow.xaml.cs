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
            mainViewModel.OnSignInAction += MainViewModel_OnSignInAction;
            this.Closed += MainWindow_Closed;
        }

        private void MainViewModel_OnSignInAction()
        {
            txtEmail.Clear();
            txtpassword.passwordBox.Clear();
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
