using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;
using TaskManagerGUI.Models.Enums;

namespace TaskManagerGUI.Services
{
    public class SignInHandler : ISignInHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthHandler _authHandler;
        private readonly INavigationService _navigation;
        private readonly IErrorHandler _errorHandler;

        public SignInHandler(HttpClient httpClient, IAuthHandler authHandler, INavigationService navigationService, IErrorHandler errorHandler)
        {
            _httpClient = httpClient;
            _authHandler = authHandler;
            _navigation = navigationService;
            _errorHandler = errorHandler;
        }
        public async Task SignIn(SignInDto signInDto)
        {
            HttpResponseMessage response;
            response = await _httpClient.PostAsJsonAsync("https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/api/identity/login", signInDto);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<SignInTokenResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (!string.IsNullOrEmpty(authResponse?.AccessToken))
                {
                    //{ authResponse.TokenType}
                    string token = $"{authResponse.AccessToken}";
                    _authHandler.RegisterSessionToken(token);  // Store token in memory
                    _navigation.OpenWindow(WindowType.Dashboard);
                    _navigation.CloseWindow(WindowType.LoginWindow);
                }
            }

            else
            {
                _errorHandler.HandleError(response.ReasonPhrase!, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
