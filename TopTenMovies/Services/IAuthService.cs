using TopTenMovies.Model;

namespace TopTenMovies.Services
{
    public interface IAuthService
    {

        public Task<string> GetToken();
    }
}
