using TaskManagementApi.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddApplicationServices(builder.Configuration)
            .AddJwtAuth(builder.Configuration)
            .AddCustomOpenApi();

        var app = builder.Build();

        app.UseApplicationMiddleware();

        app.Run();
    }
}