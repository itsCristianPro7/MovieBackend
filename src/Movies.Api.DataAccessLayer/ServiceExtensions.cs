using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Movies.Api.Application.Services;
using Movies.Api.DataAccessLayer.DataAccess;
using Movies.Api.Domain;
using System.Text;


namespace Movies.Api.DataAccessLayer
{
    public static class ServiceExtensions
    {
        public static void AddIdentityAndPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDBSettingsConfigurationSection = configuration.GetSection(nameof(MongoDBSettings));
            var mongoDBSettings = mongoDBSettingsConfigurationSection.Get<MongoDBSettings>();
            services.Configure<MongoDBSettings>(mongoDBSettingsConfigurationSection);
            services.AddSingleton<IMovieSuggestionRepository, MovieSuggestionRepository>();
            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 7;
            })
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
                    mongoDBSettings.ConnectionString,
                    mongoDBSettings.DatabaseName
                );

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });
        }
    }
}
