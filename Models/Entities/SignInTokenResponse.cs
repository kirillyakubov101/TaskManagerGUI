﻿using System.Text.Json.Serialization;

namespace TaskManagerGUI.Models.Entities
{
    public class SignInTokenResponse
    {
        [JsonPropertyName("tokenType")]
        public string? TokenType { get; set; }

        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }
    }
}
