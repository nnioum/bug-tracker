namespace BugTracker.Api.Models;

/// <summary>Программное обеспечение (проект), по которому заводятся тикеты.</summary>
public class Software
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string? RepositoryUrl { get; set; }
    public string? Description { get; set; }

    // При удалении ПО удаляются все его тикеты
    public List<Ticket> Tickets { get; set; } = new();
}
