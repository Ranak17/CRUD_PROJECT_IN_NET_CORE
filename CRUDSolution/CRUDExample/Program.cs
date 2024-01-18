namespace CRUDExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            WebApplication app = builder.Build();

            if(builder.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllers();

            //app.MapGet("/", () => "Hello World!");
            app.Run();
        }
    }
}
