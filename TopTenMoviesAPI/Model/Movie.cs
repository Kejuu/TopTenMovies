namespace TopTenMoviesAPI.Model
{
    public class Movie
    {
        public string? Title { get; set; }
        public int Year { get; set; }
        public double Rating { get; set; }
        public int Metascore { get; set; }
        public string? Director { get; set; }
    }
}
