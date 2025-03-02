using SharedModels;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.ViewModel
{
    public class DashboardViewModel
    {
        public ICommand SignOutCommand { get;}

        private readonly IAuthHandler _authHandler;
        private readonly INavigationService _navigationService;
        private readonly ILoginEnterHandler _loginEnterHandler;

        public ObservableCollection<UserTaskDto> Tasks { get; set; } = new ObservableCollection<UserTaskDto>();

        public DashboardViewModel(IAuthHandler authHandler, INavigationService navigationService, ILoginEnterHandler loginEnterHandler)
        {
            _authHandler = authHandler;
            _navigationService = navigationService;
            _loginEnterHandler = loginEnterHandler;
            SignOutCommand = new RelayCommand(ExecuteSignOut, CanExecuteSignOut);
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
    }
}
