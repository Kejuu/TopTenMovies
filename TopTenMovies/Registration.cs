using TopTenMovies.Services;

namespace TopTenMovies
{
    public static class Registration
    {
        public static IServiceCollection Register(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IAuthService, AuthService>();
            return services;
        }

    }
}

