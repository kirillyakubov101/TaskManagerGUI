using System.Windows;
using TaskManagerGUI.ViewModel;

namespace TaskManagerGUI.View
{
    /// <summary>
    /// Interaction logic for NewTaskWindow.xaml
    /// </summary>
    public partial class NewTaskWindow : Window
    {
        public NewTaskWindow(NewTaskWindowViewModel newTaskWindowViewModel)
        {
            InitializeComponent();
            DataContext = newTaskWindowViewModel;

            newTaskWindowViewModel.CloseWindowAction = () => this.Close();
        }

     
    }
}
