// ComplianceDashboard.Infrastructure/Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ComplianceDashboard.Core.Entities;
using ComplianceDashboard.Infrastructure.Data.Configurations;

namespace ComplianceDashboard.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ComplianceDocument> ComplianceDocuments { get; set; }
        public DbSet<ComplianceNotification> ComplianceNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ComplianceDocumentConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new ComplianceNotificationConfiguration());
            
            // Set default schema
            modelBuilder.HasDefaultSchema("dbo");
        }
    }
}