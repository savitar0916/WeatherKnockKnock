namespace WeatherKnockKnock
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            //var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
            //var url = $"http://127.0.0.1:{port}";
            //builder.WebHost.UseUrls(url);

            var app = builder.Build();


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}