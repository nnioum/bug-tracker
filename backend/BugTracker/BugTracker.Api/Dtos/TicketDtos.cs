using System.ComponentModel.DataAnnotations;
using BugTracker.Api.Models;

namespace BugTracker.Api.Dtos;

public class TicketCreateRequest
{
    [Required, MaxLength(200)] public string Title { get; set; } = string.Empty;
    [Required, MaxLength(4000)] public string Description { get; set; } = string.Empty;
    [MaxLength(4000)] public string? StepsToReproduce { get; set; }
    [EnumDataType(typeof(Priority))] public Priority Priority { get; set; } = Priority.Medium;

    [Range(1, int.MaxValue)] public int SoftwareId { get; set; }
}

public class TicketUpdateRequest
{
    [Required, MaxLength(200)] public string Title { get; set; } = string.Empty;
    [Required, MaxLength(4000)] public string Description { get; set; } = string.Empty;
    [MaxLength(4000)] public string? StepsToReproduce { get; set; }
    [EnumDataType(typeof(Priority))] public Priority Priority { get; set; }
}

public class TicketPlanRequest
{
    [Range(1, int.MaxValue)] public int AssigneeId { get; set; }

    [Required] public DateOnly? Deadline { get; set; }

    [Required, MaxLength(2000)] public string PlanningRationale { get; set; } = string.Empty;
}

public class TicketFixRequest
{
    [Required, MaxLength(100)] public string FixRelease { get; set; } = string.Empty;
    [Required, MaxLength(500), Url] public string FixCommitsUrl { get; set; } = string.Empty;
    [MaxLength(2000)] public string? FixComment { get; set; }
}

public class TicketReviewRequest
{
    [Required, MaxLength(2000)] public string ReviewResult { get; set; } = string.Empty;
}

public class TicketFilterRequest
{
    public int? SoftwareId { get; set; }
    public TicketStatus? Status { get; set; }
    public Priority? Priority { get; set; }
    public int? AssigneeId { get; set; }
    public int? CreatedById { get; set; }

    [MaxLength(200)] public string? Search { get; set; }
}

public class TicketListItemResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Priority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateOnly? Deadline { get; set; }

    public int SoftwareId { get; set; }
    public string? SoftwareName { get; set; }
    public string? AssigneeName { get; set; }
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
