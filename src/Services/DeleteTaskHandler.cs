﻿using System.Net.Http;
using System.Net.Http.Headers;

using TaskManagerGUI.Interfaces;


namespace TaskManagerGUI.Services;

public class DeleteTaskHandler(IAuthHandler authHandler, HttpClient httpClient,IErrorHandler errorHandler) : IDeleteTaskHander
{
    public async Task<bool> Delete(int taskId)
    {
        if (!authHandler.IsAuthenticated())
        {
            errorHandler.HandleError("No Token");
            return false;
        }

        var token = authHandler.GetSessionToken();
        var requestUri = $"https://taskmanager-api-prod-eydvashjgtasftbj.canadaeast-01.azurewebsites.net/api/tasks/{taskId}";

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await httpClient.DeleteAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            errorHandler.HandleError($"Error: {response.StatusCode}, {errorContent}");
        }

        return response.IsSuccessStatusCode;
    }
}
