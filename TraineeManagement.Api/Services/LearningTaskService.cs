
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;

namespace TraineeManagement.API.Services;

public class LearningTaskService : ILearningTaskService
{
    private readonly AppDbContext _context;
    private readonly ILogger<MentorService> _logger;
    public LearningTaskService(AppDbContext context , ILogger<MentorService> logger)
    {
        _context = context;
        _logger = logger;
    }


    // create
    public async Task<LearningTaskResponse> CreateAsync(CreateLearningTaskRequest request)
    {   
        var learningTask = new LearningTask
        {
            // Id = nextId++,
            Title = request.Title,
            Description = request.Description,
            DueDate = request.DueDate,
            ExpectedTechStack = request.ExpectedTechStack,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow

        };
        await _context.LearningTasks.AddAsync(learningTask);
        await _context.SaveChangesAsync();
        _logger.LogInformation("learning task Created with DueDate "+request.DueDate);
        return new LearningTaskResponse
        {
            Id = learningTask.Id,
            Title = learningTask.Title,
            Description = learningTask.Description,
            DueDate = learningTask.DueDate,
            ExpectedTechStack = learningTask.ExpectedTechStack,
            Status = learningTask.Status
        };
    }






    // delete
    public async Task<bool> DeleteAsync(int id)
    {
        var t = await _context.LearningTasks.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return false;
        }
        _context.LearningTasks.Remove(t);
        await _context.SaveChangesAsync();
        _logger.LogInformation("LearningTask with id "+id+" deleted");
        return true;
    }





// get
    public async Task<List<LearningTaskResponse>> GetAllAsync()
    {
        _logger.LogInformation("Info Displayed");
        return await _context.LearningTasks.Select(t => new LearningTaskResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            ExpectedTechStack = t.ExpectedTechStack,
            Status = t.Status
        }).ToListAsync();
    }

    public async Task<LearningTaskResponse?> GetByIdAsync(int id)
    {
        var t = await _context.LearningTasks.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return null;
        }
        _logger.LogInformation("Info Displayed");
        return new LearningTaskResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            DueDate = t.DueDate,
            ExpectedTechStack = t.ExpectedTechStack,
            Status = t.Status
        };
    }




// update
    public async Task<string> UpdateAsync (int id, UpdateLearningTaskRequest request)
    {
        var t = await _context.LearningTasks.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return "Id Not Found";
        }

        t.Title = request.Title;
        t.Description = request.Description;
        t.DueDate = request.DueDate;
        t.ExpectedTechStack = request.ExpectedTechStack;
        t.Status = request.Status;
        t.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("learning task Updated with Id "+id);
        return "Updated SucessFully";
    }
}
