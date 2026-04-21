namespace Blueprint.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOpenApi();

        ConfigureServices(builder.Services);

        WebApplication app = builder.Build();

        Configure(app);

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        await app.RunAsync();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        //// services.AddSwaggerGen();
        services.AddControllers();
    }

    private static void Configure(WebApplication app)
    {
        app.UseAuthorization();
        app.MapControllers();
    }
}
