using ComplianceDashboard.Core.Entities;



namespace ComplianceDashboard.Core.Interfaces
{
    public interface IExportService
    {
        Task<byte[]> ExportToPdfAsync(
            IEnumerable<ComplianceDocument> documents,
            string[] selectedCategories = null);
            
        Task<byte[]> ExportToCsvAsync(
            IEnumerable<ComplianceDocument> documents,
            string[] selectedCategories = null);
    }
}