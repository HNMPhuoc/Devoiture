using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Yeucauthuexe
{
    public int MaYc { get; set; }

    public DateTime Ngaynhanxe { get; set; }

    public DateTime Ngaytraxe { get; set; }

    public int Songaythue { get; set; }

    public double Tongtienthue { get; set; }

    public string? Diadiemnhanxe { get; set; }

    public bool? Chapnhan { get; set; }

    public double Baohiemthuexe { get; set; }

    public string Email { get; set; } = null!;

    public string Biensoxe { get; set; } = null!;

    public virtual Xe BiensoxeNavigation { get; set; } = null!;

    public virtual Taikhoan EmailNavigation { get; set; } = null!;

    public virtual ICollection<HopDongThueXe> HopDongThueXes { get; set; } = new List<HopDongThueXe>();

    public virtual ICollection<LichChoThue> LichChoThues { get; set; } = new List<LichChoThue>();
}
