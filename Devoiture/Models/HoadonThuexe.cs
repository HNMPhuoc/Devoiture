using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class HoadonThuexe
{
    public int MaHd { get; set; }

    public int? MaHdong { get; set; }

    public DateTime NgaylapHd { get; set; }

    public double Tiendatcoc { get; set; }

    public double TongTien { get; set; }

    public string Email { get; set; } = null!;

    public string Biensoxe { get; set; } = null!;

    public int Matrangthai { get; set; }

    public virtual Xe BiensoxeNavigation { get; set; } = null!;

    public virtual Taikhoan EmailNavigation { get; set; } = null!;

    public virtual HopDongThueXe? MaHdongNavigation { get; set; }

    public virtual Trangthaithanhtoan MatrangthaiNavigation { get; set; } = null!;
}
