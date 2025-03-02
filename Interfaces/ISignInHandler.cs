using System.Net.Http;
using TaskManagerGUI.Models.Entities;

namespace TaskManagerGUI.Interfaces
{
    public interface ISignInHandler
    {
        Task SignIn(SignInDto signInDto);
    }
}
