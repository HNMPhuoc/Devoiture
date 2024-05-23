using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class HopDongThueXe
{
    public int MaHdong { get; set; }

    public int MaYc { get; set; }

    public double TienDatCoc { get; set; }

    public string? DieukhoantuChuXe { get; set; }

    public string Email { get; set; } = null!;

    public string Chuki { get; set; } = null!;

    public bool Chapnhan { get; set; }

    public int MaHt { get; set; }

    public virtual ICollection<HoadonThuexe> HoadonThuexes { get; set; } = new List<HoadonThuexe>();

    public virtual Hinhthucthanhtoan MaHtNavigation { get; set; } = null!;

    public virtual Yeucauthuexe MaYcNavigation { get; set; } = null!;
}
