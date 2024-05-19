using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Quyen
{
    public int MaQuyen { get; set; }

    public string TenQuyen { get; set; } = null!;

    public virtual ICollection<Taikhoan> Taikhoans { get; set; } = new List<Taikhoan>();

    public virtual ICollection<Website> Websites { get; set; } = new List<Website>();
}
