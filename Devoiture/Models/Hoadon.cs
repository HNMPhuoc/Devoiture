using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Hoadon
{
    public int MaHd { get; set; }

    public DateTime NgaylapHd { get; set; }

    public double Tiendatcoc { get; set; }

    public bool TrangthaiThanhtoan { get; set; }

    public double TongTien { get; set; }

    public int MaHt { get; set; }

    public int MaYc { get; set; }

    public string Email { get; set; } = null!;

    public string Biensoxe { get; set; } = null!;

    public virtual Yeucauthuexe Yeucauthuexe { get; set; } = null!;
}
