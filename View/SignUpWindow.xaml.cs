using System.Windows;
using TaskManagerGUI.ViewModel;

namespace TaskManagerGUI.View
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow(SignUpViewModel signUpViewModel)
        {
            InitializeComponent();
            DataContext = signUpViewModel;
            this.Closed += SignUpWindow_Closed;
            signUpViewModel.OnSignUpAction += SignUpViewModel_OnSignUpAction;
        }

        private void SignUpViewModel_OnSignUpAction()
        {
            this.txtEmail.Clear();
            this.txtNickname.Clear();
            this.txtPassword.passwordBox.Clear();
            this.txtConfirmPassword.passwordBox.Clear();
        }

        private void SignUpWindow_Closed(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
