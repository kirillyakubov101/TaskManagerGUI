using System.ComponentModel;
using System.Windows.Input;
using TaskManagerGUI.Commands;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;


namespace TaskManagerGUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ICommand SignInCommand { get; set; }

        private readonly ISignInHandler _signInHandler;

        private string _email;
        private string _password;

       

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

        public MainViewModel(ISignInHandler signInHandler)
        {
            this._signInHandler = signInHandler;

            //SignInCommand = new RelayCommand(
            //async (parameter) => await SignInAsync(parameter),
            //(parameter) => CanSignIn(parameter));
            SignInCommand = new RelayCommand(ExecuteSignIn, CanExecuteSignIn);

        }

        public bool CanSignIn(object parameter)
        {
            return true;
        }

        // Method that is called when SignInCommand is executed
        public async Task SignInAsync(object parameter)
        {
            var signInDto = new SignInDto()
            {
                Email = this._email,
                Password = Password
            };

            if (signInDto != null)
            {
                await _signInHandler.SignIn(signInDto);
            }
        }

        private async void ExecuteSignIn(object parameter)
        {
            await SignInAsync(parameter);
        }

        private bool CanExecuteSignIn(object parameter)
        {
            return CanSignIn(parameter);
        }

        public void ResetFields()
        {
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
