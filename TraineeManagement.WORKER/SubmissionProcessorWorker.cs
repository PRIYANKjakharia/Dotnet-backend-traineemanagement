using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TraineeManagement.API.Data;
using TraineeManagement.WORKER.Messaging;
using Microsoft.Extensions.DependencyInjection;
using TraineeManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagement.WORKER;

public class SubmissionProcessorWorker : BackgroundService
{
    private readonly ILogger<SubmissionProcessorWorker> _logger;
    private readonly RabbitMqSettings _rabbitSettings;
    private readonly IServiceProvider _serviceProvider;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection? _connection;
    private IChannel? _channel;

    public SubmissionProcessorWorker(ILogger<SubmissionProcessorWorker> logger, IOptions<RabbitMqSettings> rabbitSettings , IServiceScopeFactory scopeFactory , IServiceProvider serviceProvider)
    {
        _logger = logger;
        _rabbitSettings = rabbitSettings.Value;
        _scopeFactory = scopeFactory;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitSettings.Host,
            Port = _rabbitSettings.Port,
            VirtualHost = _rabbitSettings.VirtualHost,
            UserName = _rabbitSettings.Username,
            Password = _rabbitSettings.Password
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync( queue: "submission-processing", durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync( queue: "submission-deadletter", durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);

        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        
        // ----
        consumer.ReceivedAsync += async (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);

            AppDbContext? context = null;
            ProcessingJob? processingJob = null;
            
            try
            {
                var taskRequest = JsonSerializer.Deserialize<SubmissionProcessingRequested>(messageString);
                using var scope = _serviceProvider.CreateScope();
                context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                processingJob = await context.ProcessingJobs.FirstOrDefaultAsync(x => x.CorrelationId == taskRequest!.CorrelationId);
                
                if (processingJob == null)
                {
                    await _channel.BasicAckAsync( deliveryTag: eventArgs.DeliveryTag, multiple: false ,cancellationToken: stoppingToken);
                    return;
                }
                if (processingJob.Status == "Completed")
                {
                    _logger.LogInformation( "Duplicate message ignored. MessageId: {MessageId}",taskRequest!.MessageId);
        
                    await _channel.BasicAckAsync( deliveryTag: eventArgs.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                
                    return;
                }
                processingJob.UpdatedAt = DateTime.UtcNow;
                processingJob.Attempts++;
                processingJob.Status = "Processing";
                processingJob.StartedAt = DateTime.UtcNow;
                await context.SaveChangesAsync(stoppingToken);


                var submission = await context.Submissions.FindAsync(taskRequest!.SubmissionId);
                Console.WriteLine(submission!.Status);
                submission.Status = "Processing";
                await context.SaveChangesAsync();
                // throw new Exception("trial ex");
                Console.WriteLine(submission.Status);
                if (taskRequest != null)
                {
                    _logger.LogInformation("Processing Message Context. MessageId: {MsgId}, FileId: {FileId}", 
                        taskRequest.MessageId, taskRequest.FileId);

                    await Task.Delay(9000, stoppingToken);
 
                    processingJob.Status = "Completed";
                    processingJob.CompletedAt = DateTime.UtcNow;
                    processingJob.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync(stoppingToken);


                    submission.Status = "Completed";
                    await context.SaveChangesAsync();
                    Console.WriteLine(submission.Status);

                    _logger.LogInformation("Work task completed successfully. Sending Positive Acknowledgment (Ack).");
                    
                    await _channel.BasicAckAsync(deliveryTag: eventArgs.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                }
            }
            catch (Exception ex)
            {
                // try
                // {
                    
                    _logger.LogError(ex, "An error occurred during workflow compilation inside the background agent loop.");
                    if (processingJob != null)
                    {
                        using var scope = _serviceProvider.CreateScope();
                    
                        var catchContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    
                        var failedJob = await catchContext.ProcessingJobs
                            .FirstOrDefaultAsync(x => x.Id == processingJob.Id);
                    
                        if (failedJob != null)
                        {
                            // _logger.LogError("came here");
                    
                            failedJob.Status = "Failed";
                            failedJob.ErrorSummary = ex.Message;
                            failedJob.Attempts++;
                            failedJob.UpdatedAt = DateTime.UtcNow;
                    
                            if (failedJob.Attempts >= 3)
                            {
                                failedJob.Status = "DeadLetter";
                    
                                var bodyBytes = Encoding.UTF8.GetBytes(messageString);
                    
                                await _channel.BasicPublishAsync(
                                    exchange: "",
                                    routingKey: "submission-deadletter",
                                    body: bodyBytes,
                                    cancellationToken: stoppingToken);
                            }
                    
                            await catchContext.SaveChangesAsync(stoppingToken);
                        }
                    }
                    
                    if (processingJob != null && processingJob.Attempts < 3)
                    {
                        await _channel.BasicNackAsync( deliveryTag: eventArgs.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                    }
                    else
                    {
                        await _channel.BasicAckAsync( deliveryTag: eventArgs.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                    }
                // }
                // catch(Exception ex2)
                // {
                //     _logger.LogError(ex2,"2nd exception");
                // }
            }
        };

        await _channel.BasicConsumeAsync(
            queue: "submission-processing",
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("RabbitMQ background consumer started successfully. Listening for tasks...");
        // ----
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync(cancellationToken);
        if (_connection != null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
