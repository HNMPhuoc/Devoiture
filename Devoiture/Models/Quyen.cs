using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Quyen
{
    public int MaQuyen { get; set; }

    public string TenQuyen { get; set; } = null!;

    public bool? Create { get; set; }

    public bool? Read { get; set; }

    public bool? Update { get; set; }

    public bool? Delete { get; set; }

    public virtual ICollection<Taikhoan> Taikhoans { get; set; } = new List<Taikhoan>();
}
