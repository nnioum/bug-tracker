using BugTracker.Api.Data;
using BugTracker.Api.Dtos;
using BugTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Api.Controllers;


[ApiController]
[Route("api/tickets/{id}")]
public class TicketActionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public TicketActionsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("plan")]
    public async Task<ActionResult> Plan(int id, [FromQuery] int userId, TicketPlanRequest request)
    {
        var (ticket, error) = await Prepare(id, userId, Role.TeamLead, TicketStatus.New);
        if (error != null) return error;

        if (request.Deadline!.Value < DateOnly.FromDateTime(DateTime.UtcNow))
            return BadRequest("Дедлайн не может быть в прошлом");

        var assignee = await _db.Users.FirstOrDefaultAsync(u => u.Id == request.AssigneeId);
        if (assignee == null || assignee.Role != Role.Developer)
            return BadRequest("Ответственным можно назначить только разработчика");

        ticket!.AssigneeId = assignee.Id;
        ticket.Deadline = request.Deadline;
        ticket.PlanningRationale = request.PlanningRationale;
        ticket.Status = TicketStatus.Planned;

        AddNotification(NotificationType.Assigned, assignee.Id, ticket.Id);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("start")]
    public async Task<ActionResult> Start(int id, [FromQuery] int userId)
    {
        var (ticket, error) = await Prepare(id, userId, Role.Developer, TicketStatus.Planned, TicketStatus.Rework);
        if (error != null) return error;

        ticket!.Status = TicketStatus.InProgress;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("fix")]
    public async Task<ActionResult> Fix(int id, [FromQuery] int userId, TicketFixRequest request)
    {
        var (ticket, error) = await Prepare(id, userId, Role.Developer, TicketStatus.InProgress);
        if (error != null) return error;

        ticket!.FixRelease = request.FixRelease;
        ticket.FixCommitsUrl = string.Join("\n", request.Commits
            .Select(c => c.Trim())
            .Where(c => c != ""));
        ticket.FixComment = request.FixComment;
        ticket.Status = TicketStatus.InReview;

        AddNotification(NotificationType.FixReady, ticket.CreatedById, ticket.Id);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("return")]
    public async Task<ActionResult> ReturnToRework(int id, [FromQuery] int userId, TicketReviewRequest request)
    {
        var (ticket, error) = await Prepare(id, userId, Role.Tester, TicketStatus.InReview);
        if (error != null) return error;

        ticket!.ReviewResult = request.ReviewResult;
        ticket.Status = TicketStatus.Rework;

        if (ticket.AssigneeId != null)
            AddNotification(NotificationType.ReturnedToRework, ticket.AssigneeId.Value, ticket.Id);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("close")]
    public async Task<ActionResult> Close(int id, [FromQuery] int userId, TicketCloseRequest request)
    {
        var (ticket, error) = await Prepare(id, userId, Role.Tester, TicketStatus.InReview);
        if (error != null) return error;

        ticket!.ReviewResult = request.ReviewResult;
        ticket.Status = TicketStatus.Closed;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private async Task<(Ticket? ticket, ActionResult? error)> Prepare(
        int id, int userId, Role role, params TicketStatus[] allowedStatuses)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return (null, NotFound());

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return (null, BadRequest("Пользователь с таким Id не найден"));

        if (user.Role != role)
            return (null, StatusCode(403, "Это действие недоступно для вашей роли"));

        if (role == Role.Developer && ticket.AssigneeId != userId)
            return (null, StatusCode(403, "Тикет назначен другому разработчику"));

        if (!allowedStatuses.Contains(ticket.Status))
            return (null, Conflict($"Действие недоступно для тикета в статусе {ticket.Status}"));

        return (ticket, null);
    }

    private void AddNotification(NotificationType type, int recipientId, int ticketId)
    {
        _db.Notifications.Add(new Notification
        {
            Type = type,
            RecipientId = recipientId,
            TicketId = ticketId
        });
    }
}
