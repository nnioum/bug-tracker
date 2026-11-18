using BugTracker.Api.Data;
using BugTracker.Api.Dtos;
using BugTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _db;

    public TicketsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<TicketListItemResponse>>> GetAll([FromQuery] TicketFilterRequest filter)
    {
        var query = _db.Tickets.AsNoTracking().AsQueryable();

        if (filter.SoftwareId != null) query = query.Where(t => t.SoftwareId == filter.SoftwareId);
        if (filter.Status != null) query = query.Where(t => t.Status == filter.Status);
        if (filter.Priority != null) query = query.Where(t => t.Priority == filter.Priority);
        if (filter.AssigneeId != null) query = query.Where(t => t.AssigneeId == filter.AssigneeId);
        if (filter.CreatedById != null) query = query.Where(t => t.CreatedById == filter.CreatedById);
        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(t => t.Title.ToLower().Contains(filter.Search.ToLower()));

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TicketListItemResponse
            {
                Id = t.Id,
                Title = t.Title,
                Priority = t.Priority,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                Deadline = t.Deadline,
                SoftwareId = t.SoftwareId,
                SoftwareName = t.Software!.Name,
                AssigneeName = t.Assignee != null ? t.Assignee.Name : null
            }).ToListAsync();

        return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketResponse>> GetById(int id)
    {
        var ticket = await LoadFull().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return NotFound();

        return ToResponse(ticket);
    }


    [HttpPost]
    public async Task<ActionResult<TicketResponse>> Create(TicketCreateRequest request, [FromQuery] int createdById)
    {
        if (!await _db.Softwares.AnyAsync(s => s.Id == request.SoftwareId))
            return BadRequest("ПО с таким Id не найдено");
        if (!await _db.Users.AnyAsync(u => u.Id == createdById))
            return BadRequest("Пользователь с таким Id не найден");

        var ticket = new Ticket
        {
            Title = request.Title,
            Description = request.Description,
            StepsToReproduce = request.StepsToReproduce,
            Priority = request.Priority,
            SoftwareId = request.SoftwareId,
            CreatedById = createdById
        };

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();

        var created = await LoadFull().AsNoTracking().FirstAsync(t => t.Id == ticket.Id);
        return CreatedAtAction(nameof(GetById), new { id = ticket.Id }, ToResponse(created));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TicketResponse>> Update(int id, TicketUpdateRequest request)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return NotFound();

        ticket.Title = request.Title;
        ticket.Description = request.Description;
        ticket.StepsToReproduce = request.StepsToReproduce;
        ticket.Priority = request.Priority;
        await _db.SaveChangesAsync();

        var updated = await LoadFull().AsNoTracking().FirstAsync(t => t.Id == id);
        return Ok(ToResponse(updated));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return NotFound();

        _db.Tickets.Remove(ticket);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private IQueryable<Ticket> LoadFull()
    {
        return _db.Tickets
            .Include(t => t.Software)
            .Include(t => t.CreatedBy)
            .Include(t => t.Assignee)
            .Include(t => t.Attachments);
    }

    private static TicketResponse ToResponse(Ticket t)
    {
        return new TicketResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            StepsToReproduce = t.StepsToReproduce,
            Priority = t.Priority,
            Status = t.Status,
            CreatedAt = t.CreatedAt,
            SoftwareId = t.SoftwareId,
            SoftwareName = t.Software?.Name,
            CreatedById = t.CreatedById,
            CreatedByName = t.CreatedBy?.Name,
            AssigneeId = t.AssigneeId,
            AssigneeName = t.Assignee?.Name,
            Deadline = t.Deadline,
            PlanningRationale = t.PlanningRationale,
            FixRelease = t.FixRelease,
            FixCommitsUrl = t.FixCommitsUrl,
            FixComment = t.FixComment,
            ReviewResult = t.ReviewResult,
            Attachments = t.Attachments.Select(a => new AttachmentResponse
            {
                Id = a.Id,
                FileName = a.FileName,
                TicketId = a.TicketId
            }).ToList()
        };
    }
}
