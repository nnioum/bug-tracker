namespace BugTracker.Api.Models;

public class Ticket
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? StepsToReproduce { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int SoftwareId { get; set; }
    public Software? Software { get; set; }

    public int CreatedById { get; set; }
    public User? CreatedBy { get; set; }

    public int? AssigneeId { get; set; }
    public User? Assignee { get; set; }
    public DateOnly? Deadline { get; set; }
    public string? PlanningRationale { get; set; }

    public string? FixRelease { get; set; }
    public string? FixCommitsUrl { get; set; }
    public string? FixComment { get; set; }

    public string? ReviewResult { get; set; }

    public List<Attachment> Attachments { get; set; } = new();
}
