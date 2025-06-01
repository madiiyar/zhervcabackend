using System.ComponentModel.DataAnnotations;

namespace vc.DTOs
{
    public class StartupAnketaDto
    {
        // Basic Info
        [Required] public string PublicName { get; set; }
        [Required] public string ContactFullName { get; set; }
        [Required][EmailAddress] public string PublicEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string OrganizationName { get; set; }
        public string IdentificationNumber { get; set; }

        // Profile Info
        public int FoundingYear { get; set; }
        [Required] public int CountryId { get; set; }
        public int EmployeeCount { get; set; }
        public string Description { get; set; }

        // Dropdowns
        [Required] public int DevelopmentStageId { get; set; }
        [Required] public int InvestmentStageId { get; set; }

        public bool HasSales { get; set; }
        public bool ActivelyLookingForInvestment { get; set; }
        public decimal TotalPreviousInvestment { get; set; }
        public string InvestorList { get; set; }
        public int? SourceInfoId { get; set; }

        // Files
        public IFormFile? Logo { get; set; }
        public IFormFile? Presentation { get; set; }

        // Multi-select dropdowns
        [Required] public List<int> IndustryIds { get; set; } = new();
        [Required] public List<int> TechnologyIds { get; set; } = new();
        [Required] public List<int> BusinessModelIds { get; set; } = new();
        [Required] public List<int> SalesModelIds { get; set; } = new();
        [Required] public List<int> TargetCountryIds { get; set; } = new();
    }

    public class StartupListDto
    {
        public int Id { get; set; }
        public string PublicName { get; set; }
        public string OrganizationName { get; set; }
        public string CountryName { get; set; }
        public string DevelopmentStage { get; set; }
        public string LogoPath { get; set; }
    }

    public class StartupDetailDto
    {
        // Basic Info
        public int Id { get; set; }
        public string PublicName { get; set; }
        public string ContactFullName { get; set; }
        public string PublicEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string OrganizationName { get; set; }
        public string IdentificationNumber { get; set; }
        public int FoundingYear { get; set; }
        public string CountryName { get; set; }
        public int EmployeeCount { get; set; }
        public string Description { get; set; }

        // Dropdowns
        public string DevelopmentStage { get; set; }
        public string InvestmentStage { get; set; }

        public bool HasSales { get; set; }
        public bool ActivelyLookingForInvestment { get; set; }
        public decimal TotalPreviousInvestment { get; set; }
        public string InvestorList { get; set; }
        public string LogoPath { get; set; }
        public string PresentationPath { get; set; }

        // Tags
        public List<string> Industries { get; set; }
        public List<string> Technologies { get; set; }
        public List<string> BusinessModels { get; set; }
        public List<string> SalesModels { get; set; }
        public List<string> TargetCountries { get; set; }
    }

    public class UpdateStartupStatusDto
    {
        public string Status { get; set; } = null!; // "Accepted" or "Rejected"
    }
}
