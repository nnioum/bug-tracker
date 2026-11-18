using System.ComponentModel.DataAnnotations;

namespace BugTracker.Api.Dtos;

public class SoftwareRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Version { get; set; } = string.Empty;
    public string? RepositoryUrl { get; set; }
    public string? Description { get; set; }
}

public class SoftwareResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string? RepositoryUrl { get; set; }
    public string? Description { get; set; }
}