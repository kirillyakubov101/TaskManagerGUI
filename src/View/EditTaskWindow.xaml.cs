using System.Windows;
using TaskManagerGUI.ViewModel;

namespace TaskManagerGUI.View
{
    /// <summary>
    /// Interaction logic for EditTaskWindow.xaml
    /// </summary>
    public partial class EditUserTaskWindow : Window
    {
        public EditUserTaskWindow(EditTaskWindowViewModel editTaskWindowViewModel)
        {
            InitializeComponent();
            DataContext = editTaskWindowViewModel;
            editTaskWindowViewModel.CloseWindowAction  = ()=>this.Close();
        }
    }
}
