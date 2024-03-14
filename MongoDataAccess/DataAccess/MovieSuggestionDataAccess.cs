using MongoDataAccess.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace MongoDataAccess.DataAccess
{
    public class MovieSuggestionDataAccess
    {
        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "moviesuggestiondb";
        private const string MovieCollection = "movies";

        private IMongoCollection<T> ConnectToMongo<T>(in string collection)
        {
            var client = new MongoClient(ConnectionString);
            var db = client.GetDatabase(DatabaseName);
            return db.GetCollection<T>(collection);
        }

        private async Task<List<Movie>> GetMoviesAsync()
        {
            var moviesCollection = ConnectToMongo<Movie>(MovieCollection);
            var results = await moviesCollection.FindAsync(_ => true);
            return results.ToList();
        }

        private Task CreateMovie(Movie movie)
        {
            var moviesCollection = ConnectToMongo<Movie>(MovieCollection);
            return moviesCollection.InsertOneAsync(movie);
            //adfa
        }

        private Task UpdateMovie(Movie movie)
        {
            var moviesCollection = ConnectToMongo<Movie>(MovieCollection);
            var filter = Builders<Movie>.Filter.Eq("Id", movie.Id);
            return moviesCollection.ReplaceOneAsync(filter, movie, new ReplaceOptions { IsUpsert = true});
        }

        private Task DeleteMovie(Movie movie)
        {
            var moviesCollection = ConnectToMongo<Movie>(MovieCollection);
            return moviesCollection.DeleteOneAsync(m  => m.Id == movie.Id);
        }
    }
}
