# compliance-dashboard-backend
.Net based backend for compliance-dashboard

To Run:
# Navigate to the API project directory (if not already there)
cd ../complianceDashboard-backend/src/ComplianceDashboard.API

# Clean and rebuild to ensure all changes are applied
dotnet clean
dotnet build

# Run in Development mode
dotnet run --environment Development

Once the application starts, you can access the Swagger UI at:

> http://localhost:5000/swagger

You'll see all the available API endpoints:

Compliance Controller
> GET /api/Compliance - Get compliance data with filters
> POST /api/Compliance/acknowledge - Acknowledge notifications
Export Controller
> POST /api/Export - Export compliance data in different formats
You can test these endpoints directly in the Swagger UI by:

Clicking on an endpoint to expand it
Clicking "Try it out"
Filling in any required parameters
Clicking "Execute"
