using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MongoDB.Driver;
using server.Data;
using server.DTOs;
using server.Models;
using server.IAuthService;
using Microsoft.EntityFrameworkCore;

namespace server.Services;

public class AuthService : ItokenService
{
    private readonly IConfiguration _configuraiotn;
    private readonly ApplicationDbContext _context;
    
    public AuthService(IConfiguration configuration, ApplicationDbContext context)
    {
        this._configuraiotn = configuration;
        this._context = context;
    }
    public async Task<AuthresponsetDto> RegisterAsync(RegisterDto registerDto)
    {
        bool emailExists = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
        bool usernameExists = await _context.Users.AnyAsync(u => u.Username == registerDto.Username);

        if (emailExists || usernameExists)
        {
            return null;
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        var user = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = CreateToken(user);

        return new AuthresponsetDto
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
        };      
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
this._configuraiotn["JwtSettings:Key"]!
        ));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            issuer: _configuraiotn["JwtSettings:Issuer"],
            audience: _configuraiotn["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    

    public async Task<AuthresponsetDto> LoginAsync(LoginDto loginDto)
    {

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user == null)
        {
            return null;
        }

        bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        if (!isPasswordCorrect)
        {
            return null;
        }

        var token = CreateToken(user);

        return new AuthresponsetDto
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        // Your implementation
        return false;
    }

}