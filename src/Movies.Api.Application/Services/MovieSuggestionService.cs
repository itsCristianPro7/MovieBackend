using MongoDB.Driver;
using Movies.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Application.Services
{
    public class MovieSuggestionService : IMovieSuggestionService
    {
        private readonly IMovieSuggestionRepository _movieSuggestionRepository;

        public MovieSuggestionService(IMovieSuggestionRepository movieSuggestionRepository)
        {
            _movieSuggestionRepository = movieSuggestionRepository;
        }

        public async Task CreateMovie(Movie movie)
        {
            await _movieSuggestionRepository.CreateMovie(movie);
            return;
        }

        public async Task DeleteMovie(string id)
        {
            await _movieSuggestionRepository.DeleteMovie(id);
            return;
        }

        public async Task<Movie> GetMovieAsync(string id)
        {
            var result = await _movieSuggestionRepository.GetMovieAsync(id);
            return result;
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            var results = await _movieSuggestionRepository.GetMoviesAsync();
            return results;
        }

        public async Task UpdateMovie(string id, Movie movie)
        {
            var filter = Builders<Movie>.Filter.Eq("Id", id);
            await _movieSuggestionRepository.UpdateMovie(id, movie);
            return;
        }
    }
}
