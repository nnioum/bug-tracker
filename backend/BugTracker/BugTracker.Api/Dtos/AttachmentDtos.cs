namespace BugTracker.Api.Dtos;

public class AttachmentResponse
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int TicketId { get; set; }
}