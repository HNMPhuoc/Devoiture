using Azure;
using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X9;

namespace Devoiture.Areas.Admin.Controllers
{
    [Authorize]
    public class TienCocController : Controller
    {
        private readonly Devoiture1Context _context;
        public TienCocController(Devoiture1Context context)
        {
            _context = context;
        }
        public IActionResult Danhsachxacnhantiencoc()
        {
            var thuexes = (from yc in _context.Yeucauthuexes
                           join xe in _context.Xes on yc.Biensoxe equals xe.Biensoxe
                           join nguoithue in _context.Taikhoans on yc.Nguoithue equals nguoithue.Email
                           join trangthai in _context.TrangthaiThuexes on yc.Matt equals trangthai.Matt
                           join ht in _context.Hinhthucthanhtoans on yc.Maht equals ht.MaHt
                           where yc.Matt == 2
                           select new Xacnhantienmat_VM
                           {
                               MaYc = yc.MaYc,
                               MauXe = xe.MaMxNavigation.TenMx,
                               HinhXe = xe.Hinhanh,
                               HoTenNguoiThue = nguoithue.HoTen,
                               NgayNhanXe = yc.Ngaynhanxe,
                               NgayTraXe = yc.Ngaytraxe,
                               TrangThaiThue = trangthai.Tentrangthai,
                               Maht = ht.MaHt
                           }).ToList();
            return View("~/Areas/Admin/Views/TienCoc/Danhsachxacnhantiencoc.cshtml", thuexes);
        }
        public IActionResult ChitietycThanhtoanTienCoc(int mayc)
        {
            var chitietyc = (from yc in _context.Yeucauthuexes
                             join xe in _context.Xes on yc.Biensoxe equals xe.Biensoxe
                             join nguoithue in _context.Taikhoans on yc.Nguoithue equals nguoithue.Email
                             join trangthai in _context.TrangthaiThuexes on yc.Matt equals trangthai.Matt
                             join ht in _context.Hinhthucthanhtoans on yc.Maht equals ht.MaHt
                             where yc.MaYc == mayc
                             select new ChitietycThanhtoan_VM
                             {
                                 MaYc = yc.MaYc,
                                 HinhXe = xe.Hinhanh,
                                 TenMauXe = xe.MaMxNavigation.TenMx,
                                 Ngayyeucau = yc.Ngayyeucau.ToString("dd/MM/yyyy HH:mm"),
                                 ThoiGianThue = $"{yc.Ngaynhanxe.ToString("dd/MM/yyyy")} - {yc.Ngaytraxe.ToString("dd/MM/yyyy")}",
                                 Chuxe = nguoithue.Email,
                                 DiaDiemNhanXe = yc.Diadiemnhanxe,
                                 BienSoXe = yc.Biensoxe,
                                 Trangthaiyc = trangthai.Tentrangthai,
                                 TongTienThue = yc.Tongtienthue,
                                 Sotiencantra = yc.Sotiencantra,
                                 Baohiemthuexe = yc.Baohiemthuexe,
                                 HoTen = nguoithue.HoTen,
                                 Sdt = nguoithue.Sdt,
                                 SoGplxB2 = nguoithue.SoGplxB2,
                                 HinhGplxb2 = nguoithue.HinhGplxb2,
                                 SoCccd = nguoithue.SoCccd,
                                 HinhCccd = nguoithue.HinhCccd,
                                 httt = ht.TenHt
                             }).FirstOrDefault();
            if (chitietyc == null)
            {
                return NotFound();
            }
            return View("~/Areas/Admin/Views/TienCoc/ChitietycThanhtoanTiencoc.cshtml", chitietyc);
        }
        [HttpPost]
        public IActionResult XacNhanThanhToan(int mayc)
        {
            var yc = _context.Yeucauthuexes.FirstOrDefault(x => x.MaYc == mayc);
            if (yc != null)
            {
                yc.Sotiencantra = Math.Round(yc.Sotiencantra - (30 * yc.Tongtienthue) / 100, 0);
                yc.Matt = 4;
                _context.Yeucauthuexes.Update(yc);
                var khachhang = _context.Taikhoans.FirstOrDefault(t => t.Email == yc.Nguoithue);
                var chuxe = _context.Taikhoans.FirstOrDefault(t => t.Email == yc.Chuxe);

                if (khachhang != null && chuxe != null)
                {
                    var id = Guid.NewGuid().ToString();
                    var hoadonThuexe = new HoadonThuexe
                    {
                        MaHd = id,
                        MaYc = yc.MaYc,
                        Email = yc.Nguoithue,
                        Biensoxe = yc.Biensoxe,
                        NgaylapHd = DateTime.Now,
                        HoTen = khachhang.HoTen,
                        Sdt = khachhang.Sdt,
                        Tiendatcoc = Math.Round(yc.Tongtienthue * 0.3, 0),
                        Baohiemthuexe = Math.Round(yc.Baohiemthuexe, 0),
                        TongTienThue = Math.Round(yc.Tongtienthue, 0),
                        Sotiencantra = Math.Round(yc.Tongtienthue, 0) - Math.Round(yc.Tongtienthue * 0.3, 0),
                    };
                    var hoadonchothuexe = new HoaDonChoThueXe
                    {
                        MaHdct = id,
                        MaYc = yc.MaYc,
                        Email = yc.Chuxe,
                        Biensx = yc.Biensoxe,
                        NglapHd = DateTime.Now,
                        Hoten = chuxe.HoTen,
                        Sdt = chuxe.Sdt,
                        Tongtiennhanduoc = Math.Round(yc.Tongtienthue * 0.7, 0)
                    };
                    _context.HoadonThuexes.Add(hoadonThuexe);
                    _context.HoaDonChoThueXes.Add(hoadonchothuexe);
                    _context.SaveChanges();
                    TempData["success"] = "Thanh toán tiền cọc thành công";
                }                
            }
            return RedirectToAction("Danhsachxacnhantiencoc", "TienCoc", new { area = "Admin" });
        }
    }
}
