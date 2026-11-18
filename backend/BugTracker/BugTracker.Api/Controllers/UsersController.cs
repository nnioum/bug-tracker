using BugTracker.Api.Data;
using BugTracker.Api.Dtos;
using BugTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api.Controllers;

// Только чтение. Создание пользователей и вход будут в AuthController вместе с JWT.
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _db;

    public UsersController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/users?role=Developer  (например, список разработчиков для планирования)
    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAll([FromQuery] Role? role)
    {
        var query = _db.Users.AsNoTracking().AsQueryable();
        if (role != null) query = query.Where(u => u.Role == role);

        return await query
            .OrderBy(u => u.Name)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Login = u.Login,
                Role = u.Role
            }).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetById(int id)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();

        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Login = user.Login,
            Role = user.Role
        };
    }
}
