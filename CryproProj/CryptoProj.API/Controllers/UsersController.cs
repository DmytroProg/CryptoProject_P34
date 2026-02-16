using CryptoProj.Domain.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace CryptoProj.API.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var user = await _usersService.GetById(id);
        return Ok(user);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        var user = await _usersService.Register(request);
        return Ok(user);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var user = await _usersService.Login(request);
        return Ok(user);
    }
}