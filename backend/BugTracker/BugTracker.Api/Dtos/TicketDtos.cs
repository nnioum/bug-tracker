using System.ComponentModel.DataAnnotations;
using BugTracker.Api.Models;

namespace BugTracker.Api.Dtos;

public class TicketCreateRequest
{
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    public string? StepsToReproduce { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    [Required] public int SoftwareId { get; set; }
}

public class TicketUpdateRequest
{
    [Required] public string Title { get; set; } = string.Empty;
    [Required] public string Description { get; set; } = string.Empty;
    public string? StepsToReproduce { get; set; }
    public Priority Priority { get; set; }
}

public class TicketPlanRequest
{
    [Required] public int AssigneeId { get; set; }
    [Required] public DateOnly Deadline { get; set; }
    [Required] public string PlanningRationale { get; set; } = string.Empty;
}

public class TicketFixRequest
{
    [Required] public string FixRelease { get; set; } = string.Empty;
    [Required] public string FixCommitsUrl { get; set; } = string.Empty;
    public string? FixComment { get; set; }
}

public class TicketReviewRequest
{
    [Required] public string ReviewResult { get; set; } = string.Empty;
}

public class TicketResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? StepsToReproduce { get; set; }
    public Priority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }

    public int SoftwareId { get; set; }
    public string? SoftwareName { get; set; }

    public int CreatedById { get; set; }
    public string? CreatedByName { get; set; }

    public int? AssigneeId { get; set; }
    public string? AssigneeName { get; set; }
    public DateOnly? Deadline { get; set; }
    public string? PlanningRationale { get; set; }

    public string? FixRelease { get; set; }
    public string? FixCommitsUrl { get; set; }
    public string? FixComment { get; set; }
    public string? ReviewResult { get; set; }

    public List<AttachmentResponse> Attachments { get; set; } = new();
}