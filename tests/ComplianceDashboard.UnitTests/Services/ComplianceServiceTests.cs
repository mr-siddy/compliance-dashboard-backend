using Xunit;
using Moq;
using ComplianceDashboard.Core.Interfaces;
using ComplianceDashboard.Core.Entities;
using ComplianceDashboard.Infrastructure.Services;
using ComplianceDashboard.Infrastructure.Repositories;

namespace ComplianceDashboard.UnitTests.Services
{
    public class ComplianceServiceTests
    {
        private readonly Mock<ComplianceRepository> _mockRepository;
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly ComplianceService _service;

        public ComplianceServiceTests()
        {
            _mockRepository = new Mock<ComplianceRepository>();
            _mockNotificationService = new Mock<INotificationService>();
            _service = new ComplianceService(_mockRepository.Object, _mockNotificationService.Object);
        }

        [Fact]
        public async Task GetComplianceData_ReturnsFilteredData()
        {
            // Arrange
            var departments = new[] { "HR", "IT" };
            var categories = new[] { DocumentType.DriversLicense, DocumentType.FirstAid };
            var expectedDocuments = new List<ComplianceDocument>
            {
                new ComplianceDocument { Id = 1, DocumentType = DocumentType.DriversLicense },
                new ComplianceDocument { Id = 2, DocumentType = DocumentType.FirstAid }
            };

            _mockRepository.Setup(r => r.GetComplianceDataAsync(departments, categories))
                .ReturnsAsync(expectedDocuments);

            // Act
            var result = await _service.GetComplianceDataAsync(departments, categories);

            // Assert
            Assert.Equal(expectedDocuments.Count, result.Count());
            _mockRepository.Verify(r => r.GetComplianceDataAsync(departments, categories), Times.Once);
        }

        [Fact]
        public async Task UpdateDocument_ProcessesNotifications()
        {
            // Arrange
            var document = new ComplianceDocument { Id = 1 };

            _mockRepository.Setup(r => r.UpdateAsync(document))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateDocumentAsync(document);

            // Assert
            Assert.True(result);
            _mockRepository.Verify(r => r.UpdateAsync(document), Times.Once);
            _mockNotificationService.Verify(n => n.ProcessExpirationNotificationsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetExpiringDocuments_ReturnsCorrectDocuments()
        {
            // Arrange
            int daysThreshold = 30;
            var expectedDocuments = new List<ComplianceDocument>
            {
                new ComplianceDocument 
                { 
                    Id = 1, 
                    ExpiryDate = DateTime.UtcNow.AddDays(15)
                }
            };

            _mockRepository.Setup(r => r.GetExpiringDocumentsAsync(daysThreshold))
                .ReturnsAsync(expectedDocuments);

            // Act
            var result = await _service.GetExpiringDocumentsAsync(daysThreshold);

            // Assert
            Assert.Single(result);
            _mockRepository.Verify(r => r.GetExpiringDocumentsAsync(daysThreshold), Times.Once);
        }
    }
}