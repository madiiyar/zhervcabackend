namespace vc.DTOs
{
    public class CreateOfferDto
    {
        public int TargetId { get; set; } // InvestorId or StartupId (based on role)
        public string Message { get; set; }
    }
    public class OfferResponseDto
    {
        public int Id { get; set; }

        public string RequestedBy { get; set; } // "Startup" or "Investor"
        public string Status { get; set; }

        public string Message { get; set; }

        public string CounterpartyName { get; set; } // Startup or Investor name
        public string CounterpartyType { get; set; } // "Startup" or "Investor"

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateOfferStatusDto
    {
        public string Status { get; set; } // "Accepted", "Rejected", "Cancelled"
    }
}
