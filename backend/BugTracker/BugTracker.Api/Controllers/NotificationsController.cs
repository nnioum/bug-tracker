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
                TicketTitle = n.Ticket!.Title
            }).ToListAsync();
    }

    [HttpGet("unread-count")]
    public async Task<ActionResult<UnreadCountResponse>> GetUnreadCount([FromQuery] int recipientId)
    {
        var count = await _db.Notifications.CountAsync(n => n.RecipientId == recipientId && !n.IsRead);
        return new UnreadCountResponse { Count = count };
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
