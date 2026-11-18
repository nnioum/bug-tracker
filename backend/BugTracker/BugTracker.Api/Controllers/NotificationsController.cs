using BugTracker.Api.Data;
using BugTracker.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public NotificationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<NotificationResponse>>> GetAll([FromQuery] int recipientId)
    {
        return await _db.Notifications
            .AsNoTracking()
            .Where(n => n.RecipientId == recipientId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationResponse
            {
                Id = n.Id,
                Type = n.Type,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead,
                TicketId = n.TicketId,
                TicketTitle = n.Ticket!.Title,
                TicketStatus = n.Ticket.Status,
                TicketDeadline = n.Ticket.Deadline
            }).ToListAsync();
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<UnreadCountResponse>> GetUnreadCount([FromQuery] int recipientId)
    {
        var count = await _db.Notifications.CountAsync(n => n.RecipientId == recipientId && !n.IsRead);
        return new UnreadCountResponse { Count = count };
    }

    [HttpPut("read-all")]
    public async Task<ActionResult> MarkAllAsRead([FromQuery] int recipientId)
    {
        await _db.Notifications
            .Where(n => n.RecipientId == recipientId && !n.IsRead)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
        return NoContent();
    }

    [HttpPut("{id}/read")]
    public async Task<ActionResult> MarkAsRead(int id)
    {
        var notification = await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id);
        if (notification == null) return NotFound();

        notification.IsRead = true;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
