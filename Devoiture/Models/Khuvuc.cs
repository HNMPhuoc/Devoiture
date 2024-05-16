using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Khuvuc
{
    public int MaKv { get; set; }

    public string TenKv { get; set; } = null!;

    public virtual ICollection<Xe> Xes { get; set; } = new List<Xe>();
}
