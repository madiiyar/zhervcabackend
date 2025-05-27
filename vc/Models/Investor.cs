using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Investor
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public string Investortype { get; set; } = null!;

    public string? Fullname { get; set; }

    public string? Contactfullname { get; set; }

    public string? Publicemail { get; set; }

    public string? Phonenumber { get; set; }

    public int? Countryid { get; set; }

    public string? Website { get; set; }

    public string? Organizationname { get; set; }

    public string? Identificationnumber { get; set; }

    public string? Description { get; set; }

    public string? Profilephotopath { get; set; }

    public string? Logopath { get; set; }

    public int? Sourceinfoid { get; set; }

    public decimal? Investmentamount { get; set; }

    public bool? Hasstartuppilotexperience { get; set; }

    public bool? Investsinstartups { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Sourceinfo? Sourceinfo { get; set; }

    public virtual ICollection<Startupinvestoroffer> Startupinvestoroffers { get; set; } = new List<Startupinvestoroffer>();

    public virtual User? User { get; set; }

    public virtual ICollection<Developmentstage> Developmentstages { get; set; } = new List<Developmentstage>();

    public virtual ICollection<Industry> Industries { get; set; } = new List<Industry>();

    public virtual ICollection<Innovationmethod> Innovationmethods { get; set; } = new List<Innovationmethod>();

    public virtual ICollection<Technology> Technologies { get; set; } = new List<Technology>();
}
