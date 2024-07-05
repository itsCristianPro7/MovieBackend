
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Movies.Api.Domain
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
    }
}
