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
        private readonly IErrorHandler _errorHandler;

        public LoginEnterHander(HttpClient httpClient, IAuthHandler authHandler, IErrorHandler errorHandler)
        {
            _httpClient = httpClient;
            _authHandler = authHandler;
            _errorHandler = errorHandler;
        }

        public async Task<IEnumerable<UserTaskDto>> GetAllUserTasks()
        {
            if (!_authHandler.IsAuthenticated())
            {
                _errorHandler.HandleError("No token");
                return Enumerable.Empty<UserTaskDto>();
            }
               
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
                return tasks ?? Enumerable.Empty<UserTaskDto>();
            }

            return Enumerable.Empty<UserTaskDto>();
        }

        public async Task<UserInfoDto> GetUserInfo()
        {
            //declare default user
            UserInfoDto userInfoDto = new UserInfoDto()
            {
                UserNickname = "DefaultUser"
            };

            if (!_authHandler.IsAuthenticated())
            {
                _errorHandler.HandleError("No token");
                return userInfoDto;
            }

            // Set up the HTTP GET request with the Authorization header
            var request = new HttpRequestMessage(HttpMethod.Get, "https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/api/identity/GetCurrentUser");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authHandler.GetSessionToken());

            var response = await _httpClient.SendAsync(request);
             

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var userJson = await response.Content.ReadAsStringAsync();
                var userInfo = JsonSerializer.Deserialize<UserInfoDto>(userJson, options);
                if(userInfo != null)
                {
                    userInfoDto = userInfo;
                }
            }

            return userInfoDto;
        }
    }
}
