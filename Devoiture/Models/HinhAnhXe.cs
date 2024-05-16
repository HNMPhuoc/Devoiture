using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class HinhAnhXe
{
    public int MaAnh { get; set; }

    public string Hinh { get; set; } = null!;

    public string Biensoxe { get; set; } = null!;

    public virtual Xe BiensoxeNavigation { get; set; } = null!;
}
