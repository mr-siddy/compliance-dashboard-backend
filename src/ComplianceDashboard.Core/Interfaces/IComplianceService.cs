using ComplianceDashboard.Core.Entities;
using ComplianceDashboard.Core.Enums;


namespace ComplianceDashboard.Core.Interfaces
{
    public interface IComplianceService
    {
        Task<IEnumerable<ComplianceDocument>> GetComplianceDataAsync(
            string[] departments = null,
            DocumentType[] categories = null);
            
        Task<ComplianceDocument> GetDocumentByIdAsync(int id);
        Task<bool> UpdateDocumentAsync(ComplianceDocument document);
        Task<IEnumerable<ComplianceDocument>> GetExpiringDocumentsAsync(int daysThreshold);
    }
}
