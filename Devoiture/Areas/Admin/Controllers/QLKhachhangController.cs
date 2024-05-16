using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devoiture.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class QLKhachhangController : Controller
    {
        private readonly Devoiture1Context _context;
        public QLKhachhangController(Devoiture1Context context)
        {
            _context = context;
        }

        public IActionResult DanhsachKH()
        {
            var dskh = _context.Taikhoans
                .Where(nv => nv.IdQuyen == 3)
                .Select(nv => new DanhsachKH_VM
                {
                    Email = nv.Email,
                    IdQuyen = nv.IdQuyen,
                    Username = nv.Username,
                    HoTen = nv.HoTen,
                    Matkhau = nv.Matkhau,
                    Sdt = nv.Sdt,
                    Ngsinh = nv.Ngsinh,
                    HinhDaiDien = nv.HinhDaiDien,
                    Gioitinh = nv.Gioitinh,
                    Online = nv.Online,
                    Lock = nv.Lock,
                    Soxe = _context.Xes.Count(x => x.Email == nv.Email)
                })
                .OrderBy(ten => ten.HoTen)
                .ToList();
            return View("~/Areas/Admin/Views/QLKhachhang/DanhsachKH.cshtml",dskh);
        }
    }
}
