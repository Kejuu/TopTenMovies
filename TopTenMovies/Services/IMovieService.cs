using TopTenMovies.Model;

namespace TopTenMovies.Services
{
    public interface IMovieService
    {
        public Task<List<Movie>?> GetMovies();
        public void NotifyMoviesReceived(string id, List<Movie> movies);
    }
}
