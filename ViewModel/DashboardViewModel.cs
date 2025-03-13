
using SharedModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.View;

namespace TaskManagerGUI.ViewModel
{
    public class DashboardViewModel : ViewModelBase
    {
        public ICommand SignOutCommand { get;}
        public ICommand AddNewTaskCommand { get;}
        public ICommand DeleteTaskCommand { get;}
        public ICommand EditTaskCommand { get;}




        private readonly IAuthHandler _authHandler;
        private readonly INavigationService _navigationService;
        private readonly ILoginEnterHandler _loginEnterHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDeleteTaskHander _deleteTaskHander;

        private UserTaskDto _currentSelectedTask;

        public UserTaskDto CurrentSelectedTask
        {
            get => _currentSelectedTask;
            set
            {
                _currentSelectedTask = value;
                OnPropertyChanged(nameof(CurrentSelectedTask));
            }
        }


        public ObservableCollection<UserTaskDto> Tasks { get; set; } = new ObservableCollection<UserTaskDto>();

        public DashboardViewModel(IAuthHandler authHandler, INavigationService navigationService, ILoginEnterHandler loginEnterHandler, IServiceProvider serviceProvider, IDeleteTaskHander deleteTaskHander)
        {
            _authHandler = authHandler;
            _navigationService = navigationService;
            _loginEnterHandler = loginEnterHandler;
            _serviceProvider = serviceProvider;
            _deleteTaskHander = deleteTaskHander;

            SignOutCommand = new RelayCommand(ExecuteSignOut, CanExecuteSignOut);
            AddNewTaskCommand = new RelayCommand(ExcuteAddNewTask,CanExecuteAddNewTask);
            DeleteTaskCommand = new RelayCommand(ExecuteDeleteTask, CanExecuteDeleteTask);
            EditTaskCommand = new RelayCommand(ExecuteEditTask, CanExecuteEditTask);
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
            NewTaskWindow NewTaskWindow = new NewTaskWindow(new NewTaskWindowViewModel(_serviceProvider));
            NewTaskWindow.ShowDialog();
        }

        private bool CanExecuteDeleteTask(object parameter)
        {
            return true;
        }

        private async void ExecuteDeleteTask(object parameter)
        {
            var taskToDelete = parameter as UserTaskDto;
            if (taskToDelete != null)
            {
                var status = await _deleteTaskHander.Delete(taskToDelete.Id);
                if(status)
                {
                    MessageBox.Show($"Task: {taskToDelete.Title} was removed");
                }
                else
                {
                    MessageBox.Show("failed");
                }
            }
        }

        private bool CanExecuteEditTask(object parameter)
        {
            return true;
        }

        private void ExecuteEditTask(object parameter)
        {
            var taskToEdit = parameter as UserTaskDto;
            if(taskToEdit != null)
            {
                EditUserTaskWindow editTaskWindow = new EditUserTaskWindow(new EditTaskWindowViewModel(_serviceProvider, taskToEdit));
                editTaskWindow.ShowDialog();
            }
           
        }
    }
}
