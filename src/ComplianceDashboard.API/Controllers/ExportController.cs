using Microsoft.AspNetCore.Mvc;
using ComplianceDashboard.Core.Interfaces;
using ComplianceDashboard.Shared.DTOs;
using ComplianceDashboard.Core.Entities;
using ComplianceDashboard.Core.Enums;

namespace ComplianceDashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly IComplianceService _complianceService;
        private readonly IExportService _exportService;

        public ExportController(
            IComplianceService complianceService,
            IExportService exportService)
        {
            _complianceService = complianceService;
            _exportService = exportService;
        }

        [HttpPost]
        public async Task<IActionResult> ExportComplianceData([FromBody] ExportDTO exportRequest)
        {
            var documents = await _complianceService.GetComplianceDataAsync(
                exportRequest.Departments,
                exportRequest.SelectedCategories?.Select(c => Enum.Parse<DocumentType>(c)).ToArray());

            byte[] fileContents;
            string contentType;
            string fileName;

            if (exportRequest.Format.ToLower() == "pdf")
            {
                fileContents = await _exportService.ExportToPdfAsync(
                    documents, 
                    exportRequest.SelectedCategories);
                contentType = "application/pdf";
                fileName = "ComplianceReport.pdf";
            }
            else
            {
                fileContents = await _exportService.ExportToCsvAsync(
                    documents, 
                    exportRequest.SelectedCategories);
                contentType = "text/csv";
                fileName = "ComplianceReport.csv";
            }

            return File(fileContents, contentType, fileName);
        }
    }
}