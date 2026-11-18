namespace BugTracker.Api.Models;

public class Software
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string? RepositoryUrl { get; set; }
    public string? Description { get; set; }

    public List<Ticket> Tickets { get; set; } = new();
}
