using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class LoaiXe
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public virtual ICollection<Xe> Xes { get; set; } = new List<Xe>();
}
