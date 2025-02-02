namespace ComplianceDashboard.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateDaysUntilExpiry(this DateTime expiryDate)
        {
            var today = DateTime.UtcNow.Date;
            return (expiryDate.Date - today).Days;
        }

        public static string GetExpiryStatus(this DateTime expiryDate)
        {
            var daysUntilExpiry = expiryDate.CalculateDaysUntilExpiry();
            
            return daysUntilExpiry switch
            {
                < 0 => "Expired",
                0 => "Expires Today",
                <= 15 => "Expires in 15 days",
                <= 30 => "Expires in 30 days",
                <= 60 => "Expires in 60 days",
                _ => "Valid"
            };
        }

        public static string GetStatusColor(this DateTime expiryDate)
        {
            var daysUntilExpiry = expiryDate.CalculateDaysUntilExpiry();
            
            return daysUntilExpiry switch
            {
                < 0 => "text-red-600",
                <= 15 => "text-orange-600",
                <= 30 => "text-yellow-600",
                _ => "text-green-600"
            };
        }
    }
}
