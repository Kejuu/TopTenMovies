using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json.Serialization;
using TopTenMoviesAPI.Model;

namespace TopTenMoviesAPI.Services
{
    public class MovieService : IMovieService
    {
        private readonly IHttpClientFactory _factory;
        public MovieService(IHttpClientFactory factory) 
        {
            _factory = factory;
        }

        public async Task<List<Movie>?> GetMovies()
        {
            var client = _factory.CreateClient();
            client.BaseAddress = new Uri("https://prod-61.westus.logic.azure.com/workflows/984d35048e064b61a0bf18ded384b6cf/triggers/manual/paths/invoke");
            var queryParameters = new Dictionary<string, string>
            {
                { "api-version", "2016-06-01" },
                { "sp", "/triggers/manual/run"},
                { "sv", "1.0"},
                { "sig", "6ZWKl4A16kST4vmDiWuEc94XI5CckbUH5gWqG-0gkAw"},
            };
            var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
            var queryString = await dictFormUrlEncoded.ReadAsStringAsync();

            HttpResponseMessage response = await client.GetAsync($"?{queryString}");
            string json;
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                var movies = JsonConvert.DeserializeObject<Response>(json);
                NotifyMoviesReceived(movies.response);
                return movies.response;
            }
            return null;
        }
        private async void NotifyMoviesReceived(List<Movie> movies)
        {
            var client = _factory.CreateClient();
            
            client.BaseAddress = new Uri("https://prod-62.westus.logic.azure.com/workflows/779069c026094a32bb8a18428b086b2c/triggers/manual/paths/invoke");
            var queryParameters = new Dictionary<string, string>
            {
                { "api-version", "2016-06-01" },
                { "sp", "/triggers/manual/run"},
                { "sv", "1.0"},
                { "sig", "o_zIF50Dd_EpozYSPSZ6cWB5BRQc3iERfgS0m-4gXUo"},
            };
            var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
            var queryString = await dictFormUrlEncoded.ReadAsStringAsync();
            movies = movies.OrderByDescending(movie => movie.Metascore).ToList();
            List<string> titles = movies.Select(movie => movie.Title).ToList();

            Request body = new Request();
            body.RUT = "1001235297";
            body.Peliculas = titles;
            JsonContent content = JsonContent.Create(body);
            var contentLength = JsonConvert.SerializeObject(body).Length;
            content.Headers.Add("Content-Length", contentLength.ToString());
            HttpResponseMessage response = await client.PostAsync($"?{queryString}", content);
        }
    }
}
