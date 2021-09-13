using System.Text.Json.Serialization;

namespace Common.Api.Responses
{
    public class LoginResult
    {

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
