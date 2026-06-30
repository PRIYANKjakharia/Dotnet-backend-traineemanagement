using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Data;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TraineeManagement.Api.Messaging;

namespace TraineeManagement.API.Services;

public class SubmissionFileService : ISubmissionFileService
{
    private readonly AppDbContext _context;
    private readonly IFileStorageService _storage;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SubmissionFileService> _logger;
    private readonly RabbitMqSettings _rabbitSettings;
    public SubmissionFileService(AppDbContext context , IFileStorageService storage , IConfiguration configuration , IHttpContextAccessor httpContextAccessor , ILogger<SubmissionFileService> logger, IOptions<RabbitMqSettings> rabbitSettings)
    {
        _context = context;
        _storage = storage;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _rabbitSettings = rabbitSettings.Value;
    }

    public async Task<UploadFileResponse?> UploadAsync(int submissionId, IFormFile file)
    {
        var submission = await _context.Submissions.FirstOrDefaultAsync(sub => sub.Id == submissionId);
        if(submission == null)
        {
            return null;
        }
        if(file.Length == 0)
        {
            throw new ArgumentException("File Is Empty");
        }


        string[] allowedExtensions = _configuration.GetSection("FileStorage:AllowedExtensions").Get<string[]>()!;
        string extension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(extension))
        {
            throw new BadHttpRequestException("Invalid file type");
        }



        long maxSize = _configuration.GetValue<long>("FileStorage:MaxSizeMB") *1024 * 1024;
        if(file.Length > maxSize)
        {
            throw new Exception("File too Large");
        }



        string storedFileName = await _storage.SaveFileAsync(file);



        string checksum = await GenerateChecksum(file);
        var user = _httpContextAccessor.HttpContext!.User;
        var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var Username = user.FindFirst(ClaimTypes.Name)!.Value;
        var submissionFile = new SubmissionFile
        {
            SubmissionId = submissionId,
        
            OriginalFileName = file.FileName,
        
            StoredFileName = storedFileName,
        
            ContentType = file.ContentType,
        
            FileSize = file.Length,
        
            Checksum = checksum,
        
            UploadedByUser = Username,

            UploadedByUserId = userId,
            // UploadedByUser = "Admin",
        
            UploadedDate = DateTime.UtcNow,
        
            UpdatedDate = DateTime.UtcNow
        };

        await _context.SubmissionFiles.AddAsync(submissionFile);
        await _context.SaveChangesAsync();
        var correlationId = Guid.NewGuid();
        var messageContract = new SubmissionProcessingRequested
        {
            CorrelationId = correlationId,
            SubmissionId = submissionId,
            FileId = submissionFile.Id
        };

        var processingJob = new ProcessingJob
        {
            MessageId = messageContract.MessageId,
            CorrelationId = correlationId,
            SubmissionId = submissionId,
            FileId = submissionFile.Id,
            Status = "Queued",
            Attempts = 0
        };
        
        _context.ProcessingJobs.Add(processingJob);
        
        await _context.SaveChangesAsync();

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitSettings.Host,
                Port = _rabbitSettings.Port,
                VirtualHost = _rabbitSettings.VirtualHost,
                UserName = _rabbitSettings.Username,
                Password = _rabbitSettings.Password
            };


            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "submission-processing",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var jsonPayload = JsonSerializer.Serialize(messageContract);
            var body = Encoding.UTF8.GetBytes(jsonPayload);
            var properties = new BasicProperties
            {
                Persistent = true 
            };

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: "submission-processing",
                mandatory: true,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Message published to broker layout stack. MessageId: {MessageId}, CorrelationId: {CorrelationId}",  messageContract.MessageId, correlationId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RabbitMQ connectivity failure structural anomaly detected for submission target {Id}.", submissionId);
            
            _context.SubmissionFiles.Remove(submissionFile);
            await _context.SaveChangesAsync();

            throw new InvalidOperationException("Messaging broker communication pipelines are offline. Async task queue injection aborted.");
        }

        return new UploadFileResponse
        {
            Id = submissionFile.Id,
        
            OriginalFileName = submissionFile.OriginalFileName,
        
            StoredFileName = submissionFile.StoredFileName,
        
            FileSize = submissionFile.FileSize,
        
            ContentType = submissionFile.ContentType,
        
            Checksum = submissionFile.Checksum,
            
            CorrelationId = correlationId
        };
    }
    public async Task<(Stream stream, string contentType, string fileName)?> DownloadAsync(int fileId)
    {
        var file = await _context.SubmissionFiles.FirstOrDefaultAsync(_file => _file.Id == fileId);
    
        if (file == null)
        {
            return null;
        }
    
        bool exists = await _storage.ExistsAsync(file.StoredFileName!);
    
        if (!exists)
        {
            return null;
        }
    
        var stream = await _storage.OpenReadAsync(file.StoredFileName!);
    
        return ( stream , file.ContentType , file.OriginalFileName )!;
    }
    public async Task<bool> DeleteAsync(int fileId)
    {
        var file = await _context.SubmissionFiles.FirstOrDefaultAsync(_file => _file.Id == fileId);
    
        if (file == null)
        {
            return false;
        }
        await _storage.DeleteAsync(file.StoredFileName!);
        _context.SubmissionFiles.Remove(file);
        await _context.SaveChangesAsync();
    
        return true;
    }
    private static async Task<string> GenerateChecksum(IFormFile file)
    {
        using var sha256 = SHA256.Create();
    
        using var stream = file.OpenReadStream();
    
        byte[] hash = await sha256.ComputeHashAsync(stream);
    
        return Convert.ToHexString(hash);
    }
}