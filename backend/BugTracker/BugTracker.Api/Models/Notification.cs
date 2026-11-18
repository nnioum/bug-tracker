namespace BugTracker.Api.Models;

/// <summary>Уведомление пользователя о событии по тикету.</summary>
public class Notification
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }

    // Кому адресовано уведомление
    public int RecipientId { get; set; }
    public User? Recipient { get; set; }

    // По какому тикету
    public int TicketId { get; set; }
    public Ticket? Ticket { get; set; }
}
