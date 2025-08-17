
using NewsLetter.Server.Cache;
using NewsLetter.Server.Services;

namespace NewsLetter.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var cacheOptions = new CacheOptions();
            builder.Configuration.GetSection("CacheOptions").Bind(cacheOptions);

            builder.Services.AddSingleton(cacheOptions);

            // HttpClient for HN
            builder.Services.AddHttpClient<IHnClient, HnClient>();

            // Memory cache
            builder.Services.AddMemoryCache();

            // Domain services
            builder.Services.AddScoped<IStoryService, StoryService>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
