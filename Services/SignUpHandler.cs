using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;

namespace TaskManagerGUI.Services
{
    public class SignUpHandler(INavigationService navigationService, HttpClient httpClient, IErrorHandler errorHandler) : ISignUpHandler
    {
        public async Task<bool> CreateUser(SignUpDto signUpDto)
        {
            string baseUri = "https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/";
            string endpoint = "api/identity/";
            string requestUri = $"{baseUri}{endpoint}";

            var response = await httpClient.PostAsJsonAsync(requestUri, signUpDto);
            if (response.IsSuccessStatusCode == false)
            {
                errorHandler.HandleError(response.ReasonPhrase!, MessageBoxImage.Error);
            }

            return response.IsSuccessStatusCode;



        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            string baseUri = "https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/";
            string endpoint = "api/identity/";
            string requestUri = $"{baseUri}{endpoint}?email={email}";

            var response = await httpClient.GetAsync(requestUri);

            if(response.IsSuccessStatusCode == false)
            {
                var json = await response.Content.ReadAsStringAsync();
                var errorContent = JsonSerializer.Deserialize<SignUpEmailErrorResponse>(json);
                if(errorContent.message != null)
                {
                    errorHandler.HandleError(errorContent.message, MessageBoxImage.Error);
                }
                else
                {
                    errorHandler.HandleError("Available email validation failed", MessageBoxImage.Error);
                }
            }
                  
               

            return response.IsSuccessStatusCode;
        }
    }

    public class SignUpEmailErrorResponse
    {
        public string message { get; set; }
    }
}
