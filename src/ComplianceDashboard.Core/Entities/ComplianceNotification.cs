using ComplianceDashboard.Core.Enums;


namespace ComplianceDashboard.Core.Entities
{
    public class ComplianceNotification
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime NotificationDate { get; set; }
        public bool IsAcknowledged { get; set; }
        public virtual ComplianceDocument Document { get; set; }
    }
}