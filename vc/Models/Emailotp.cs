using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Emailotp
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public string Otpcode { get; set; } = null!;

    public string Purpose { get; set; } = null!;

    public bool? Isused { get; set; }

    public DateTime Expiresat { get; set; }

    public DateTime? Createdat { get; set; }

    public virtual User? User { get; set; }
}
