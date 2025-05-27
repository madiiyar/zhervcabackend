using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Innovationmethod
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Investor> Investors { get; set; } = new List<Investor>();
}
