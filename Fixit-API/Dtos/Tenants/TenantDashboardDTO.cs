namespace Fixit_API.Dtos.Tenants
{
    public class TenantDashboardDTO
    {
        public string ApartmentNumber { get; set; } = string.Empty;

        public string UnitNumber { get; set; } = string.Empty;

        public int TotalTickets { get; set; }

        public int OpenTickets { get; set; }

        public int ResolvedTickets { get; set; }
    }
}
