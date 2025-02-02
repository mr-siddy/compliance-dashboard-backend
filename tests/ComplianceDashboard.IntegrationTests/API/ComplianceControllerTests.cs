using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit;
using ComplianceDashboard.Shared.DTOs;

namespace ComplianceDashboard.IntegrationTests.API
{
    public class ComplianceControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ComplianceControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetComplianceData_ReturnsSuccessStatusCode()
        {
            // Arrange
            var filter = new ComplianceFilterDTO
            {
                Departments = new[] { "HR" },
                Categories = new[] { "DriversLicense" }
            };

            // Act
            var response = await _client.GetAsync("/api/compliance?" + 
                $"departments={string.Join(",", filter.Departments)}&" +
                $"categories={string.Join(",", filter.Categories)}");

            // Assert
            response.EnsureSuccessStatusCode();
            var complianceData = await response.Content.ReadFromJsonAsync<IEnumerable<ComplianceDTO>>();
            Assert.NotNull(complianceData);
        }

        [Fact]
        public async Task Export_ReturnsPdfFile()
        {
            // Arrange
            var exportRequest = new ExportDTO
            {
                Format = "pdf",
                SelectedCategories = new[] { "DriversLicense" },
                Departments = new[] { "HR" }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/export", exportRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", response.Content.Headers.ContentType?.MediaType);
        }

        [Fact]
        public async Task Export_ReturnsCsvFile()
        {
            // Arrange
            var exportRequest = new ExportDTO
            {
                Format = "csv",
                SelectedCategories = new[] { "DriversLicense" },
                Departments = new[] { "HR" }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/export", exportRequest);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/csv", response.Content.Headers.ContentType?.MediaType);
        }
    }
}