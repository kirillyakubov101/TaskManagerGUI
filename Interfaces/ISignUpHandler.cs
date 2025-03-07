namespace TaskManagerGUI.Interfaces;

public interface ISignUpHandler
{
    Task<bool> CreateUser(string username,string email, string password);
}
