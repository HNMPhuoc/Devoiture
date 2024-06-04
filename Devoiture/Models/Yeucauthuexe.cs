using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Yeucauthuexe
{
    public int MaYc { get; set; }

    public string Chuxe { get; set; } = null!;

    public string Nguoithue { get; set; } = null!;

    public string Biensoxe { get; set; } = null!;

    public int Maht { get; set; }

    public int Matt { get; set; }

    public DateTime Ngayyeucau { get; set; }

    public string Diadiemnhanxe { get; set; } = null!;

    public DateTime Ngaynhanxe { get; set; }

    public DateTime Ngaytraxe { get; set; }

    public int Songaythue { get; set; }

    public double Baohiemthuexe { get; set; }

    public double Dongiathue { get; set; }

    public double Tongtienthue { get; set; }

    public virtual Xe BiensoxeNavigation { get; set; } = null!;

    public virtual ICollection<HoaDonChoThueXe> HoaDonChoThueXes { get; set; } = new List<HoaDonChoThueXe>();

    public virtual ICollection<HoadonThuexe> HoadonThuexes { get; set; } = new List<HoadonThuexe>();

    public virtual Hinhthucthanhtoan MahtNavigation { get; set; } = null!;

    public virtual TrangthaiThuexe MattNavigation { get; set; } = null!;

    public virtual Taikhoan NguoithueNavigation { get; set; } = null!;
}
