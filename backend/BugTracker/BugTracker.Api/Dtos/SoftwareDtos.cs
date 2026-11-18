using System.ComponentModel.DataAnnotations;

namespace BugTracker.Api.Dtos;

public class SoftwareRequest
{
    [Required, MaxLength(200)] public string Name { get; set; } = string.Empty;
    [Required, MaxLength(50)] public string Version { get; set; } = string.Empty;
    [MaxLength(500), Url] public string? RepositoryUrl { get; set; }
    [MaxLength(2000)] public string? Description { get; set; }
}

public class SoftwareResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string? RepositoryUrl { get; set; }
    public string? Description { get; set; }
}