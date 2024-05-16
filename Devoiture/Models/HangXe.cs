using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class HangXe
{
    public int MaHx { get; set; }

    public string TenHx { get; set; } = null!;

    public virtual ICollection<MauXe> MauXes { get; set; } = new List<MauXe>();
}
