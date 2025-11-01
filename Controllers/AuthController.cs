using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;
using server.DTOs;
namespace server.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    
    public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService auth)
    {
        this._authService = auth;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(dto);

        if (result == null)
        {
            return BadRequest(new
            {
                message = "username or eamil already exists"
            });
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(dto);

        if (result == null)
        {
            return BadRequest(new
            {
                message = "username or eamil already exists"
            });
        }

        return Ok(result);
    }
}
