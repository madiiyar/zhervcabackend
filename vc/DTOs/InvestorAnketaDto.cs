using System.ComponentModel.DataAnnotations;

namespace vc.DTOs
{
    // For list view
    public class InvestorListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string OrganizationName { get; set; }
        public string InvestorType { get; set; }
        public string PublicEmail { get; set; }
        public string CountryName { get; set; }
        public string ProfilePhotoPath { get; set; }
    }

    // For detailed view
    public class InvestorDetailDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string ContactFullName { get; set; }
        public string PublicEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryName { get; set; }
        public string Website { get; set; }
        public string OrganizationName { get; set; }
        public string IdentificationNumber { get; set; }
        public string Description { get; set; }
        public decimal? InvestmentAmount { get; set; }
        public bool? HasStartupPilotExperience { get; set; }
        public bool? InvestsInStartups { get; set; }
        public string ProfilePhotoPath { get; set; }
        public string LogoPath { get; set; }
        public List<string> Industries { get; set; }
        public List<string> Technologies { get; set; }
        public List<string> InnovationMethods { get; set; }
        public List<string> DevelopmentStages { get; set; }
    }

        public class InvestorAnketaDto
        {
            // Basic Info
            [Required]
            public string FullName { get; set; }

            [Required]
            public string ContactFullName { get; set; }

            [Required]
            [EmailAddress]
            public string PublicEmail { get; set; }

            public string PhoneNumber { get; set; }

            [Required]
            public int CountryId { get; set; }

            public string Website { get; set; }

            public string OrganizationName { get; set; }
            public string InvestorType { get; set; }


            public string IdentificationNumber { get; set; }

            public string Description { get; set; }

            // Investment Info
            public decimal InvestmentAmount { get; set; }

            public bool HasStartupPilotExperience { get; set; }

            public bool InvestsInStartups { get; set; }

            public int? SourceInfoId { get; set; }

            // Optional file upload
            public IFormFile? ProfilePhoto { get; set; }
            public IFormFile? Logo { get; set; }


        // Multi-select dropdowns (IDs from UI)
        [Required]
            public List<int> IndustryIds { get; set; } = new();

            [Required]
            public List<int> TechnologyIds { get; set; } = new();

            [Required]
            public List<int> InnovationMethodIds { get; set; } = new();

            [Required]
            public List<int> DevelopmentStageIds { get; set; } = new();
        }
    


}
