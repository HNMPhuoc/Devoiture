using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Yeucauthuexe
{
    public int MaYc { get; set; }

    public int MaHt { get; set; }

    public DateTime Ngaynhanxe { get; set; }

    public DateTime Ngaytraxe { get; set; }

    public int Songaythue { get; set; }

    public double TienCoc { get; set; }

    public double Tongtienthue { get; set; }

    public string? Diadiemnhanxe { get; set; }

    public bool? Chapnhan { get; set; }

    public double Baohiemxe { get; set; }

    public string Email { get; set; } = null!;

    public string Biensoxe { get; set; } = null!;

    public virtual Xe BiensoxeNavigation { get; set; } = null!;

    public virtual Taikhoan EmailNavigation { get; set; } = null!;

    public virtual ICollection<Hoadon> Hoadons { get; set; } = new List<Hoadon>();

    public virtual Hinhthucthanhtoan MaHtNavigation { get; set; } = null!;
}
