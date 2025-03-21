using System.Windows;
using System.Windows.Controls;
using TaskManagerGUI.ViewModel;

namespace TaskManagerGUI.View
{
    /// <summary>
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        public DashboardWindow(DashboardViewModel dashboardViewModel)
        {
            InitializeComponent();
            DataContext = dashboardViewModel;

            this.Closed += DashboardWindow_Closed;
            
        }


        protected override async void OnActivated(EventArgs e)
        {
            // Ensure DataContext is properly set
            if (DataContext is DashboardViewModel dashboardViewModel)
            {
                await dashboardViewModel.PopulateUserTaskList();
                await dashboardViewModel.PopulateUserNickname();
            }
            else
            {
                // Handle error or fallback (e.g., log error)
                MessageBox.Show("DataContext is not set correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DashboardWindow_Closed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenContextMenu_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.ContextMenu != null)
            {
                button.ContextMenu.IsOpen = true;
            }
        }

        //private void EditTask_OnClick(object sender, RoutedEventArgs e)
        //{

        //}

        //private void DeleteTask_OnClick(object sender, RoutedEventArgs e)
        //{
        //    var menuItem = sender as MenuItem;
        //    (DataContext as DashboardViewModel).DeleteTaskCommand.Execute(menuItem);
        //}
    }
}
