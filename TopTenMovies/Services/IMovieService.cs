using TopTenMovies.Model;

namespace TopTenMovies.Services
{
    public interface IMovieService
    {
        public Task<List<Movie>?> GetMovies();
    }
}
