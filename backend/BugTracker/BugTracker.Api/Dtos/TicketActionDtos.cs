using System.ComponentModel.DataAnnotations;

namespace BugTracker.Api.Dtos;

public class TicketCloseRequest
{
    [MaxLength(2000)] public string? ReviewResult { get; set; }
}
