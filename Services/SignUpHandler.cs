using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.Services
{
    public class SignUpHandler(INavigationService navigationService, HttpClient httpClient) : ISignUpHandler
    {
        public async Task<bool> CreateUser(string nickname, string email, string password)
        {
            string baseUri = "https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/";
            string endpoint = "api/identity/";
            string requestUri = $"{baseUri}{endpoint}";

            HttpResponseMessage response = null;

            var createNewUserCommand = new
            {
                Nickname = nickname,
                Email = email,
                Password = password
            };

            try
            {
                response = await httpClient.PostAsJsonAsync(requestUri, createNewUserCommand);
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
