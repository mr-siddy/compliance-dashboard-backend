using Microsoft.EntityFrameworkCore;
using ComplianceDashboard.Core.Entities;
using ComplianceDashboard.Core.Enums;
using ComplianceDashboard.Infrastructure.Data;

namespace ComplianceDashboard.Infrastructure.Repositories
{
    public class ComplianceRepository : BaseRepository<ComplianceDocument>
    {
        private readonly ApplicationDbContext _context;
        public ComplianceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplianceDocument>> GetComplianceDataAsync(
            string[] departments = null,
            DocumentType[] categories = null)
        {
            var query = DbSet
                .Include(d => d.Employee)
                    .ThenInclude(e => e.Department)
                .Include(d => d.Notifications)
                .AsQueryable();

            if (departments?.Any() == true)
            {
                query = query.Where(d => departments.Contains(d.Employee.Department.Name));
            }

            if (categories?.Any() == true)
            {
                query = query.Where(d => categories.Contains(d.DocumentType));
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ComplianceDocument>> GetExpiringDocumentsAsync(int daysThreshold)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
            return await DbSet
                .Include(d => d.Employee)
                .Include(d => d.Notifications)
                .Where(d => d.ExpiryDate <= thresholdDate && d.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<ComplianceDocument>> GetDocumentsByEmployeeAsync(int employeeId)
        {
            return await DbSet
                .Include(d => d.Notifications)
                .Where(d => d.EmployeeId == employeeId && d.IsActive)
                .ToListAsync();
        }

        public async Task<bool> HasValidDocumentAsync(int employeeId, DocumentType documentType)
        {
            return await DbSet.AnyAsync(d => 
                d.EmployeeId == employeeId && 
                d.DocumentType == documentType && 
                d.ExpiryDate > DateTime.UtcNow &&
                d.IsActive);
        }
    }
}
