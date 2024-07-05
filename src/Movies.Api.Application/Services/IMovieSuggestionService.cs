using Movies.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Application.Services
{
    public interface IMovieSuggestionService
    {
        Task CreateMovie(Movie movie);
        Task DeleteMovie(string id);
        Task<Movie> GetMovieAsync(string id);
        Task<List<Movie>> GetMoviesAsync();
        Task UpdateMovie(string id, Movie movie);
    }
}
