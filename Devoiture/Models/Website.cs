using System;
using System.Collections.Generic;

namespace Devoiture.Models;

public partial class Website
{
    public int MaWebsite { get; set; }

    public int MaQuyen { get; set; }

    public string TenWebsite { get; set; } = null!;

    public bool? Quyentruycap { get; set; }

    public bool? Create { get; set; }

    public bool? Read { get; set; }

    public bool? Update { get; set; }

    public bool? Delete { get; set; }

    public virtual Quyen MaQuyenNavigation { get; set; } = null!;
}
