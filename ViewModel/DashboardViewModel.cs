
using SharedModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.View;

namespace TaskManagerGUI.ViewModel
{
    public class DashboardViewModel
    {
        public ICommand SignOutCommand { get;}
        public ICommand AddNewTaskCommand { get;}

        private readonly IAuthHandler _authHandler;
        private readonly INavigationService _navigationService;
        private readonly ILoginEnterHandler _loginEnterHandler;
        private readonly IServiceProvider _serviceProvider;

        private NewTaskWindow _NewTaskWindow;

        public ObservableCollection<UserTaskDto> Tasks { get; set; } = new ObservableCollection<UserTaskDto>();

        public DashboardViewModel(IAuthHandler authHandler, INavigationService navigationService, ILoginEnterHandler loginEnterHandler, IServiceProvider serviceProvider)
        {
            _authHandler = authHandler;
            _navigationService = navigationService;
            _loginEnterHandler = loginEnterHandler;
            _serviceProvider = serviceProvider;

            SignOutCommand = new RelayCommand(ExecuteSignOut, CanExecuteSignOut);
            AddNewTaskCommand = new RelayCommand(ExcuteAddNewTask,CanExecuteAddNewTask);
        }

        

        public async Task PopulateUserTaskList()
        {
            Tasks.Clear();
            var tasks = await _loginEnterHandler.GetAllUserTasks();
            foreach (var item in tasks)
            {
                Tasks.Add(item);
            }
        }

        private void ExecuteSignOut(object parameter)
        {
            _authHandler.UnregisterSessionToken();
            _navigationService.CloseWindow(Models.Enums.WindowType.Dashboard);
            _navigationService.OpenWindow(Models.Enums.WindowType.LoginWindow);
        }

        private bool CanExecuteSignOut(object parameter)
        {
            return true;
        }

        private bool CanExecuteAddNewTask(object parameter)
        {
            return true;
        }

        private void ExcuteAddNewTask(object parameter)
        {
            _NewTaskWindow = new NewTaskWindow(new NewTaskWindowViewModel(_serviceProvider));
            _NewTaskWindow.ShowDialog();
        }
    }
}
