using Fixit_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fixit_API.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; } = null!;
        public DbSet<TenantProfile> TenantProfiles { get; set; } = null!;
        public DbSet<MaintenanceTicket> MaintenanceTickets { get; set; } = null!;
        public DbSet<TicketStatusHistory> TicketStatusHistories { get; set; } = null!;
        public DbSet<TicketComment> TicketComments { get; set; } = null!;
        public DbSet<TicketAttachment> TicketAttachments { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TicketComment>()
                .HasOne(c => c.AuthorUser)
                .WithMany(u => u.TicketComments)
                .HasForeignKey(c => c.AuthorUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TicketStatusHistory>()
                .HasOne(h => h.ChangedByUser)
                .WithMany(u => u.TicketStatusHistoryEntries)
                .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
