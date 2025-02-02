using ComplianceDashboard.Core.Entities;

namespace ComplianceDashboard.Core.Interfaces
{
    public interface INotificationService
    {
        Task ProcessExpirationNotificationsAsync();
        Task<IEnumerable<ComplianceNotification>> GetPendingNotificationsAsync(int employeeId);
        Task MarkNotificationAsAcknowledgedAsync(int notificationId);
    }
}