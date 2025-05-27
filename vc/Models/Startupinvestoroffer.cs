using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Startupinvestoroffer
{
    public int Id { get; set; }

    public int? Startupid { get; set; }

    public int? Investorid { get; set; }

    public string Requestedby { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Message { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Investor? Investor { get; set; }

    public virtual Startup? Startup { get; set; }
}
