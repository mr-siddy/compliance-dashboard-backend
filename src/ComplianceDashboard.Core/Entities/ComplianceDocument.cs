using ComplianceDashboard.Core.Enums;


namespace ComplianceDashboard.Core.Entities
{
    public class ComplianceDocument
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DocumentType DocumentType { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string DocumentNumber { get; set; }
        public bool IsActive { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<ComplianceNotification> Notifications { get; set; }
    }
}
