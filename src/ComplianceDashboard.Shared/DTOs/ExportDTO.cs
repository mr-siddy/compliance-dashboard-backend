namespace ComplianceDashboard.Shared.DTOs
{
    public class ExportDTO
    {
        public string Format { get; set; } // PDF or CSV
        public string[] SelectedCategories { get; set; }
        public string[] Departments { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}