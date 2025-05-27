using System;
using System.Collections.Generic;

namespace vc.Models;

public partial class Supportmessage
{
    public int Id { get; set; }

    public string Fullname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime? Createdat { get; set; }
}
