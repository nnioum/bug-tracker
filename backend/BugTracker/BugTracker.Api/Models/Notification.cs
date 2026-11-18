namespace BugTracker.Api.Models;

public class Notification
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }

    public int RecipientId { get; set; }
    public User? Recipient { get; set; }

    public int TicketId { get; set; }
    public Ticket? Ticket { get; set; }
}
