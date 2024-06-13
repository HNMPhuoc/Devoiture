using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class HoadonThuexe
{
    public string MaHd { get; set; } = null!;

    public int MaYc { get; set; }

    public string Email { get; set; } = null!;

    public string Biensoxe { get; set; } = null!;

    public DateTime NgaylapHd { get; set; }

    public string HoTen { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public double Tiendatcoc { get; set; }

    public double Baohiemthuexe { get; set; }

    public double TongTienThue { get; set; }

    public double Sotiencantra { get; set; }

    public virtual Yeucauthuexe MaYcNavigation { get; set; } = null!;
}
