using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class MauXe
{
    public int MaMx { get; set; }

    public string TenMx { get; set; } = null!;

    public int MaHx { get; set; }

    public virtual HangXe MaHxNavigation { get; set; } = null!;

    public virtual ICollection<Xe> Xes { get; set; } = new List<Xe>();
}
