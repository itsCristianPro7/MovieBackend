using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Movies.Api.Application.Services;
using Movies.Api.Domain;

namespace Movies.Api.DataAccessLayer.DataAccess
{
    public class MovieSuggestionRepository : IMovieSuggestionRepository
    {
        private readonly IMongoCollection<Movie> _movieCollection;

        public MovieSuggestionRepository(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var db = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _movieCollection = db.GetCollection<Movie>(mongoDBSettings.Value.MovieCollection);
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            var results = await _movieCollection.FindAsync(_ => true);
            return results.ToList();
        }

        public async Task<Movie> GetMovieAsync(string id)
        {
            var result = await _movieCollection.Find(m => m.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task CreateMovie(Movie movie)
        {
            await _movieCollection.InsertOneAsync(movie);
            return;
        }

        public async Task UpdateMovie(string id, Movie movie)
        {
            var filter = Builders<Movie>.Filter.Eq("Id", id);
            await _movieCollection.ReplaceOneAsync(filter, movie, new ReplaceOptions { IsUpsert = true });
            return;
        }

        public async Task DeleteMovie(string id)
        {
            await _movieCollection.DeleteOneAsync(m => m.Id == id);
            return;
        }
    }
}
