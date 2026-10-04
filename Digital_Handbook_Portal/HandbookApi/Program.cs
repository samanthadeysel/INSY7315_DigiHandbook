using Digital_Handbook_Portal.Models;
using HandbookApi.Services;
using Microsoft.EntityFrameworkCore;

namespace HandbookApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("Digital_Handbook_PortalContext")
                ?? throw new InvalidOperationException("Connection string 'Digital_Handbook_PortalContext' not found.");

            builder.Services.AddDbContext<Digital_Handbook_PortalContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            builder.Services.AddEndpointsApiExplorer();

            // Register Google Cloud Storage Service
            builder.Services.AddScoped<ICloudStorageService, CloudStorageService>();

            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Handbook API v1");
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}