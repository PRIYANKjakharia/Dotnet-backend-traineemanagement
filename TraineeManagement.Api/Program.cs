using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Data;
using TraineeManagement.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TraineeManagement.Api.Middleware;
using TraineeManagement.Api.Messaging;
using System.Globalization;
using TraineeManagement.API.Interfaces;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TraineeManagement.API.Extensions;
using Polly;
using Polly.CircuitBreaker;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddHttpClient("TrainingDirectoryService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5050/");
    client.Timeout = TimeSpan.FromSeconds(5);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddResilienceHandler("circuit-breaker", builder =>
{
    builder.AddCircuitBreaker(new HttpCircuitBreakerOptions
    {
        // The failure ratio required to open the circuit (e.g., 50%)
        FailureRatio = 0.5,

        // Minimum number of requests to process before enforcing the breaker
        SamplingDuration = TimeSpan.FromSeconds(10),

        // Duration the circuit remains open before entering Half-Open
        BreakDuration = TimeSpan.FromSeconds(30),

        MinimumThroughput = 10
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi();
builder.Services.AddScoped<ITraineeService , TraineeService>();
builder.Services.AddScoped<IMentorService , MentorService>();
builder.Services.AddScoped<ILearningTaskService , LearningTaskService>();
builder.Services.AddScoped<ITaskAssignmentService , TaskAssignmentService>();
builder.Services.AddScoped<IAuthService , AuthService>();
builder.Services.AddScoped<ISubmissionService , SubmissionService>();
builder.Services.AddScoped<IReviewService , ReviewService>();
builder.Services.AddScoped<IFileStorageService , LocalFileStorageService>();
builder.Services.AddScoped<ISubmissionFileService , SubmissionFileService>();
builder.Services.AddScoped<IProcessingJobService , ProcessingJobService>();
builder.Services.AddScoped<ITrainingDirectoryClient , TrainingDirectoryClient>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>(tags: new[] { "ready" })
    .AddRedis(
        builder.Configuration["Redis:ConnectionString"]!,
        name: "Redis",
        tags: new[] { "ready" }
    ).AddRabbitMQ(
        async sp => await sp.GetRequiredService<RabbitMQ.Client.ConnectionFactory>().CreateConnectionAsync(),
        name: "RabbitMQ",
        tags: new[] { "ready" }
    );
 


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySQL(connectionString!);
});

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme ).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
});
 
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

builder.Services.AddAuthorization();
// Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("Admin@123"));
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton(sp =>
{
    var rabbitMqSection = builder.Configuration.GetSection("RabbitMQ");
 
    return new RabbitMQ.Client.ConnectionFactory
    {
        HostName = rabbitMqSection["Host"] ?? "localhost",
        Port = int.Parse(rabbitMqSection["Port"] ?? "5672"),
        UserName = rabbitMqSection["UserName"] ?? "guest",
        Password = rabbitMqSection["Password"] ?? "guest",
        VirtualHost = rabbitMqSection["VirtualHost"] ?? "/"
    };
});
 

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,policy  => {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false,
    ResponseWriter = HealthCheckReportExtension.WriteHealthCheckResponse
});
 
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = HealthCheckReportExtension.WriteHealthCheckResponse
});
 
 
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors(MyAllowSpecificOrigins);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();

    // Try up to 5 times with a delay to allow MySQL time to boot up completely
    for (int i = 0; i < 5; i++)
    {
        try
        {
            logger.LogInformation("Attempting to apply database migrations (Attempt {Attempt}/5)...", i + 1);
            context.Database.Migrate();
            logger.LogInformation("Database migrations applied successfully!");
            break; // Exit loop if migration succeeds
        }
        catch (Exception ex)
        {
            logger.LogWarning("Database not ready yet. Retrying in 5 seconds...");
            if (i == 4) // If it fails on the final attempt, log the hard error
            {
                logger.LogError(ex, "An error occurred while migrating the database after multiple attempts.");
            }
            System.Threading.Thread.Sleep(5000); // Wait 5 seconds before trying again
        }
    }
}
 

app.Run();

internal class HttpCircuitBreakerOptions : CircuitBreakerStrategyOptions<HttpResponseMessage>
{
    public double FailureRatio { get; set; }
    public TimeSpan SamplingDuration { get; set; }
    public TimeSpan BreakDuration { get; set; }
    public int MinimumThroughput { get; set; }
}