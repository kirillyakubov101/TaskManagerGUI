using System.Windows;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.ViewModel;

public class SignUpViewModel : ViewModelBase
{
    private string _nickname;
    private string _email;
    private string _password;
    private string _Confirmpassword;

    private readonly INavigationService _navigationService;
    private readonly ISignUpHandler _signUpHandler;

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

    public SignUpViewModel(INavigationService navigationService, ISignUpHandler signUpHandler)
    {
        _navigationService = navigationService;
        BackToSignInCommand = new RelayCommand(ExecuteBackackToSignIn, CanExecuteBackackToSignIn);
        SignUpCommand = new RelayCommand(ExecuteSignUp, CanExecuteSignUp);
        _signUpHandler = signUpHandler;
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

    private bool CanExecuteSignUp(object parameter)
    {
        bool correctCreds =  Password == ConfirmPassword && !_midSignUpProcess;
        if (correctCreds) { return true; }
        else
        {
            MessageBox.Show("Wrong Input");
            return false;
        }

    }

    private async void ExecuteSignUp(object parameter)
    {
        _midSignUpProcess = true;
        bool status = await _signUpHandler.CreateUser(Nickname,Email,Password);
        _midSignUpProcess = false;
        if (status)
        {
            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            OnSignUpAction?.Invoke();
            ExecuteBackackToSignIn(null);
        }
        else
        {
            MessageBox.Show("Error");
        }
    }
}
