using SharedModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.Services
{
    public class LoginEnterHander : ILoginEnterHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthHandler _authHandler;

        public LoginEnterHander(HttpClient httpClient, IAuthHandler authHandler)
        {
            _httpClient = httpClient;
            _authHandler = authHandler;
        }

        public async Task<IEnumerable<UserTaskDto>> GetAllUserTasks()
        {
            if (!_authHandler.IsAuthenticated()) { throw new Exception("No token"); }
            // Set up the HTTP GET request with the Authorization header
            var request = new HttpRequestMessage(HttpMethod.Get, "https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/api/tasks");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authHandler.GetSessionToken());

            // Send the request
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var tasksJson = await response.Content.ReadAsStringAsync();
                var tasks = JsonSerializer.Deserialize<IEnumerable<UserTaskDto>>(tasksJson, options);
                return tasks;
            }
            else
            {
                throw new Exception();
            }

        }
    }
}
