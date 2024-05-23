using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Hinhthucthanhtoan
{
    public int MaHt { get; set; }

    public string TenHt { get; set; } = null!;

    public virtual ICollection<HopDongThueXe> HopDongThueXes { get; set; } = new List<HopDongThueXe>();
}
