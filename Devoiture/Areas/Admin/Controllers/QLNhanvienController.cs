using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devoiture.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class QLNhanvienController : Controller
    {
        private readonly Devoiture1Context _context;
        public QLNhanvienController(Devoiture1Context context)
        {
            _context = context;
        }
        [AdminAuthorize]
        public IActionResult DanhsachNV()
        {            
            var dsnv = _context.Taikhoans
                .Where(kh => kh.IdQuyen == 2)
                .Select(kh => new DanhsachKH_VM
                {
                    Email = kh.Email,
                    IdQuyen = kh.IdQuyen,
                    Username = kh.Username,
                    HoTen = kh.HoTen,
                    Matkhau = kh.Matkhau,
                    Sdt = kh.Sdt,
                    Ngsinh = kh.Ngsinh,
                    HinhDaiDien = kh.HinhDaiDien,
                    Gioitinh = kh.Gioitinh,
                    Online = kh.Online,
                    Lock = kh.Lock,
                    Soxe = _context.Xes.Count(x => x.Email == kh.Email)
                })
                .OrderBy(kh => kh.HoTen)
                .ToList();
            return View("~/Areas/Admin/Views/QLNhanvien/DanhsachNV.cshtml", dsnv);
        }
    }
}
