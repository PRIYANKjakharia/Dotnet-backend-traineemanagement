
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using TraineeManagement.API.Interfaces;

namespace TraineeManagement.API.Services;

public class TaskAssignmentService : ITaskAssignmentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TaskAssignmentService> _logger;
    private readonly IRedisCacheService _redis;
 
    public TaskAssignmentService(AppDbContext context , ILogger<TaskAssignmentService> logger , IRedisCacheService redis)
    {
        _context = context;
        _logger = logger;
        _redis = redis;
    }


    // create
    public async Task<TaskAssignmentResponse?> CreateAsync(CreateTaskAssignmentRequest request)
    {   
        var traineeExists = await _context.Trainees.FirstOrDefaultAsync(x => x.Id == request.TraineeId);
        var mentorExists = await _context.Mentors.FirstOrDefaultAsync(x => x.Id == request.MentorId);
        var learningTaskExists = await _context.LearningTasks.FirstOrDefaultAsync(x => x.Id == request.LearningTaskId);

        if(traineeExists == null || mentorExists == null || learningTaskExists == null || DateTime.UtcNow >= learningTaskExists.DueDate ) return null;

        var TaskAssignment = new TaskAssignment
        {
            TraineeId = request.TraineeId,
            TraineeName = traineeExists.FirstName +" "+ traineeExists.LastName,
            MentorId = request.MentorId,
            MentorName = mentorExists.FirstName +" "+ mentorExists.LastName,
            LearningTaskId = request.LearningTaskId,
            LearningTaskTitle = learningTaskExists.Title,
            DueDate = learningTaskExists.DueDate,
            AssignedDate = DateTime.UtcNow,
            Status = request.Status,
            Remarks = request.Remarks,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow

        };
        // trainees.Add(learningTask);
        await _context.TaskAssignments.AddAsync(TaskAssignment);
        await _context.SaveChangesAsync();
        _logger.LogInformation("task Assignment done with AssignedDate "+ TaskAssignment.AssignedDate+" and with DueDate "+TaskAssignment.DueDate);
        await _redis.RemoveAsync("taskassignment:all");
        return new TaskAssignmentResponse
        {
            Id = TaskAssignment.Id,
            TraineeId = request.TraineeId,
            TraineeName = traineeExists.FirstName +" "+ traineeExists.LastName,
            MentorId = request.MentorId,
            MentorName = mentorExists.FirstName +" "+ mentorExists.LastName,
            LearningTaskId = request.LearningTaskId,
            LearningTaskTitle = learningTaskExists.Title,
            DueDate = learningTaskExists.DueDate,
            AssignedDate = DateTime.UtcNow,
            Status = request.Status,
            Remarks = request.Remarks,
        };
    }






    // delete
    public async Task<bool> DeleteAsync(int id)
    {
        var t = await _context.TaskAssignments.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return false;
        }
        _context.TaskAssignments.Remove(t);
        await _context.SaveChangesAsync();
        _logger.LogInformation("TaskAssignment with id "+id+" deleted");
        await _redis.RemoveAsync($"taskassignment:{id}");
        await _redis.RemoveAsync("taskassignment:all");
        return true;
    }





// get
    public async Task<List<TaskAssignmentResponse>> GetAllAsync()
    {
        _logger.LogInformation("Info Displayed");

        string cacheKey = "taskassignment:all";
        var cachedData =await _redis.GetAsync<List<TaskAssignmentResponse>>(cacheKey);
        if (cachedData != null)
        {
            _logger.LogInformation("GetAll Redis Hit $$$$$$$$$$$$$$$$");
            return cachedData;
        }

        // .Include(e=>e.Trainee).Include(e=>e.Mentor).Include(e=>e.LearningTask)
        var res = await _context.TaskAssignments.Select(t => new TaskAssignmentResponse
        {
            Id = t.Id,
            TraineeId = t.TraineeId,
            TraineeName = t.TraineeName,
            MentorId = t.MentorId,
            MentorName = t.MentorName,
            LearningTaskId = t.LearningTaskId,
            LearningTaskTitle = t.LearningTaskTitle,
            DueDate = t.DueDate,
            AssignedDate = t.AssignedDate,
            Status = t.Status,
            Remarks = t.Remarks,
            // Trainee = t.Trainee,
        }).ToListAsync();
        _logger.LogInformation("GetAll Redis Miss $$$$$$$$$$$$$$$$");
        await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));
        return res;
    }

    public async Task<TaskAssignmentResponse?> GetByIdAsync(int id)
    {
        string cacheKey = $"taskassignment:{id}";
        
        var cachedData = await _redis.GetAsync<TaskAssignmentResponse>(cacheKey);
        
        if (cachedData != null)
        {
            _logger.LogInformation("Redis Hit $$$$$$$$$$$$$$$$$$$$$$");
            return cachedData;
        }

        var t = await _context.TaskAssignments.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return null;
        }
        _logger.LogInformation("Info Displayed");
        var res = new TaskAssignmentResponse
        {
            Id = t.Id,
            TraineeId = t.TraineeId,
            TraineeName = t.TraineeName,
            MentorId = t.MentorId,
            MentorName = t.MentorName,
            LearningTaskId = t.LearningTaskId,
            LearningTaskTitle = t.LearningTaskTitle,
            DueDate = t.DueDate,
            AssignedDate = t.AssignedDate,
            Status = t.Status,
            // Trainee = t.Trainee,
            Remarks = t.Remarks,
        };
        await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));
        _logger.LogInformation("Redis Miss $$$$$$$$$$$$$$$$$$$$$$$$");
        return res;
    }




// update
    public async Task<string> UpdateAsync (int id, UpdateTaskAssignmentRequest request)
    {
        var t = await _context.TaskAssignments.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return "Task Id Not Found";
        }

        t.UpdatedDate = DateTime.UtcNow;
        t.Status = request.Status;
        // if(request.Remarks != "")t.Remarks = request.Remarks;
        await _context.SaveChangesAsync();
        _logger.LogInformation("learning task Updated with Id "+id);
        var cacheKey = $"taskassignment:{id}";
        var res = new TaskAssignmentResponse
        {
            Id = t.Id,
            TraineeId = t.TraineeId,
            TraineeName = t.TraineeName,
            MentorId = t.MentorId,
            MentorName = t.MentorName,
            LearningTaskId = t.LearningTaskId,
            LearningTaskTitle = t.LearningTaskTitle,
            DueDate = t.DueDate,
            AssignedDate = t.AssignedDate,
            Status = t.Status,
            // Trainee = t.Trainee,
            Remarks = t.Remarks,
        };
        await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));
        await _redis.RemoveAsync("taskassignment:all");
        return "Updated SucessFully";
    }

}
