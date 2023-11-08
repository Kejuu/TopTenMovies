using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TopTenMoviesAPI.Controllers;

namespace UnitTest.Controllers
{
    public class AuthControllerTest
    {
        private AuthController _controller;
        private IConfiguration _configuration;

        public AuthControllerTest()
        {
            var inMemorySettings = new Dictionary<string, string>()
            {
                { "Jwt:Issuer", "kevinissuer" },
                { "Jwt:Key", "218d12mt883gnb8238v" },
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _controller = new AuthController(_configuration);
        }

        [Fact]
        public void Post_ShouldReturnOkWithValidToken()
        {

            // Act
            var result = _controller.Post();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;

            Assert.NotNull(okResult.Value);
            var token = (string)okResult.Value;

            // Validate the JWT token
            var jwtSecurityHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityHandler.ReadJwtToken(token);

            Assert.NotNull(jwtSecurityToken);
            Assert.Equal(_configuration["Jwt:Issuer"], jwtSecurityToken.Issuer);
        }
    }
}
