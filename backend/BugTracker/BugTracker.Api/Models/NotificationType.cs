namespace BugTracker.Api.Models;

/// <summary>Тип события, о котором приходит уведомление.</summary>
public enum NotificationType
{
    Assigned,        // Назначен
    ReturnedToRework, // Возвращён на доработку
    TicketCreated,   // Тикет создан
    FixReady         // Исправление готово
}
