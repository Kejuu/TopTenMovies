using Newtonsoft.Json;
using TopTenMovies.Model;

namespace TopTenMovies.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _factory;
        private HttpClient _httpClient;
        public AuthService(IHttpClientFactory factory) 
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7049/api/Auth");
        }

        public async Task<string> GetToken()
        {

            HttpResponseMessage response = await _httpClient.PostAsync("", null);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token;
            }
            return null;
        }
    }
}
