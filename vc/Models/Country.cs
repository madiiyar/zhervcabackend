using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Investor> Investors { get; set; } = new List<Investor>();

    public virtual ICollection<Startup> Startups { get; set; } = new List<Startup>();

    public virtual ICollection<Startup> StartupsNavigation { get; set; } = new List<Startup>();
}
