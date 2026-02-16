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
}