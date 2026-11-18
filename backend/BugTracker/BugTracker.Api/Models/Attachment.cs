namespace BugTracker.Api.Models;

/// <summary>Вложение к тикету (скриншот, лог и т.п.).</summary>
public class Attachment
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;

    // Путь к файлу на диске сервера (само содержимое в БД не хранится)
    public string FilePath { get; set; } = string.Empty;

    public int TicketId { get; set; }
    public Ticket? Ticket { get; set; }
}
