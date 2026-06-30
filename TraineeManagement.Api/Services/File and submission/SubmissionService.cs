
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using TraineeManagement.API.Interfaces;

namespace TraineeManagement.API.Services;

public class SubmissionService : ISubmissionService
{
    private readonly AppDbContext _context;
    private readonly ILogger<SubmissionService> _logger;
    private readonly IRedisCacheService _redis;

    public SubmissionService(AppDbContext context , ILogger<SubmissionService> logger, IRedisCacheService redis)
    {
        _context = context;
        _logger = logger;
        _redis = redis;
    }


    // create
    public async Task<SubmissionResponse> CreateAsync(CreateSubmissionRequest request)
    {   
        var AssignmentExists = await _context.TaskAssignments.FirstOrDefaultAsync(ta => ta.Id == request.TaskAssignmentId);
        
        if(AssignmentExists == null) return null!;

        var submission = new Submission
        {
            TaskAssignmentId = request.TaskAssignmentId,
            SubmissionUrl = request.SubmissionUrl,
            Notes = request.Notes,
            Status = request.Status,
            SubmissionDate = request.SubmissionDate,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,

        };
        // trainees.Add(learningTask);
        await _context.Submissions.AddAsync(submission);
        await _context.SaveChangesAsync();

        _logger.LogInformation("task Assigned done with assignmentId "+ submission.TaskAssignmentId );
        var res = new SubmissionResponse
        {
            Id= submission.Id,
            TaskAssignmentId = submission.TaskAssignmentId,
            SubmissionUrl = submission.SubmissionUrl,
            Notes = submission.Notes,
            Status = submission.Status,
            SubmissionDate = submission.SubmissionDate,
            TaskTitle = AssignmentExists.LearningTaskTitle,
            TraineeName = AssignmentExists.TraineeName
        };

        string cacheKey = $"submission:{submission.Id}";
        await _redis.RemoveAsync("submission:all");
        await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));
        return res;
    }


// get
    public async Task<List<SubmissionResponse>> GetAllAsync()
    {
        _logger.LogInformation("Info Displayed");

        string cacheKey = "submission:all";
        var cachedData = await _redis.GetAsync<List<SubmissionResponse>>(cacheKey);
        
        if (cachedData != null)
        {
            _logger.LogInformation("GetAll Redis Hit $$$$$$$$$$$$$");
            return cachedData;
        }

        // .Include(e=>e.Trainee)
        var res = await _context.Submissions.Include(x => x.TaskAssignment).Select(t => new SubmissionResponse
        {
            Id= t.Id,
            TaskAssignmentId = t.TaskAssignmentId,
            SubmissionUrl = t.SubmissionUrl,
            Notes = t.Notes,
            Status = t.Status,
            SubmissionDate = t.SubmissionDate,
            // Trainee = t.Trainee,
        }).ToListAsync();
        _logger.LogInformation("GetAll Redis Miss $$$$$$$$$$$$$$$$");
        await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));
        return res;
    }

    public async Task<SubmissionResponse?> GetByIdAsync(int id)
    {

        string cacheKey = $"submission:{id}";
        
        var cachedData = await _redis.GetAsync<SubmissionResponse>(cacheKey);
        
        if (cachedData != null)
        {
            _logger.LogInformation("Redis Hit $$$$$$$$$$$$$$$$$$$$$$");
            return cachedData;
        }
        
        var t = await _context.Submissions.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return null;
        }
        _logger.LogInformation("Info Displayed");
        var res = new SubmissionResponse
        {
            Id= t.Id,
            TaskAssignmentId = t.TaskAssignmentId,
            SubmissionUrl = t.SubmissionUrl,
            Notes = t.Notes,
            Status = t.Status,
            SubmissionDate = t.SubmissionDate,
        };
        await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));
        _logger.LogInformation("Redis Miss $$$$$$$$$$$$$$$$$$$$$$$$");
        return res;
    }

}
