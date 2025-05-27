using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Startup
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public string? Publicname { get; set; }

    public string? Website { get; set; }

    public string? Contactfullname { get; set; }

    public string? Publicemail { get; set; }

    public string? Phonenumber { get; set; }

    public string? Organizationname { get; set; }

    public string? Identificationnumber { get; set; }

    public int? Foundingyear { get; set; }

    public int? Countryid { get; set; }

    public int? Employeecount { get; set; }

    public string? Description { get; set; }

    public int? Developmentstageid { get; set; }

    public int? Investmentstageid { get; set; }

    public bool? Hassales { get; set; }

    public bool? Activelylookingforinvestment { get; set; }

    public decimal? Totalpreviousinvestment { get; set; }

    public string? Investorlist { get; set; }

    public string? Logopath { get; set; }

    public string? Presentationpath { get; set; }

    public int? Sourceinfoid { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Developmentstage? Developmentstage { get; set; }

    public virtual Investmentstage? Investmentstage { get; set; }

    public virtual Sourceinfo? Sourceinfo { get; set; }

    public virtual ICollection<Startupinvestoroffer> Startupinvestoroffers { get; set; } = new List<Startupinvestoroffer>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Businessmodel> Businessmodels { get; set; } = new List<Businessmodel>();

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

    public virtual ICollection<Industry> Industries { get; set; } = new List<Industry>();

    public virtual ICollection<Salesmodel> Salesmodels { get; set; } = new List<Salesmodel>();

    public virtual ICollection<Technology> Technologies { get; set; } = new List<Technology>();
}
