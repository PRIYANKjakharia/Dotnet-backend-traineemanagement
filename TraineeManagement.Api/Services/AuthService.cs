using TraineeManagement.API.DTOs;
using TraineeManagement.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TraineeManagement.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public AuthService(AppDbContext context , IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(_user => _user.Username == request.Username);
        if(user == null)
        {
            return null!;
        }

        bool isValid = BCrypt.Net.BCrypt.Verify(request.Password,user.PasswordHash);
        if (!isValid)
        {
            return null!;
        }

        var claims = new[]
        {
          new Claim( ClaimTypes.NameIdentifier , user.Id.ToString()),
          new Claim( ClaimTypes.Name, user.Username!),
          new Claim( ClaimTypes.Role, user.Role!)

        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var token = new JwtSecurityToken( issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])),
        signingCredentials: new SigningCredentials(key , SecurityAlgorithms.HmacSha256));

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponse
        {
            Id = user.Id,
            Username = user.Username!,
            Role = user.Role!,
            Token= tokenString,
            ExpiresIn = 3600
        };
    }
}
