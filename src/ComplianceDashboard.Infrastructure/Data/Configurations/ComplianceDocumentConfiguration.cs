using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ComplianceDashboard.Core.Entities;

namespace ComplianceDashboard.Infrastructure.Data.Configurations
{
    public class ComplianceDocumentConfiguration : IEntityTypeConfiguration<ComplianceDocument>
    {
        public void Configure(EntityTypeBuilder<ComplianceDocument> builder)
        {
            builder.ToTable("ComplianceDocuments");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.DocumentType)
                .IsRequired();

            builder.Property(d => d.IssueDate)
                .IsRequired();

            builder.Property(d => d.ExpiryDate)
                .IsRequired();

            builder.Property(d => d.DocumentNumber)
                .HasMaxLength(100);

            builder.Property(d => d.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Employee relationship
            builder.HasOne(d => d.Employee)
                .WithMany(e => e.ComplianceDocuments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for faster lookups
            builder.HasIndex(d => new { d.EmployeeId, d.DocumentType, d.IsActive });
            builder.HasIndex(d => d.ExpiryDate);
        }
    }
}