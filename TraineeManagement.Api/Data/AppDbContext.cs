using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Models;
namespace TraineeManagement.API.Data;
public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }
    public DbSet<Trainee> Trainees {get ; set;}
    public DbSet<User> Users {get ; set;}
    public DbSet<Mentor> Mentors {get ; set;}
    public DbSet<LearningTask> LearningTasks {get ; set;}
    public DbSet<TaskAssignment> TaskAssignments {get ; set;}
    public DbSet<Submission> Submissions {get ; set;}
    public DbSet<Review> Reviews {get ; set;}
    public DbSet<SubmissionFile> SubmissionFiles {get ; set;}
    public DbSet<ProcessingJob> ProcessingJobs {get ; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var adminUsername = _configuration["AdminSettings:Username"];
        var adminEmail = _configuration["AdminSettings:Email"];
        var adminPassword = _configuration["AdminSettings:Password"];
  
        var mentorUsername = _configuration["MentorSettings:Username"];
        var mentorEmail = _configuration["MentorSettings:Email"];
        var mentorPassword = _configuration["MentorSettings:Password"];

        var traineeUsername = _configuration["TraineeSettings:Username"];
        var traineeEmail = _configuration["TraineeSettings:Email"];
        var traineePassword = _configuration["TraineeSettings:Password"];

        modelBuilder.Entity<User>().HasData(
            new User{
                Id = 1,
                Username = adminUsername,
                Email = adminEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                Role = "admin"
    
            },
            new User{
                Id = 2,
                Username = mentorUsername,
                Email = mentorEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(mentorPassword),
                Role = "mentor"
            },
            new User{
                Id = 3,
                Username = traineeUsername,
                Email = traineeEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(traineePassword),
                Role = "trainee"
            }
        );

        modelBuilder.Entity<TaskAssignment>().HasOne(t=> t.Trainee).WithMany().HasForeignKey(t=> t.TraineeId);
        modelBuilder.Entity<TaskAssignment>().HasOne(t=> t.Mentor).WithMany().HasForeignKey(t=> t.MentorId);
        modelBuilder.Entity<TaskAssignment>().HasOne(t=> t.LearningTask).WithMany().HasForeignKey(t=> t.LearningTaskId);

        modelBuilder.Entity<Submission>().HasOne(t=> t.TaskAssignment).WithMany().HasForeignKey(t=> t.TaskAssignmentId);

        modelBuilder.Entity<Review>().HasOne(s=> s.Submission).WithMany().HasForeignKey(s=> s.SubmissionId);
        modelBuilder.Entity<Review>().HasOne(m=> m.Mentor).WithMany().HasForeignKey(m=> m.MentorId);
 
        modelBuilder.Entity<SubmissionFile>().HasOne(f=> f.Submission).WithMany(f=>f.Files).HasForeignKey(f=> f.SubmissionId);
    }
}