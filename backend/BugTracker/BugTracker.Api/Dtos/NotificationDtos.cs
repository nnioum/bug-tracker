using BugTracker.Api.Models;

namespace BugTracker.Api.Dtos;

public class NotificationResponse
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public int TicketId { get; set; }
    public string? TicketTitle { get; set; }
    public TicketStatus TicketStatus { get; set; }
    public DateOnly? TicketDeadline { get; set; }
}

public class UnreadCountResponse
{
    public int Count { get; set; }
}
