using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TopTenMoviesAPI.Controllers;
using TopTenMoviesAPI.Model;
using TopTenMoviesAPI.Services;

namespace UnitTest.Controllers
{
    public class MoviesControllerTests
    {
        private MoviesController controller;
        private readonly Mock<IMovieService> mockMovieService;

        public MoviesControllerTests()
        {
            mockMovieService = new Mock<IMovieService>();
        }


        [Fact]
        public async Task GetMovies_ShouldReturnOkWithValidMovies()
        {
            var expectedMovies = new List<Movie>()
            {
                new Movie { Title = "The Shawshank Redemption" },
                new Movie { Title = "The Godfather" },
            };
            mockMovieService.Setup(m => m.GetMovies()).ReturnsAsync(expectedMovies);

            // Act
            controller = new MoviesController(mockMovieService.Object);
            var result =  controller.GetMovies();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;

            Assert.NotNull(okResult.Value);
            var response = (ApiResponse<List<Movie>>)okResult.Value;

            Assert.True(response.Success);
        }

        [Fact]
        public async Task GetMovies_ShouldReturnInternalServerErrorWithUnexpectedError()
        {
            // Arrange
            mockMovieService.Setup(m => m.GetMovies()).ThrowsAsync(new Exception("An unexpected error occurred."));

            // Act
            controller = new MoviesController(mockMovieService.Object);
            var result = controller.GetMovies();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;

            Assert.NotNull(okResult.Value);
            var response = (ApiResponse<List<Movie>>)okResult.Value;

            Assert.False(response.Success);
        }

        [Fact]
        public async Task NotifyMoviesReceived()
        {
            var expectedMovies = new List<string>()
            {
                "The Shawshank Redemption",
                "The Godfather"
            };
            mockMovieService.Setup(m => m.NotifyMoviesReceived("1001235297", expectedMovies));

            // Act
            controller = new MoviesController(mockMovieService.Object);
            var result = controller.NotifyMoviesReceived(new Request() { RUT = "1001235297", Peliculas = expectedMovies });

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;

            Assert.NotNull(okResult.Value);
            var response = (ApiResponse<string>)okResult.Value;

            Assert.True(response.Success);
        }
    }
}
