using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TraineeManagement.API.DTOs;
namespace TraineeManagement.API.Services;

public class TrainingDirectoryClient : ITrainingDirectoryClient
{
    // 🟢 TASK 3.18: Explicit utilization of the unmanaged socket factory manager pooling abstraction
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TrainingDirectoryClient> _logger;

    public TrainingDirectoryClient( IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger<TrainingDirectoryClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<DirectoryProfileDto?> GetProfileAsync(int traineeId, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient("TrainingDirectoryService");


        var httpContext = _httpContextAccessor.HttpContext;
        string correlationId = httpContext?.Request.Headers["X-Correlation-ID"].ToString() ?? Guid.NewGuid().ToString();


        var request = new HttpRequestMessage(HttpMethod.Get, $"api/internal/trainees/{traineeId}");
        request.Headers.Add("X-Correlation-ID", correlationId); // Propagated securely

        _logger.LogInformation("Dispatching inter-service sync factory network call. CorrelationID: {Header}", correlationId);

        using var response = await httpClient.SendAsync(request, cancellationToken);

        // Explicitly handle non-success responses
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Remote internal directory API returned a non-success status code: {Status}", response.StatusCode);
            
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new ArgumentException("Validation failure ");
            }
            
            response.EnsureSuccessStatusCode(); 
        }

        return await response.Content.ReadFromJsonAsync<DirectoryProfileDto>(cancellationToken: cancellationToken);
    }
}