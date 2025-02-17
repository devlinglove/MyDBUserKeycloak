using System.Text.Json;
using WebApplicationTokenSub.Entities;

namespace WebApplicationTokenSub.Services
{
    public class CreateUserService
    {
        private readonly HttpClient _client;

        public CreateUserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<KeyCloakUser?> GetUserAsync()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            KeyCloakUser? user = await _client.GetFromJsonAsync<KeyCloakUser>($"userinfo", options);

            return user;
        }
    }

}


