using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Technology
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Investor> Investors { get; set; } = new List<Investor>();

    public virtual ICollection<Startup> Startups { get; set; } = new List<Startup>();
}
