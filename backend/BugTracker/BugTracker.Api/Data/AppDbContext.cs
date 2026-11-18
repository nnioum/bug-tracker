using Microsoft.EntityFrameworkCore;
using BugTracker.Api.Models;

namespace BugTracker.Api.Data;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Software> Softwares { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
}