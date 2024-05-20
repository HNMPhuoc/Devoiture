using AutoMapper;
using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Devoiture.Areas.Admin.Controllers
{
    [Authorize]
    public class QLKhachhangController : Controller
    {
        private readonly Devoiture1Context _context;
        private readonly IMapper _mapper;
        public QLKhachhangController(Devoiture1Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                    Soxe = _context.Xes.Count(x => x.Email == nv.Email && x.TrangthaiDuyet == true)
                })
                .OrderBy(ten => ten.HoTen)
                .ToList();
            return View("~/Areas/Admin/Views/QLKhachhang/DanhsachKH.cshtml",dskh);
        }
        [HttpPost]
        public IActionResult LockAccount(string email, bool isLocked)
        {
            var khachhang = _context.Taikhoans.SingleOrDefault(t => t.Email == email);
            if (khachhang != null)
            {
                khachhang.Lock = isLocked;
                _context.SaveChanges();
            }
            return RedirectToAction("DanhsachKH");
        }
        [HttpGet]
        public IActionResult ChiTietKhachHang(string email)
        {
            var khachHang = _context.Taikhoans
                .Include(kh => kh.Xes)
                .FirstOrDefault(kh => kh.Email == email);

            if (khachHang == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<ChitietKH_VM>(khachHang);
            return View("~/Areas/Admin/Views/QLKhachhang/ChiTietKhachHang.cshtml", model);
        }
    }
}
