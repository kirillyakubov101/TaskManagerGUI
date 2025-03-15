using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TaskManagerGUI.Interfaces;
using TaskManagerGUI.Models.Entities;

namespace TaskManagerGUI.Services
{
    public class CreateNewTaskHandler(IAuthHandler authHandler, HttpClient httpClient) : ICreateNewTaskHandler
    {
        public async Task<bool> CreateNewTask(CreateNewTaskDto createNewTaskDto)
        {
            if (!authHandler.IsAuthenticated()) { throw new Exception("No token"); }

            var token = authHandler.GetSessionToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/api/tasks");
            var jsonContent = new StringContent(JsonSerializer.Serialize(createNewTaskDto), Encoding.UTF8, "application/json");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = jsonContent;


            var response = await httpClient.SendAsync(request);

            if(!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error: {response.StatusCode}, {errorContent}");
            }

            return response.IsSuccessStatusCode;
        }
    }
}
