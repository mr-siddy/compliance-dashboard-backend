// ComplianceDashboard.Infrastructure/Services/ExportService.cs
using ComplianceDashboard.Core.Interfaces;
using ComplianceDashboard.Core.Entities;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Text;
using CsvHelper;
using System.Globalization;

namespace ComplianceDashboard.Infrastructure.Services
{
    public class ExportService : IExportService
    {
        public async Task<byte[]> ExportToPdfAsync(
            IEnumerable<ComplianceDocument> documents,
            string[]? selectedCategories = null)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new PdfWriter(memoryStream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            // Add Title
            document.Add(new Paragraph("Compliance Report")
                .SetFontSize(20)
                .SetBold());

            // Add Report Generation Date
            document.Add(new Paragraph($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
                .SetFontSize(12));

            // Create table
            var table = new Table(6)
                .UseAllAvailableWidth();

            // Add headers
            var headers = new[] { "Employee", "Department", "Document Type", "Issue Date", "Expiry Date", "Status" };
            foreach (var header in headers)
            {
                table.AddCell(new Cell().Add(new Paragraph(header).SetBold()));
            }

            // Add data rows
            foreach (var doc in documents)
            {
                if (selectedCategories?.Any() == true && 
                    !selectedCategories.Contains(doc.DocumentType.ToString()))
                    continue;

                table.AddCell(new Cell().Add(new Paragraph($"{doc.Employee.FirstName} {doc.Employee.LastName}")));
                table.AddCell(new Cell().Add(new Paragraph(doc.Employee.Department.Name)));
                table.AddCell(new Cell().Add(new Paragraph(doc.DocumentType.ToString())));
                table.AddCell(new Cell().Add(new Paragraph(doc.IssueDate.ToString("yyyy-MM-dd"))));
                table.AddCell(new Cell().Add(new Paragraph(doc.ExpiryDate.ToString("yyyy-MM-dd"))));
                
                var daysUntilExpiry = (doc.ExpiryDate - DateTime.Now).Days;
                var status = daysUntilExpiry switch
                {
                    < 0 => "Expired",
                    0 => "Expires Today",
                    <= 15 => $"Expires in {daysUntilExpiry} days",
                    _ => "Valid"
                };
                table.AddCell(new Cell().Add(new Paragraph(status)));
            }

            document.Add(table);
            document.Close();

            return memoryStream.ToArray();
        }

        public async Task<byte[]> ExportToCsvAsync(
            IEnumerable<ComplianceDocument> documents,
            string[]? selectedCategories = null)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            // Write headers
            csv.WriteField("Employee Name");
            csv.WriteField("Department");
            csv.WriteField("Document Type");
            csv.WriteField("Issue Date");
            csv.WriteField("Expiry Date");
            csv.WriteField("Days Until Expiry");
            csv.WriteField("Status");
            csv.NextRecord();

            // Write data
            foreach (var doc in documents)
            {
                if (selectedCategories?.Any() == true && 
                    !selectedCategories.Contains(doc.DocumentType.ToString()))
                    continue;

                var daysUntilExpiry = (doc.ExpiryDate - DateTime.Now).Days;
                var status = daysUntilExpiry switch
                {
                    < 0 => "Expired",
                    0 => "Expires Today",
                    <= 15 => $"Expires in {daysUntilExpiry} days",
                    _ => "Valid"
                };

                csv.WriteField($"{doc.Employee.FirstName} {doc.Employee.LastName}");
                csv.WriteField(doc.Employee.Department.Name);
                csv.WriteField(doc.DocumentType.ToString());
                csv.WriteField(doc.IssueDate.ToString("yyyy-MM-dd"));
                csv.WriteField(doc.ExpiryDate.ToString("yyyy-MM-dd"));
                csv.WriteField(daysUntilExpiry);
                csv.WriteField(status);
                csv.NextRecord();
            }

            writer.Flush();
            return memoryStream.ToArray();
        }
    }
}