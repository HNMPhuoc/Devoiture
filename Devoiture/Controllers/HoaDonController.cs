using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;

namespace Devoiture.Controllers
{
    [Authorize]
    public class HoaDonController : Controller
    {
        private readonly Devoiture1Context _context;
        public HoaDonController(Devoiture1Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Dangnhap", "Khachhang");
            }

            var hoadonThueList = (from hd in _context.HoadonThuexes
                                  join xe in _context.Xes on hd.Biensoxe equals xe.Biensoxe
                                  join nguoithue in _context.Taikhoans on hd.Email equals nguoithue.Email
                                  where hd.Email == userEmail && hd.Sotiencantra == 0
                                  select new HoaDonThue
                                  {
                                      email = hd.Email,
                                      MaHd = hd.MaHd,
                                      NgaylapHd = hd.NgaylapHd,
                                      TenKhachHang = nguoithue.HoTen,
                                      TongTien = hd.TongTienThue
                                  }).ToList();
            var hoadonChoThueList = (from hd in _context.HoaDonChoThueXes
                                     join xe in _context.Xes on hd.Biensx equals xe.Biensoxe
                                     join mx in _context.MauXes on xe.MaMx equals mx.MaMx
                                     join tien in _context.Yeucauthuexes on hd.MaYc equals tien.MaYc
                                     join nguoithue in _context.Taikhoans on hd.Email equals nguoithue.Email
                                     where hd.Email == userEmail && hd.Tongtiennhanduoc == tien.Tongtienthue
                                     select new HoaDonChoThue
                                     {
                                         email = hd.Email,
                                         MaHd = hd.MaHdct,
                                         NgaylapHd = hd.NglapHd,
                                         TenKhachHang = nguoithue.HoTen,
                                         TongTien = hd.Tongtiennhanduoc
                                     }).ToList();

            var viewModel = new HoaDon_VM
            {
                Hoadonthuexes = hoadonThueList,
                HoadonchoThuexes = hoadonChoThueList
            };

            return View(viewModel);
        }
        public IActionResult ChitietHDT(string mahd)
        {
            var chitiethd = (from hd in _context.HoadonThuexes
                             where hd.MaHd == mahd
                select new ChitietHDTX 
                {
                    MaHd = mahd,
                    MaYc = hd.MaYc,
                    Email = hd.Email,
                    Biensoxe = hd.Biensoxe,
                    NgaylapHd = hd.NgaylapHd,
                    HoTen = hd.HoTen,
                    Baohiemthuexe = hd.Baohiemthuexe,
                    Sdt = hd.Sdt,
                    Tiendatcoc = hd.Tiendatcoc,
                    TongTienThue = hd.TongTienThue,
                    Sotiencantra = hd.Sotiencantra
                }).FirstOrDefault();
            return View(chitiethd);
        }
        public IActionResult ChitietHDCT(string mahd)
        {
            var chitiethd = (from hd in _context.HoaDonChoThueXes
                             where hd.MaHdct == mahd
                             select new ChitietHDCT
                             {
                                 MaHdct = mahd,
                                 MaYc = hd.MaYc,
                                 Email = hd.Email,
                                 Biensx = hd.Biensx,
                                 NglapHd = hd.NglapHd,
                                 Hoten = hd.Hoten,
                                 Sdt = hd.Sdt,
                                 Tongtiennhanduoc = hd.Tongtiennhanduoc
                             }).FirstOrDefault();
            return View(chitiethd);
        }
    }
}
