using SharedModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using TaskManagerGUI.Interfaces;

namespace TaskManagerGUI.Services
{
    public class EditTaskHandler(IAuthHandler authHandler, HttpClient httpClient,IErrorHandler errorHandler) : IEditTaskHandler
    {
        public async Task<bool> EditTask(int taskId, UserTaskEditDto UserTaskEditDto)
        {
            if (!authHandler.IsAuthenticated()) { errorHandler.HandleError("No Token"); return false; }

            var token = authHandler.GetSessionToken();

            var payload = new
            {
                id = taskId,
                userTaskEditDto = UserTaskEditDto
            };


            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Patch, $"https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/api/tasks/{taskId}")
            {
                Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
                Content = jsonContent
            };

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                errorHandler.HandleError($"Error: {response.StatusCode}, {errorContent}");
                return false;
            }

            return response.IsSuccessStatusCode;
        }
    }
}




