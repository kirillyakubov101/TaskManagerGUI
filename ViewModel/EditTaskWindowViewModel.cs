using Microsoft.Extensions.DependencyInjection;
using SharedModels;
using System.Windows;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.ViewModel
{
    public class EditTaskWindowViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private string _priority;
        private string _status;
        private DateTime _dueDate;

        private readonly IServiceProvider _serviceProvider;
        private UserTaskDto _userTaskDto;

        public ICommand ApplyChangesCommand { get; }
        public ICommand CancelCommand { get; }

        public EditTaskWindowViewModel(IServiceProvider serviceProvider, UserTaskDto userTaskDto)
        {
            _serviceProvider = serviceProvider;
            _userTaskDto = userTaskDto;

            Title = userTaskDto.Title;
            Description = userTaskDto.Description;
            Priority = userTaskDto.Priority.ToString();
            Status = userTaskDto.Status.ToString();
            DueDate = (DateTime)userTaskDto.DueDate;

            ApplyChangesCommand = new RelayCommand(ExecuteApplyChangesCommand, CanExecuteApplyChangesCommand);
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public string Priority
        {
            get => _priority;
            set { _priority = value; OnPropertyChanged(nameof(Priority)); }
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        
        private bool CanExecuteApplyChangesCommand(object parameter)
        {
            return true;
        }

        private async void ExecuteApplyChangesCommand(object parameter)
        {
            var priorityEnum = (UserTaskPriority)Enum.Parse(typeof(UserTaskPriority), Priority);
            var statusEnum = (UserTaskStatus)Enum.Parse(typeof(UserTaskStatus), Status);
            var userTaskEditDto = new UserTaskEditDto
            {
                Title = _title,
                Description = _description,
                DueDate = _dueDate,
                Priority = priorityEnum,
                Status = statusEnum,
            };


            var status = await _serviceProvider.GetRequiredService<IEditTaskHandler>().EditTask(_userTaskDto.Id, userTaskEditDto);
            if(status)
            {
                MessageBox.Show("Updated");
            }
            else
            {
                MessageBox.Show("Wrong");
            }
        }


    }
}
