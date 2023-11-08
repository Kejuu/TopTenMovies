using Newtonsoft.Json;
using TopTenMovies.Model;

namespace TopTenMovies.Services
{
    public class MovieService : IMovieService
    {
        private readonly IHttpClientFactory _factory;
        private HttpClient _httpClient;
        private readonly IAuthService _authService;
        public string Token;
        public MovieService(IHttpClientFactory factory, IAuthService authService) 
        {
            _factory = factory;
            _authService = authService;
            _httpClient = _factory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7049/api/Movies");
        }

        public async Task<List<Movie>?> GetMovies()
        {
            if (string.IsNullOrEmpty(Token))
            {
                Token = _authService.GetToken().Result;
            }
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            HttpResponseMessage response = await _httpClient.GetAsync("");
            string json;
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                var movies = JsonConvert.DeserializeObject<ApiResponse<List<Movie>>>(json);
                return movies.Data;
            }
            return null;
        }
    }
}
