namespace vc.DTOs
{
    public class CreateOfferDto
    {
        public int TargetId { get; set; } // ID of the Investor or Startup depending on who sends
        public string? Message { get; set; }
    }

    public class OfferResponseDto
    {
        public int Id { get; set; }

        public string StartupName { get; set; }
        public string InvestorName { get; set; }

        public string RequestedBy { get; set; } // "Startup" or "Investor"
        public string Status { get; set; }      // Pending, Accepted, etc.

        public string? Message { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateOfferStatusDto
    {
        public string Status { get; set; } = null!; // "Accepted", "Rejected", or "Cancelled"
    }
}
