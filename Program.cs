using MongoDataAccess.DataAccess;
using MongoDataAccess.Models;

namespace MovieBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));
            builder.Services.AddSingleton<MovieSuggestionService>();

            //var db = new MovieSuggestionDataAccess();
            //var actors = new List<string> { "Millie Bobby Brown" };
            //await db.CreateMovie(new Movie() { Title = "Damsel",
            //                             Director = "Juan Carlos Fresnadillo",
            //                             ReleaseYear = 2024,
            //                             Genre = "Fantasy/Adventure",
            //                             Description = "A young woman agrees to marry a handsome prince -- only to discover it was all a trap. She is thrown into a cave with a fire-breathing dragon and must rely solely on her wits and will to survive.",
            //                             Duration = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(50)),
            //});
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}