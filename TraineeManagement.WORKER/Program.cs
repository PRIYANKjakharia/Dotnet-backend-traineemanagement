using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Data;
using TraineeManagement.WORKER;
using TraineeManagement.WORKER.Messaging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMQ"));
 
builder.Services.AddHostedService<SubmissionProcessorWorker>();
 
builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMySQL( builder.Configuration.GetConnectionString("DefaultConnection")!);
});

var host = builder.Build();
host.Run();