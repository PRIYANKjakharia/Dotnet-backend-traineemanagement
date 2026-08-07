using Microsoft.EntityFrameworkCore;
using TraineeManagement.API.Data;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Models;

namespace TraineeManagement.API.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(
        AppDbContext context,
        ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserResponse?> CreateAsync(CreateUserRequest request)
    {
        // Check username
        var usernameExists = await _context.Users
            .AnyAsync(u => u.Username!.ToLower() == request.Username.ToLower());

        if (usernameExists)
        {
            _logger.LogWarning("Username already exists");
            return null;
        }

        // Check email in Users
        var userEmailExists = await _context.Users
            .AnyAsync(u => u.Email!.ToLower() == request.Email.ToLower());

        if (userEmailExists)
        {
            _logger.LogWarning("User email already exists");
            return null;
        }

        // Check email in profiles too
        var traineeEmailExists = await _context.Trainees
            .AnyAsync(t => t.Email!.ToLower() == request.Email.ToLower());

        var mentorEmailExists = await _context.Mentors
            .AnyAsync(m => m.Email!.ToLower() == request.Email.ToLower());

        if (traineeEmailExists || mentorEmailExists)
        {
            _logger.LogWarning("Profile email already exists");
            return null;
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Create User
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // 2. Create profile according to role
            int profileId;

            if (request.Role.ToLower() == "mentor")
            {
                var mentor = new Mentor
                {
                    UserId = user.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Expertise = request.Expertise,
                    Status = request.Status,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                await _context.Mentors.AddAsync(mentor);
                await _context.SaveChangesAsync();

                profileId = mentor.Id;
            }
            else
            {
                var trainee = new Trainee
                {
                    UserId = user.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    TechStack = request.TechStack,
                    Status = request.Status,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                await _context.Trainees.AddAsync(trainee);
                await _context.SaveChangesAsync();

                profileId = trainee.Id;
            }

            // 3. Everything succeeded
            await transaction.CommitAsync();

            _logger.LogInformation(
                "User created with username {Username} and role {Role}",
                user.Username,
                user.Role);

            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username!,
                Email = user.Email!,
                Role = user.Role!,
                ProfileId = profileId
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}