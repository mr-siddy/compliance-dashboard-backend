namespace ComplianceDashboard.Shared.DTOs
{
    public class ComplianceDTO
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string IdNumber { get; set; }
        public string Department { get; set; }
        public string DocumentType { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string Status { get; set; }
    }

    public class ComplianceFilterDTO
    {
        public string[] Departments { get; set; }
        public string[] Categories { get; set; }
    }

    public class ComplianceUpdateDTO
    {
        public int Id { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string DocumentNumber { get; set; }
    }
}
