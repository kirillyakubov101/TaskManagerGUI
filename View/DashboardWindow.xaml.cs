using System.Windows;
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

       
    }
}
