using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Models;
namespace TraineeManagement.API.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
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
 
        modelBuilder.Entity<User>().HasData(new User
        {
            Id=1,
            Username="admin",
            Email="admin@gmail.com",
            PasswordHash=BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role="admin"
   
        });

        modelBuilder.Entity<TaskAssignment>().HasOne(t=> t.Trainee).WithMany().HasForeignKey(t=> t.TraineeId);
        modelBuilder.Entity<TaskAssignment>().HasOne(t=> t.Mentor).WithMany().HasForeignKey(t=> t.MentorId);
        modelBuilder.Entity<TaskAssignment>().HasOne(t=> t.LearningTask).WithMany().HasForeignKey(t=> t.LearningTaskId);

        modelBuilder.Entity<Submission>().HasOne(t=> t.TaskAssignment).WithMany().HasForeignKey(t=> t.TaskAssignmentId);

        modelBuilder.Entity<Review>().HasOne(s=> s.Submission).WithMany().HasForeignKey(s=> s.SubmissionId);
        modelBuilder.Entity<Review>().HasOne(m=> m.Mentor).WithMany().HasForeignKey(m=> m.MentorId);
 
        modelBuilder.Entity<SubmissionFile>().HasOne(f=> f.Submission).WithMany(f=>f.Files).HasForeignKey(f=> f.SubmissionId);
    }
}