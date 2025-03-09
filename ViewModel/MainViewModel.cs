using System.Windows;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;
using TaskManagerGUI.Models.Validators;


namespace TaskManagerGUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand SignInCommand { get;}
        public ICommand RegisterCommand { get;}
        public event Action OnSignInAction;

        private readonly ISignInHandler _signInHandler;
        private readonly INavigationService _navigationService;
        private readonly SignInDtoValidator _signInDtoValidator;


        private string _email;
        private string _password;
        private bool _isLoading;

        public MainViewModel(ISignInHandler signInHandler, INavigationService navigationService)
        {
            this._signInHandler = signInHandler;
            this._navigationService = navigationService;
            this._signInDtoValidator = new SignInDtoValidator();
            SignInCommand = new RelayCommand(ExecuteSignIn, CanExecuteSignIn);
            RegisterCommand = new RelayCommand(ExecuteRegisterNewUser, CanExecuteRegisterNewUser);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                ((RelayCommand)SignInCommand).RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ((RelayCommand)SignInCommand).RaiseCanExecuteChanged();
            }
        }



        private bool CanExecuteRegisterNewUser(object parameter)
        {
            return true;
        }
        private void ExecuteRegisterNewUser(object parameter)
        {
            _navigationService.OpenWindow(Models.Enums.WindowType.SignUpWindow);
            _navigationService.CloseWindow(Models.Enums.WindowType.LoginWindow);
            OnSignInAction?.Invoke();
        }

        private async Task SignInAsync(object parameter)
        {
            var signInDto = new SignInDto()
            {
                Email = this.Email,
                Password = this.Password
            };

            var validationResult = _signInDtoValidator.Validate(signInDto);

            if (!validationResult.IsValid)
            {
                string allErrors = string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage));
                MessageBox.Show(allErrors);
                return;
            }


            IsLoading = true;

            try
            {
                if (signInDto != null)
                {
                    await _signInHandler.SignIn(signInDto);
                    OnSignInAction?.Invoke();
                }
            }

            catch (Exception ex)
            {
                throw new Exception();
            }

            finally
            {
                IsLoading = false;
            }

           

        }

        private async void ExecuteSignIn(object parameter)
        {
            await SignInAsync(parameter);
        }

        private bool CanExecuteSignIn(object parameter)
        {
            //TODO: add a correct validation
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
        }

    }
}
