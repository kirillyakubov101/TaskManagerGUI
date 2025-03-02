using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
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

        public SignInHandler(HttpClient httpClient, IAuthHandler authHandler, INavigationService navigationService)
        {
            _httpClient = httpClient;
            _authHandler = authHandler;
            _navigation = navigationService;
        }
        public async Task SignIn(SignInDto signInDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7281/api/identity/login", signInDto);

                if (response == null) { throw new Exception("SignInAsync responseMessage is null"); }
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
                    //ERROR
                    
                    MessageBox.Show("Wrong credentials");
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
