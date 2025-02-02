using ComplianceDashboard.Core.Interfaces;
using ComplianceDashboard.Core.Entities;
using ComplianceDashboard.Core.Enums;
using ComplianceDashboard.Infrastructure.Repositories;

namespace ComplianceDashboard.Infrastructure.Services
{
    public class ComplianceService : IComplianceService
    {
        private readonly ComplianceRepository _complianceRepository;
        private readonly INotificationService _notificationService;

        public ComplianceService(
            ComplianceRepository complianceRepository,
            INotificationService notificationService)
        {
            _complianceRepository = complianceRepository;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ComplianceDocument>> GetComplianceDataAsync(
            string[]? departments = null,
            DocumentType[]? categories = null)
        {
            return await _complianceRepository.GetComplianceDataAsync(departments, categories);
        }

        public async Task<ComplianceDocument> GetDocumentByIdAsync(int id)
        {
            return await _complianceRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateDocumentAsync(ComplianceDocument document)
        {
            try
            {
                await _complianceRepository.UpdateAsync(document);
                
                // Process notifications after update
                await _notificationService.ProcessExpirationNotificationsAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ComplianceDocument>> GetExpiringDocumentsAsync(int daysThreshold)
        {
            return await _complianceRepository.GetExpiringDocumentsAsync(daysThreshold);
        }
    }
}
