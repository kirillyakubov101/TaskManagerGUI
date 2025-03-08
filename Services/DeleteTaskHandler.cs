using System.Net.Http;
using System.Net.Http.Headers;

using TaskManagerGUI.Interfaces;


namespace TaskManagerGUI.Services;

public class DeleteTaskHandler(IAuthHandler authHandler, HttpClient httpClient) : IDeleteTaskHander
{
    public async Task<bool> Delete(int taskId)
    {
        if (!authHandler.IsAuthenticated()) { throw new Exception("No token"); }

        var token = authHandler.GetSessionToken();
        var request = new HttpRequestMessage(HttpMethod.Delete, $"https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/api/tasks/{taskId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);


        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error: {response.StatusCode}, {errorContent}");
        }

        return response.IsSuccessStatusCode;
    }
}
