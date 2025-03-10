using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;

namespace TaskManagerGUI.Services
{
    public class SignUpHandler(INavigationService navigationService, HttpClient httpClient) : ISignUpHandler
    {
        public async Task<bool> CreateUser(SignUpDto signUpDto)
        {
            string baseUri = "https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/";
            string endpoint = "api/identity/";
            string requestUri = $"{baseUri}{endpoint}";

            try
            {
                var response = await httpClient.PostAsJsonAsync(requestUri, signUpDto);
                if(response.IsSuccessStatusCode == false)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show(errorContent);
                }
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex )
            {
                MessageBox.Show(ex.Message);
                return false;
            }
           

        }
    }
}
