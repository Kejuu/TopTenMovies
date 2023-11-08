using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TopTenMoviesAPI.Model;
using TopTenMoviesAPI.Services;

namespace TopTenMoviesAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult GetMovies()
        {
            ApiResponse<List<Movie>> response = new ApiResponse<List<Movie>>();
            try
            {
                var movies = _movieService.GetMovies();
                response.Data = movies.Result;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "An unexpected error occurred.";
            }
            return Ok(response);
        }

        [HttpPost("NotifyMoviesReceived")]
        public IActionResult NotifyMoviesReceived(Request body)
        {
            ApiResponse<string> response = new ApiResponse<string>();
            try
            {
                _movieService.NotifyMoviesReceived(body.RUT, body.Peliculas);
                response.Data = "OK";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = "An unexpected error occurred.";
            }
            return Ok(response);
        }
    }
}