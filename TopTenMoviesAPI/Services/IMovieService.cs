using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TopTenMoviesAPI.Model;

namespace TopTenMoviesAPI.Services
{
    public interface IMovieService
    {

        public Task<List<Movie>?> GetMovies();

    }
}
