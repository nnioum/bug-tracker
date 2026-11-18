using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BugTracker.Api.Dtos;

public class AttachmentUploadRequest
{
    [Required] public IFormFile? File { get; set; }
}

public class AttachmentResponse
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int TicketId { get; set; }
}
