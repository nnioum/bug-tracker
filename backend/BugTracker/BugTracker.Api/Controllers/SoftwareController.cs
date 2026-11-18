using BugTracker.Api.Data;
using BugTracker.Api.Dtos;
using BugTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SoftwareController: ControllerBase
{
    private readonly AppDbContext _db;
    
    public SoftwareController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<SoftwareResponse>>> GetAll()
    {
        var items = await _db.Softwares
            .AsNoTracking()
            .Select(s => new SoftwareResponse{
                Id = s.Id,
                Name = s.Name,
                Version = s.Version,
                RepositoryUrl = s.RepositoryUrl,
                Description = s.Description,
            }).ToListAsync();

        return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SoftwareResponse>> GetById(int id)
    {
        var software = await _db.Softwares
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
        if (software == null) return NotFound();
        
        return ToResponse(software);
    }

    [HttpPost]
    public async Task<ActionResult<SoftwareResponse>> Create(SoftwareRequest request)
    {
        if (await NameExists(request.Name, null))
            return Conflict("ПО с таким названием уже существует");

        var newSoftware = new Software
        {
            Name = request.Name,
            Version = request.Version,
            RepositoryUrl = request.RepositoryUrl,
            Description = request.Description
        };

        _db.Softwares.Add(newSoftware);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = newSoftware.Id }, ToResponse(newSoftware));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SoftwareResponse>> Update(int id, SoftwareRequest request)
    {
        var software = await _db.Softwares
            .FirstOrDefaultAsync(s => s.Id == id);
        if (software == null) return NotFound();

        if (await NameExists(request.Name, id))
            return Conflict("ПО с таким названием уже существует");
        
        software.Name = request.Name;
        software.Version = request.Version;
        software.RepositoryUrl = request.RepositoryUrl;
        software.Description = request.Description;
        await _db.SaveChangesAsync();
        return Ok(ToResponse(software));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, [FromQuery] int userId)
    {
        // Удалять ПО может только тимлид
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return BadRequest("Пользователь с таким Id не найден");
        if (user.Role != Role.TeamLead)
            return StatusCode(403, "Удалять ПО может только тимлид");

        var software = await _db.Softwares
            .FirstOrDefaultAsync(s => s.Id == id);
        if (software == null) return NotFound();
        _db.Softwares.Remove(software);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private async Task<bool> NameExists(string name, int? exceptId)
    {
        var lower = name.Trim().ToLower();
        return await _db.Softwares.AnyAsync(s => s.Name.ToLower() == lower && s.Id != exceptId);
    }

    private static SoftwareResponse ToResponse(Software s)
    {
        return new SoftwareResponse
        {
            Id = s.Id,
            Name = s.Name,
            Version = s.Version,
            RepositoryUrl = s.RepositoryUrl,
            Description = s.Description
        };
    }
}