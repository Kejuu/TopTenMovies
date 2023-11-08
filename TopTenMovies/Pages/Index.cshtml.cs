using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopTenMovies.Model;
using TopTenMovies.Services;
using TopTenMoviesAPI.Model;

namespace TopTenMovies.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMovieService _movieService;

        public string Token { get; set; }
        public List<Movie> Movies { get; set; }


        public IndexModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public void OnGet()
        {

        }

        public async Task OnPostGetMovies()
        {
            if (Movies == null)
            {
                Movies = _movieService.GetMovies().Result.OrderByDescending(movie => movie.Rating).ThenByDescending(movie => movie.Metascore).ToList();
                _movieService.NotifyMoviesReceived("1001235297", Movies);
            }
        }
    }
}