using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Repositories;
namespace CRUDExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ICountriesService, CountriesService>();
            builder.Services.AddScoped<IPersonsService, PersonsService>();
            builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //To enable httplogging of require fields
            /*builder.Services.AddHttpLogging(options =>
            {
                options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestBody |
                Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseBody;
            });*/
            WebApplication app = builder.Build();

            if (builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpLogging(); //to enable logging -> don't log request/response body by default due to performance drawback -> we have to enable it if we want body
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            //app.MapGet("/", () => "Hello World!");
            app.Run();
        }
    }
}
