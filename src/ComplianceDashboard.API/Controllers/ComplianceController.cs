using Microsoft.AspNetCore.Mvc;
using ComplianceDashboard.Core.Interfaces;
using ComplianceDashboard.Shared.DTOs;
using ComplianceDashboard.Core.Entities;
using ComplianceDashboard.Core.Enums;
using ComplianceDashboard.Shared.Extensions;  

namespace ComplianceDashboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplianceController : ControllerBase
    {
        private readonly IComplianceService _complianceService;
        private readonly INotificationService _notificationService;

        public ComplianceController(
            IComplianceService complianceService,
            INotificationService notificationService)
        {
            _complianceService = complianceService;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplianceDTO>>> GetComplianceData(
            [FromQuery] ComplianceFilterDTO filter)
        {
            var documents = await _complianceService.GetComplianceDataAsync(
                filter.Departments,
                filter.Categories?.Select(c => Enum.Parse<DocumentType>(c)).ToArray());

            var dtos = documents.Select(doc => new ComplianceDTO
            {
                Id = doc.Id,
                EmployeeName = $"{doc.Employee.FirstName} {doc.Employee.LastName}",
                IdNumber = doc.Employee.IdNumber,
                Department = doc.Employee.Department.Name,
                DocumentType = doc.DocumentType.ToString(),
                IssueDate = doc.IssueDate,
                ExpiryDate = doc.ExpiryDate,
                DaysUntilExpiry = doc.ExpiryDate.CalculateDaysUntilExpiry(),
                Status = doc.ExpiryDate.GetExpiryStatus(),
                // RequiresAttention = doc.ExpiryDate.CalculateDaysUntilExpiry() <= 60
            });

            return Ok(dtos);
        }

        [HttpGet("notifications/{employeeId}")]
        public async Task<ActionResult<IEnumerable<ComplianceNotification>>> GetNotifications(
            int employeeId)
        {
            var notifications = await _notificationService
                .GetPendingNotificationsAsync(employeeId);
            return Ok(notifications);
        }

        [HttpPost("notifications/{notificationId}/acknowledge")]
        public async Task<ActionResult> AcknowledgeNotification(int notificationId)
        {
            await _notificationService.MarkNotificationAsAcknowledgedAsync(notificationId);
            return Ok();
        }
    }
}
