using Microsoft.Extensions.DependencyInjection;
using SharedModels;
using System;
using System.Windows;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;
using TaskManagerGUI.Models.Validators;

namespace TaskManagerGUI.ViewModel
{
    public class EditTaskWindowViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private string _priority;
        private string _status;
        private DateTime _dueDate;
        private bool _taskEditted = false;

        private readonly IServiceProvider _serviceProvider;
        private UserTaskDto _userTaskDto;
        private EditTaskValidator _editTaskValidator;

        public Action? CloseWindowAction;

        public ICommand ApplyChangesCommand { get; }
        public ICommand CancelCommand { get; }

        public EditTaskWindowViewModel(IServiceProvider serviceProvider, UserTaskDto userTaskDto)
        {
            _serviceProvider = serviceProvider;
            _userTaskDto = userTaskDto;
            _editTaskValidator = new EditTaskValidator();

            Title = userTaskDto.Title;
            Description = userTaskDto.Description;
            Priority = userTaskDto.Priority.ToString();
            Status = userTaskDto.Status.ToString();
            DueDate = (DateTime)userTaskDto.DueDate;

            ApplyChangesCommand = new RelayCommand(ExecuteApplyChangesCommand, CanExecuteApplyChangesCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand, CanExecuteCancelCommand);
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
            return !_taskEditted;
        }

        private async void ExecuteApplyChangesCommand(object parameter)
        {
            var priorityEnum = (UserTaskPriority)Enum.Parse(typeof(UserTaskPriority), Priority);
            var statusEnum = (UserTaskStatus)Enum.Parse(typeof(UserTaskStatus), Status);
           
            DateTime myDateTime = (DateTime)_dueDate;
            if (myDateTime.Kind != DateTimeKind.Utc)
            {
                myDateTime = myDateTime.ToUniversalTime();
            }

            var userTaskEditDto = new UserTaskEditDto
            {
                Title = _title,
                Description = _description,
                DueDate = myDateTime,
                Priority = priorityEnum,
                Status = statusEnum,
            };

            var validationResult = _editTaskValidator.Validate(userTaskEditDto);

            if (!validationResult.IsValid)
            {
                string allErrors = string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage));
                MessageBox.Show(allErrors);
                return;
            }


            var status = await _serviceProvider.GetRequiredService<IEditTaskHandler>().EditTask(_userTaskDto.Id, userTaskEditDto);
            if(status)
            {
                _taskEditted = true;
                MessageBox.Show("Updated");
                CloseWindowAction?.Invoke();
            }
            else
            {
                MessageBox.Show("Wrong");
            }
        }

        private bool CanExecuteCancelCommand(object parameter)
        {
            return true;
        }

        private void ExecuteCancelCommand(object parameter)
        {
            CloseWindowAction?.Invoke();
        }

    }
}
