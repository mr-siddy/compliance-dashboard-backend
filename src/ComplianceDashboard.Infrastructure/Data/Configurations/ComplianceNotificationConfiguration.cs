using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComplianceDashboard.Core.Entities;

namespace ComplianceDashboard.Infrastructure.Data.Configurations
{
    public class ComplianceNotificationConfiguration : IEntityTypeConfiguration<ComplianceNotification>
    {
        public void Configure(EntityTypeBuilder<ComplianceNotification> builder)
        {
            builder.ToTable("ComplianceNotifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.NotificationType)
                .IsRequired();

            builder.Property(n => n.NotificationDate)
                .IsRequired();

            builder.Property(n => n.IsAcknowledged)
                .IsRequired()
                .HasDefaultValue(false);

            // Document relationship
            builder.HasOne(n => n.Document)
                .WithMany(d => d.Notifications)
                .HasForeignKey(n => n.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for faster lookups
            builder.HasIndex(n => new { n.DocumentId, n.NotificationType, n.IsAcknowledged });
        }
    }
}