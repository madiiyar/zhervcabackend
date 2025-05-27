using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Investmentstage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Startup> Startups { get; set; } = new List<Startup>();
}
