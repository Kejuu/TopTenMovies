using Newtonsoft.Json;
using TopTenMovies.Model;
using TopTenMoviesAPI.Model;

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
            _httpClient.BaseAddress = new Uri("https://localhost:7049/api/Movies/");
        }

        public async Task<List<Movie>?> GetMovies()
        {
            GenerateToken();
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

        public async void NotifyMoviesReceived(string id, List<Movie> movies)
        {
            GenerateToken();
            var body = new Request();
            body.RUT = "1001235297";
            movies = movies.OrderByDescending(movie => movie.Metascore).ToList();
            List<string> titles = movies.Select(movie => movie.Title).ToList();
            body.Peliculas = titles;
            var x = await _httpClient.PostAsJsonAsync($"NotifyMoviesReceived", body);
            Console.Write(x);
        }

        private async void GenerateToken()
        {
            if (string.IsNullOrEmpty(Token))
            {
                Token = _authService.GetToken().Result;
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            }
        }
    }
}
