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
        
        software.Name = request.Name;
        software.Version = request.Version;
        software.RepositoryUrl = request.RepositoryUrl;
        software.Description = request.Description;
        await _db.SaveChangesAsync();
        return Ok(ToResponse(software));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var software = await _db.Softwares
            .FirstOrDefaultAsync(s => s.Id == id);
        if (software == null) return NotFound();
        _db.Softwares.Remove(software);
        await _db.SaveChangesAsync();
        return NoContent();
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