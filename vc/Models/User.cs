using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class User
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phonenumber { get; set; }

    public string Passwordhash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool? Isemailconfirmed { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Emailotp> Emailotps { get; set; } = new List<Emailotp>();

    public virtual ICollection<Investor> Investors { get; set; } = new List<Investor>();

    public virtual ICollection<Startup> Startups { get; set; } = new List<Startup>();
}
