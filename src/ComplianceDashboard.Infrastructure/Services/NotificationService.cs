using Microsoft.EntityFrameworkCore;
using ComplianceDashboard.Core.Entities;
using ComplianceDashboard.Core.Interfaces;
using ComplianceDashboard.Core.Enums;
using ComplianceDashboard.Infrastructure.Data;

namespace ComplianceDashboard.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly int[] _notificationThresholds = new[] { 60, 30, 15 };

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ProcessExpirationNotificationsAsync()
        {
            foreach (var threshold in _notificationThresholds)
            {
                var expiryDate = DateTime.UtcNow.AddDays(threshold);
                var documentsNearingExpiry = await _context.ComplianceDocuments
                    .Where(d => d.ExpiryDate.Date == expiryDate.Date && d.IsActive)
                    .ToListAsync();

                foreach (var document in documentsNearingExpiry)
                {
                    if (!await NotificationExistsAsync(document.Id, (NotificationType)threshold))
                    {
                        var notification = new ComplianceNotification
                        {
                            DocumentId = document.Id,
                            NotificationType = (NotificationType)threshold,
                            NotificationDate = DateTime.UtcNow,
                            IsAcknowledged = false
                        };

                        _context.ComplianceNotifications.Add(notification);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ComplianceNotification>> GetPendingNotificationsAsync(int employeeId)
        {
            return await _context.ComplianceNotifications
                .Include(n => n.Document)
                .Where(n => n.Document.EmployeeId == employeeId && !n.IsAcknowledged)
                .OrderByDescending(n => n.NotificationDate)
                .ToListAsync();
        }

        public async Task MarkNotificationAsAcknowledgedAsync(int notificationId)
        {
            var notification = await _context.ComplianceNotifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsAcknowledged = true;
                await _context.SaveChangesAsync();
            }
        }

        private async Task<bool> NotificationExistsAsync(int documentId, NotificationType notificationType)
        {
            return await _context.ComplianceNotifications
                .AnyAsync(n => n.DocumentId == documentId && n.NotificationType == notificationType);
        }
    }
}