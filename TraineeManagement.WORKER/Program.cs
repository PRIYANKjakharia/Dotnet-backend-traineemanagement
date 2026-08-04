using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Data;
using TraineeManagement.API.Interfaces;
using TraineeManagement.API.Services;
using TraineeManagement.WORKER;
using TraineeManagement.WORKER.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqSettings>(
builder.Configuration.GetSection("RabbitMQ"));
 
builder.Services.AddHostedService<SubmissionProcessorWorker>();
 
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMySQL( builder.Configuration.GetConnectionString("DefaultConnection")!);
});

builder.Services.AddStackExchangeRedisCache(options =>
{
   options.Configuration = builder.Configuration["Redis:ConnectionString"]; 
});
builder.Services.AddScoped<IRedisCacheService , RedisCacheService>();


var host = builder.Build();
host.Run();