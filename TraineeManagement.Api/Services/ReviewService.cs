
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace TraineeManagement.API.Services;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ReviewService> _logger;
    public ReviewService(AppDbContext context , ILogger<ReviewService> logger)
    {
        _context = context;
        _logger = logger;
    }


    // create
    public async Task<ReviewResponse> CreateAsync(CreateReviewRequest request)
    {   
        var submissionExists = await _context.Submissions.FirstOrDefaultAsync(subex => subex.Id == request.SubmissionId);
        var mentorExists = await _context.Mentors.FirstOrDefaultAsync(mnex => mnex.Id == request.MentorId);
        
        if(submissionExists == null || mentorExists == null) return null!;

        var review = new Review
        {
            SubmissionId = request.SubmissionId,
            MentorId = request.MentorId,
            Feedback = request.Feedback,
            Score = request.Score,
            ReviewStatus = request.ReviewStatus,
            ReviewedDate = request.ReviewedDate,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow

        };
        // trainees.Add(learningTask);
        await _context.Reviews.AddAsync(review);
        await _context.SaveChangesAsync();
        _logger.LogInformation("task Assigned done with assignmentId "+ review.SubmissionId );
        return new ReviewResponse
        {
            Id = review.Id,
            SubmissionId = request.SubmissionId,
            MentorId = request.MentorId,
            Feedback = request.Feedback,
            Score = request.Score,
            ReviewStatus = request.ReviewStatus,
            ReviewedDate = request.ReviewedDate,
        };
    }


// get
    public async Task<List<ReviewResponse>> GetAllAsync()
    {
        _logger.LogInformation("Info Displayed");

        

        // .Include(e=>e.Trainee)
        return await _context.Reviews.Include(x => x.TaskAssignment).Select(t => new ReviewResponse
        {
            Id = t.Id,
            SubmissionId = t.SubmissionId,
            MentorId = t.MentorId,
            Feedback = t.Feedback,
            Score = t.Score,
            ReviewStatus = t.ReviewStatus,
            ReviewedDate = t.ReviewedDate,
        }).ToListAsync();
    }

    public async Task<ReviewResponse?> GetByIdAsync(int id)
    {
        var t = await _context.Reviews.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return null;
        }
        _logger.LogInformation("Info Displayed");
        return new ReviewResponse
        {
            Id = t.Id,
            SubmissionId = t.SubmissionId,
            MentorId = t.MentorId,
            Feedback = t.Feedback,
            Score = t.Score,
            ReviewStatus = t.ReviewStatus,
            ReviewedDate = t.ReviewedDate,
        };
    }

}
