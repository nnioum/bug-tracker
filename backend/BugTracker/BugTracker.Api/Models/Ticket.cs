namespace BugTracker.Api.Models;

/// <summary>Тикет: описание найденного бага и ход его исправления.</summary>
public class Ticket
{
    public int Id { get; set; }

    // Заполняет тестировщик при создании
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? StepsToReproduce { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // К какому ПО относится тикет
    public int SoftwareId { get; set; }
    public Software? Software { get; set; }

    // Кто создал тикет (тестировщик)
    public int CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    // Планирование (заполняет тимлид)
    public int? AssigneeId { get; set; }
    public User? Assignee { get; set; }
    public DateOnly? Deadline { get; set; }
    public string? PlanningRationale { get; set; }

    // Исправление (заполняет разработчик)
    public string? FixRelease { get; set; }
    public string? FixCommitsUrl { get; set; }
    public string? FixComment { get; set; }

    // Проверка исправления (заполняет тестировщик)
    public string? ReviewResult { get; set; }

    // Вложения удаляются вместе с тикетом
    public List<Attachment> Attachments { get; set; } = new();
}
