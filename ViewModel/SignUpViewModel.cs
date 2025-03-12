using System.Windows;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;
using TaskManagerGUI.Models.Validators;

namespace TaskManagerGUI.ViewModel;

public class SignUpViewModel : ViewModelBase
{
    private string _nickname;
    private string _email;
    private string _password;
    private string _Confirmpassword;

    private readonly INavigationService _navigationService;
    private readonly ISignUpHandler _signUpHandler;
    private readonly SignUpDtoValidator _signUpDtoValidator;
    private readonly IMessageService _messageService;

    public event Action OnSignUpAction;
    private bool _midSignUpProcess = false;
    public ICommand BackToSignInCommand { get; }
    public ICommand SignUpCommand { get; }

    public string Nickname {
        get => _nickname;
        set
        {
            _nickname = value;
            OnPropertyChanged(nameof(Nickname));
        }
    }
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }
    public string ConfirmPassword
    {
        get => _Confirmpassword;
        set
        {
            _Confirmpassword = value;
            OnPropertyChanged(nameof(ConfirmPassword));
        }
    }

    public bool MidSignUpProcess { get => _midSignUpProcess; set => _midSignUpProcess = value; }

    public SignUpViewModel(INavigationService navigationService, ISignUpHandler signUpHandler, IMessageService messageService)
    {
        _navigationService = navigationService;
        BackToSignInCommand = new RelayCommand(ExecuteBackackToSignIn, CanExecuteBackackToSignIn);
        SignUpCommand = new RelayCommand(ExecuteSignUp, CanExecuteSignUp);
        _signUpHandler = signUpHandler;
        _signUpDtoValidator = new SignUpDtoValidator();
        _messageService = messageService;
    }

    private bool CanExecuteBackackToSignIn(object parameter)
    {
        return true;
    }

    private void ExecuteBackackToSignIn(object parameter)
    {
        _navigationService.CloseWindow(Models.Enums.WindowType.SignUpWindow);
        _navigationService.OpenWindow(Models.Enums.WindowType.LoginWindow);
        OnSignUpAction?.Invoke();
    }

    public bool CanExecuteSignUp(object parameter)
    {
        bool correctCreds =  Password == ConfirmPassword && !_midSignUpProcess;
        if (correctCreds) { return true; }
        else
        {
            _midSignUpProcess = false;
            _messageService.ShowMessage("Password and confirm password don't match!");
            return false;
        }

    }

    private async void ExecuteSignUp(object parameter)
    {
        _midSignUpProcess = true;
        var signUpDto = new SignUpDto()
        {
            Nickname = this.Nickname,
            Email = this.Email,
            Password = this.Password,
        };

        var validationResult = _signUpDtoValidator.Validate(signUpDto);

        if (!validationResult.IsValid)
        {
            string allErrors = string.Join("\n", validationResult.Errors.Select(x => x.ErrorMessage));
            MessageBox.Show(allErrors);
            _midSignUpProcess = false;
            return;
        }

        bool canEmailBeUsed = await _signUpHandler.IsEmailAvailable(signUpDto.Email);


        bool status = await _signUpHandler.CreateUser(signUpDto);
        _midSignUpProcess = false;
        if (status)
        {
            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            OnSignUpAction?.Invoke();
            ExecuteBackackToSignIn(null);
        }
    }
}
