using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using netCore_Begginer.Interfaces;
using netCore_Begginer.Models;
using netCore_Begginer.Repository;
using netCore_Begginer.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ProductDbContext>(
    (opt) =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "products",
        Version = "v1",
        Description = "A simple example ASP.NET Core Web API"
    });
    swagger.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "list",
        Version = "v2",
        Description = "A simple v2 version example"
    });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add security requirement
    //Swagger understands that API endpoints require a valid Bearer token in the Authorization
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });

});
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });


builder.Services.AddScoped<IGenerateJwtToken,GenerateJwtToken>();
builder.Services.AddScoped(typeof(ITaskManager<,>), typeof(TaskManager<,>));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Configure Swagger JSON route
    app.UseSwagger(configureSwaggerRoute =>
    {
        configureSwaggerRoute.RouteTemplate = "api/{documentName}/swagger.json";
    });

    // Configure Swagger UI
    app.UseSwaggerUI(endPoint =>
    {
        endPoint.SwaggerEndpoint("/api/v1/swagger.json", "products");
        endPoint.SwaggerEndpoint("api/v2/swagger.json", "list");
        endPoint.RoutePrefix = ""; // Swagger UI will be served at /api
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
// Map Controllers (or endpoints, if you are not using MapControllers)
app.MapControllers();

// Run the application
app.Run();
