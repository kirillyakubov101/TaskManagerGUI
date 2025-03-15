using Microsoft.Extensions.DependencyInjection;
using SharedModels;
using System.Windows;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;
using FluentValidation;
using TaskManagerGUI.Models.Validators;

namespace TaskManagerGUI.ViewModel
{
    public class NewTaskWindowViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private string _priority = "Low";     // Default priority
        private DateTime? _dueDate = DateTime.Today;

        private IServiceProvider _serviceProvider;

        //validators
        private readonly CreateNewTaskDtoValidator _createNewTaskDtoValidator;

        // Commands
        public ICommand CreateTaskCommand { get; }
        public ICommand CancelCommand { get; }

        public Action? CloseWindowAction;

        public bool IsTaskCreated { get; private set; } = false;

        public NewTaskWindowViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _createNewTaskDtoValidator = new CreateNewTaskDtoValidator();
            CancelCommand = new RelayCommand(ExecuteCancelCommand, CanCancelCommand);
            CreateTaskCommand = new RelayCommand(ExecuteCreateTaskCommand, CanCreateTaskCommand);
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

        public DateTime? DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        private bool CanCancelCommand(object parameter)
        {
            return true;
        }

        private void ExecuteCancelCommand(object parameter)
        {
            CloseWindowAction?.Invoke();
        }

        private bool CanCreateTaskCommand(object parameter)
        {
            return true;
        }

        private async void ExecuteCreateTaskCommand(object parameter)
        {
            var priorityEnum = (UserTaskPriority)Enum.Parse(typeof(UserTaskPriority), Priority);

            DateTime myDateTime = (DateTime)_dueDate; // Your DateTime value
            if (myDateTime.Kind != DateTimeKind.Utc)
            {
                myDateTime = myDateTime.ToUniversalTime();  // Convert to UTC if it's not already
            }


            CreateNewTaskDto createNewTaskDto = new CreateNewTaskDto()
            {
                Title = Title,
                Description = Description,
                Priority = priorityEnum,
                DueDate = myDateTime,
            };

            var validationResult = _createNewTaskDtoValidator.Validate(createNewTaskDto);

            if (!validationResult.IsValid)
            {
                string allErrors = string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage));
                MessageBox.Show(allErrors);
                ClearAllInput();
                return;
            }

            bool successful = await _serviceProvider.GetRequiredService<ICreateNewTaskHandler>().CreateNewTask(createNewTaskDto);

            if (successful)
            {
                CloseWindowAction?.Invoke();
            }

        }

        private void ClearAllInput()
        {
            Priority = "Low";
            Title = string.Empty;
            Description = string.Empty;
            DueDate = DateTime.Today;
        }


    }
}
