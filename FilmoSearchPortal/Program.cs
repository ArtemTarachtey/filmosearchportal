using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Context;
using DataAccess.Extensions;
using FilmoSearchPortal.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FilmoSearchPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddDatabaseContext(builder.Configuration); // регистрирую контекст в экстеншн классе
            builder.Services.AddControllers()
                .AddJsonOptions(option =>
                {
                    option.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<IFilmService, FilmService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IActorService, ActorService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") 
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
                options.Audience = builder.Configuration["Auth0:Audience"];
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                }
            }

            app.UseCors();


            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
            });

            app.UseCustomExceptionHandling(); // использую кастомный мв для исключений
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}