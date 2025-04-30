using Forum.API.BackgroundServices;
using Forum.API.Converters;
using Forum.API.Filters;
using Forum.API.Middleware;
using Forum.API.RateLimit;
using Forum.API.Token;
using Forum.Application;
using Forum.Domain.Security.AccessToken;
using Forum.Infrastructure;
using Forum.Infrastructure.DataAccess.Migrations;
using Forum.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;

const string AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options => options.AddPolicy<string, RateLimiterPolicy>("RateLimiterPolicy"));

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new StringConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ForumAPI",
        Version = "1.0",
    });

    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = AUTHENTICATION_TYPE,
    });


    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = AUTHENTICATION_TYPE
                },
                Scheme = AUTHENTICATION_TYPE,
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilters)));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddRouting(options => options.LowercaseUrls =  true);

builder.Services.AddHttpContextAccessor();

if (!builder.Configuration.IsUnitTestEnviroment())
{
    var connection = builder.Configuration.GetValue<string>("Settings:RabbitMQ:Connection")!;
    var queueName = builder.Configuration.GetValue<string>("Settings:RabbitMQ:QueueName")!;

    builder.Services.AddHostedService(provider =>
    {
        var logger = provider.GetRequiredService<ILogger<DeleteUserService>>();
        return new DeleteUserService(connection, queueName, provider, logger);
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    if (builder.Configuration.IsUnitTestEnviroment())
    {
        return;
    }

    var databaseType = builder.Configuration.DatabaseType();

    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(databaseType, connectionString, serviceScope.ServiceProvider);
}

public partial class Program
{
    protected Program() { }
}
