
using TraineeManagement.API.Models;
using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp;
using TraineeManagement.API.Interfaces;

namespace TraineeManagement.API.Services;

public class TraineeService : ITraineeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TraineeService> _logger;
    private readonly IRedisCacheService _redis;
 
    public TraineeService( AppDbContext context, ILogger<TraineeService> logger, IRedisCacheService redis)
    {
        _context = context;
        _logger = logger;
        _redis = redis;
    }

    // create
    public async Task<TraineeResponse> Create(CreateTraineeRequest request)
    {   
        var EmailExists = await _context.Trainees.FirstOrDefaultAsync(k => k.Email!.ToLower() == request.Email!.ToLower());
        if(EmailExists != null){
            _logger.LogCritical("Email already exists");
            return null!;
        }
        var trainee = new Trainee
        {
            // Id = nextId++,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            TechStack = request.TechStack,
            Status = request.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow

        };
        // trainees.Add(trainee);
        await _context.Trainees.AddAsync(trainee);
        await _context.SaveChangesAsync();

        _logger.LogInformation("User Created with email "+request.Email);
        return new TraineeResponse
        {
            Id = trainee.Id,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            TechStack = trainee.TechStack,
            Status = trainee.Status
        };
    }






    // delete
    public async Task<bool> Delete(int id)
    {
        var t = await _context.Trainees.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return false;
        }
        _context.Trainees.Remove(t);
        await _context.SaveChangesAsync();
        _logger.LogInformation("User with id "+id+" deleted");

        await _redis.RemoveAsync($"trainee:{id}");
        return true;
    }

    public async Task<TraineeResponse?> GetById(int id)
    {
        string cacheKey = $"trainee:{id}";
        
        var cachedData = await _redis.GetAsync<TraineeResponse>(cacheKey);
        
        if (cachedData != null)
        {
            _logger.LogInformation("Redis Hit $$$$$$$$$$$$$$$$$$$$$$");
            return cachedData;
        }
        
        var t = await _context.Trainees.FindAsync(id);
        if(t == null){
            _logger.LogCritical("Id not found");
            return null;
        }
        _logger.LogInformation("Info Displayed");
        var res = new TraineeResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            TechStack = t.TechStack,
            Status = t.Status
        };

        await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));
        _logger.LogInformation("Redis Miss $$$$$$$$$$$$$$$$$$$$$$$$");
        return res;
    }




// update
    public async Task<string> Update (int id, UpdateTraineeRequest request)
    {
        var t = await _context.Trainees.FindAsync(id);
        var EmailExists = await _context.Trainees.FirstOrDefaultAsync(k => k.Id != id && k.Email!.ToLower() == request.Email!.ToLower());
        if(EmailExists != null){
            _logger.LogCritical("Email already exists");
            return "Email already exists";
        }
        if(t == null){
            _logger.LogCritical("Id not found");
            return "Id Not Found";
        }

        t.FirstName = request.FirstName;
        t.LastName = request.LastName;
        t.Email = request.Email;
        t.TechStack = request.TechStack;
        t.Status = request.Status;
        t.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("User Updated with Id "+id);

        var res = new TraineeResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            TechStack = t.TechStack,
            Status = t.Status
        };
        var cacheKey = $"trainee:{id}";
        await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));

        return "Updated SucessFully";
    }

    public async Task<PagedResponse<TraineeResponse>> GetAllAsync(TraineeQueryParameters query)
    {
        // await _redis.SetAsync("test","hello-----------------------------------------------------=====================", TimeSpan.FromMinutes(5));
        // var value = await _redis.GetAsync<string>("test");
        // _logger.LogInformation(value);

        // string cacheKey = "trainees:all";
        // var cachedData =await _redis.GetAsync<PagedResponse<TraineeResponse>>(cacheKey);
        
        // if (cachedData != null && string.IsNullOrWhiteSpace(query.Search) && string.IsNullOrWhiteSpace(query.Status))
        // {
        //     _logger.LogInformation("GetAll Redis Hit");
        //     return cachedData;
        // }
 
        var trainees = _context.Trainees.AsQueryable();
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            trainees = trainees.Where(t => t.FirstName!.Contains(query.Search) || t.LastName!.Contains(query.Search));
        }
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            trainees = trainees.Where(t => t.Status == query.Status);
        }
        int page = query.PageSize * (query.PageNumber-1);
        List<TraineeResponse> ret =  await trainees.Select( t => new TraineeResponse
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            TechStack = t.TechStack,
            Status = t.Status
        }).Skip(page).Take(query.PageSize).ToListAsync();

        _logger.LogInformation("request for page no. "+query.PageNumber+" with page size "+query.PageSize+" fetched and Displayed");
        var res = new PagedResponse<TraineeResponse>
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalRecords = ret.Count,
            Data = ret
        };

        // await _redis.SetAsync(cacheKey , res , TimeSpan.FromMinutes(5));
        return res;
    }

}
