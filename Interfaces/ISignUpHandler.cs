using TaskManagerGUI.Models.Entities;

namespace TaskManagerGUI.Interfaces;

public interface ISignUpHandler
{
    Task<bool> CreateUser(SignUpDto signUpDto);
    Task<bool> IsEmailAvailable(string email);
}
