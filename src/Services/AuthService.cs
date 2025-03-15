using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.Services
{
    public class AuthService : IAuthHandler
    {
        public string? Token { get; private set; }
        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(Token);
        }

        public string? GetSessionToken()
        {
            return Token;
        }

        public void RegisterSessionToken(string sessionToken)
        {
            Token = sessionToken;
        }
        
        public void UnregisterSessionToken()
        {
            Token = null;
        }

       
    }
}
