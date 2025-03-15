namespace TaskManagerGUI.Interfaces
{
    public interface IAuthHandler
    {
        void RegisterSessionToken(string sessionToken);
        void UnregisterSessionToken();

        public bool IsAuthenticated();
        string? GetSessionToken();
    }
}
