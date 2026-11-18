namespace BugTracker.Api.Models;

/// <summary>Статус тикета в жизненном цикле.</summary>
public enum TicketStatus
{
    New,         // Новый
    Planned,     // Запланирован
    InProgress,  // В работе
    InReview,    // На проверке
    Rework,      // На доработке
    Closed       // Закрыт
}
