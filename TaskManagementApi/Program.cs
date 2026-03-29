using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;
using TaskManagementApi.Data;
using TaskManagementApi.Filters;
using TaskManagementApi.Middleware;
public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // add problem details factory
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); 
        // Add services to the container.
        builder.Services.AddControllers(op =>
        {
            op.Filters.Add<EnvelopResultFilter>();
        });
        // Configure Entity Framework Core with SQL Server
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        // Add authentication services
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        builder.Services.AddOpenApi("v1", options =>
        {

            options.AddDocumentTransformer((document, request, ct) =>
            {
                var version = request.DocumentName;

                document.Info.Version = version;
                document.Info.Title = $"Task managment {version}";

                return Task.CompletedTask;
            });
            options.AddDocumentTransformer((document, request,ct) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                document.Components.SecuritySchemes[JwtBearerDefaults.AuthenticationScheme] =
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Description = "JWT Authorization header using Bearer scheme."
                    };
                return Task.CompletedTask;
            });
            options.AddOperationTransformer((documnet, context, ct) =>
            {

                var metadata = context.Description.ActionDescriptor.EndpointMetadata;

                var hasAuthorize = metadata.OfType<AuthorizeAttribute>().Any();
                var hasAllowAnonymous = metadata.OfType<AllowAnonymousAttribute>().Any();

                if (!hasAuthorize || hasAllowAnonymous)
                {
                    return Task.CompletedTask;
                }

                documnet.Security ??= [];

                documnet.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = []
                });

                return Task.CompletedTask;
            });

        });
        builder.Services.AddEndpointsApiExplorer();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/openapi/v1.json", "v1");
                s.RoutePrefix = string.Empty;
            

            });
            
        }
        app.MapScalarApiReference(options =>
        {
            options
                .WithTitle("Task Management API")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();
        app.UseExceptionHandler();
        app.MapControllers();

     


        app.Run();
    }
}

