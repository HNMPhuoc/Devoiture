using Devoiture.Helpers;
using Devoiture.Models;
using Devoiture.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using System.Security.Policy;
using System.Security.Claims;

namespace Devoiture.Controllers
{
    [Authorize]
    public class ThuexetulaiController : Controller
    {
        private readonly Devoiture1Context _context;
        private readonly EmailSender _emailsender;

        public ThuexetulaiController(Devoiture1Context context, EmailSender emailsender)
        {
            _context = context;
            _emailsender = emailsender;
        }
        public IActionResult Thuexe(string biensoxe)
        {
            if (string.IsNullOrEmpty(biensoxe))
            {
                return BadRequest("Biensoxe is required");
            }
            var xe = _context.Xes
                .Include(x => x.HinhAnhXes)
                .Include(x => x.MaMxNavigation)
                .FirstOrDefault(x => x.Biensoxe == biensoxe);
            if (xe == null)
            {
                return NotFound("Xe not found");
            }
            var hinhAnhList = xe.HinhAnhXes?.OrderByDescending(p => p.MaAnh).Select(p => p.Hinh).Take(4).ToList() ?? new List<string>();
            var viewModel = new Thuexe_VM
            {
                CarModel = xe.MaMxNavigation.TenMx,
                Giathue = xe.Giathue,
                NamSx = xe.NamSx,
                Soghe = xe.Soghe,
                Muctieuthunhienlieu = xe.Muctieuthunhienlieu,
                Hinhanh = xe.Hinhanh,
                Diachixe = xe.Diachixe,
                Dieukhoanthuexe = xe.Dieukhoanthuexe,
                MotaDacDiemChucNang = xe.MotaDacDiemChucNang,
                Loainhienlieu = xe.Loainhienlieu,
                HinhAnhList = hinhAnhList,
                HinhThucThanhToanList = _context.Hinhthucthanhtoans.ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Thuexe(Thuexe_VM model, string biensoxe)
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY).Value;                           
            var xe = _context.Xes
                .Include(x => x.HinhAnhXes)
                .Include(x => x.MaMxNavigation)
                .FirstOrDefault(x => x.Biensoxe == model.Biensoxe);
            var hinhAnhList = xe.HinhAnhXes?.OrderByDescending(p => p.MaAnh).Select(p => p.Hinh).Take(4).ToList() ?? new List<string>();
            var viewModel = new Thuexe_VM
            {
                CarModel = xe.MaMxNavigation.TenMx,
                Giathue = xe.Giathue,
                NamSx = xe.NamSx,
                Soghe = xe.Soghe,
                Muctieuthunhienlieu = xe.Muctieuthunhienlieu,
                Hinhanh = xe.Hinhanh,
                Diachixe = xe.Diachixe,
                Dieukhoanthuexe = xe.Dieukhoanthuexe,
                MotaDacDiemChucNang = xe.MotaDacDiemChucNang,
                Loainhienlieu = xe.Loainhienlieu,
                HinhAnhList = hinhAnhList,
                HinhThucThanhToanList = _context.Hinhthucthanhtoans.ToList()
            };
            if (model == null)
            {
                return View(viewModel);
            }
            if (xe.Email == kh)
            {
                TempData["error"] = "Bạn không thể thuê xe của chính mình";
                return View(viewModel);
            }
            var ngaythue = _context.Yeucauthuexes
                .Where(y => y.Biensoxe == biensoxe && (y.Matt == 2 || y.Matt == 4 || y.Matt == 5))
                .Where(y => (model.Ngaynhanxe >= y.Ngaynhanxe && model.Ngaynhanxe <= y.Ngaytraxe) ||
                            (model.Ngaytraxe >= y.Ngaynhanxe && model.Ngaytraxe <= y.Ngaytraxe))
                .ToList();

            if (ngaythue.Any())
            {
                TempData["error"] = "Xe đã được thuê trong khoảng thời gian này.";
                return View(viewModel);
            }
            var owner = _context.Taikhoans.SingleOrDefault(u => u.Email == xe.Email);
            if (owner != null && owner.Lock == true)
            {
                TempData["error"] = "Chủ xe hiện đang không thể cho thuê xe";
                return View(viewModel);
            }
            var ngThue = _context.Taikhoans.SingleOrDefault(u => u.Email == kh);
            if (ngThue != null && ngThue.HinhCccd == null && ngThue.SoCccd == null && ngThue.HinhGplxb2 == null && ngThue.SoGplxB2 == null)
            {
                TempData["error"] = "Vui lòng cung cấp thông tin CCCD và GPLX trong profile của bạn.";
                return View(viewModel);
            }
            var yeucauthuexe = new Yeucauthuexe
            {
                Biensoxe = xe.Biensoxe,
                Chuxe = xe.Email,
                Nguoithue = kh,
                Ngayyeucau = DateTime.Now,
                Maht = model.Maht,
                Matt = 1,
                Diadiemnhanxe = model.diachinhanxe,
                Ngaynhanxe = model.Ngaynhanxe,
                Ngaytraxe = model.Ngaytraxe,
                Songaythue = model.Songaythue,
                Baohiemthuexe = model.Baohiemthuexe,
                Dongiathue = model.Dongiathue,
                Tongtienthue = model.Tongtienthue,
                Sotiencantra = model.Tongtienthue
            };
            _context.Yeucauthuexes.Add(yeucauthuexe);
            _context.SaveChanges();
            TempData["success"] = "Đặt yêu cầu thuê xe thành công! Vui lòng chờ xác nhận của chủ xe.";
            var expiry = DateTime.Now.AddMinutes(1);
            var maYc = yeucauthuexe.MaYc;
            var token = TokenHelper.GenerateToken(expiry, maYc);
            var url = Url.Action("ValidateEmailLink", "Thuexetulai", new { token }, Request.Scheme);
            var ownerHtmlContent = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MessageEmail/OwnerNotification.html"));
            ownerHtmlContent = ownerHtmlContent.Replace("{{Link}}", url);
            _emailsender.SendMail(xe.Email, "Yêu cầu đặt xe Devoiture", ownerHtmlContent);
            return RedirectToAction("Index","Home");
        }
        public IActionResult ValidateEmailLink(string token)
        {
            var expiry = TokenHelper.ValidateToken(token);
            if (expiry == null || expiry < DateTime.Now)
            {
                return RedirectToAction("Index", "Home");
            }
            var currentUserEmail = HttpContext.Session.GetString(MySettings.ACCOUNT_KEY);
            int? maYc = TokenHelper.GetMaYcFromToken(token);
            var yeuCau = _context.Yeucauthuexes.FirstOrDefault(y => y.MaYc == maYc);
            if (yeuCau == null)
            {
                return RedirectToAction("Error", "Home");
            }
            if (currentUserEmail == null)
            {
                HttpContext.Session.SetInt32("MaYcFromToken", maYc.Value);
                return RedirectToAction("Dangnhap", "Khachhang", new { returnUrl = Url.Action("Chitietyc", new { mayc = maYc }) });
            }
            if (currentUserEmail == yeuCau.Chuxe)
            {
                return RedirectToAction("Chitietyc", new { mayc = maYc });
            }
            return RedirectToAction("Dangnhap", "Khachhang", new { returnUrl = Url.Action("Chitietyc", new { mayc = maYc }) });
        }
        public IActionResult Danhsachyeucauthuexe()
        {
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY)?.Value;
            if (string.IsNullOrEmpty(kh))
            {
                return BadRequest("Email tài khoản là bắt buộc");
            }
            var yeuCauThueXeList = _context.Yeucauthuexes
                .Where(y => y.Chuxe == kh.ToString() && y.Matt == 1)
                .Include(y => y.BiensoxeNavigation)
                .Include(y => y.BiensoxeNavigation.MaMxNavigation)
                .Include(y => y.NguoithueNavigation)
                .Select(y => new Danhsachyc_VM
                {
                    MaYc = y.MaYc,
                    NguoiThue = y.NguoithueNavigation.HoTen,
                    Ngayyeucau = y.Ngayyeucau.ToString("dd/MM/yyyy HH:mm"),
                    HinhXe = y.BiensoxeNavigation.Hinhanh,
                    TenMauXe = y.BiensoxeNavigation.MaMxNavigation.TenMx,
                    ThoiGianThue = y.Ngaynhanxe.ToString("dd/MM/yyyy") + " - " + y.Ngaytraxe.ToString("dd/MM/yyyy"),
                    Trangthaiyc = y.MattNavigation.Tentrangthai,
                })
                .ToList();
            return View(yeuCauThueXeList);
        }
        public IActionResult Chitietyc(int mayc)
        {
            var yeuCau = _context.Yeucauthuexes
                .Where(y => y.MaYc == mayc)
                .Include(y => y.BiensoxeNavigation)
                .Include(y => y.BiensoxeNavigation.MaMxNavigation)
                .Include(y => y.NguoithueNavigation)
                .FirstOrDefault();
            if (yeuCau == null)
            {
                return NotFound();
            }
            var httt = _context.Hinhthucthanhtoans
                    .Where(h => h.MaHt == yeuCau.Maht)
                    .Select(h => h.TenHt)
                    .FirstOrDefault();
            var model = new ChitietYC_VM
            {
                MaYc = yeuCau.MaYc,
                NguoiThue = yeuCau.Nguoithue != null ? yeuCau.Nguoithue : "",
                Ngayyeucau = yeuCau.Ngayyeucau.ToString("dd/MM/yyyy HH:mm"),
                HinhXe = yeuCau.BiensoxeNavigation != null ? yeuCau.BiensoxeNavigation.Hinhanh : "",
                BienSoXe = yeuCau.Biensoxe,
                TenMauXe = yeuCau.BiensoxeNavigation != null && yeuCau.BiensoxeNavigation.MaMxNavigation != null ? yeuCau.BiensoxeNavigation.MaMxNavigation.TenMx : "",
                ThoiGianThue = yeuCau.Ngaynhanxe.ToString("dd/MM/yyyy") + " - " + yeuCau.Ngaytraxe.ToString("dd/MM/yyyy"),
                DiaDiemNhanXe = yeuCau.Diadiemnhanxe,
                TongTienThue = yeuCau.Tongtienthue,
                Baohiemthuexe = yeuCau.Baohiemthuexe,
                HoTen = yeuCau.NguoithueNavigation != null ? yeuCau.NguoithueNavigation.HoTen : "",
                Sdt = yeuCau.NguoithueNavigation != null ? yeuCau.NguoithueNavigation.Sdt : "",
                SoGplxB2 = yeuCau.NguoithueNavigation != null ? yeuCau.NguoithueNavigation.SoGplxB2 : "",
                HinhGplxb2 = yeuCau.NguoithueNavigation != null ? yeuCau.NguoithueNavigation.HinhGplxb2 : "",
                SoCccd = yeuCau.NguoithueNavigation != null ? yeuCau.NguoithueNavigation.SoCccd : "",
                HinhCccd = yeuCau.NguoithueNavigation != null ? yeuCau.NguoithueNavigation.HinhCccd : "",
                httt = httt
            };
            return View(model);
        }
        public IActionResult Access(int mayc)
        {
            var yeuCau = _context.Yeucauthuexes.FirstOrDefault(y => y.MaYc == mayc);
            if (yeuCau == null)
            {
                return NotFound();
            }
            if(yeuCau.Ngaynhanxe < DateTime.Now)
            {
                TempData["error"] = "Yêu cầu thuê xe đã quá hạn. Quý khách không thể chấp nhận yêu cầu này";
                return RedirectToAction("Chitietyc", new {mayc = yeuCau.MaYc});
            }
            yeuCau.Matt = 2;
            _context.SaveChanges();
            var hoTenNguoiThue = _context.Taikhoans.FirstOrDefault(tk => tk.Email == yeuCau.Nguoithue)?.HoTen;
            string emailContent = "";
            if (yeuCau.Maht == 1)
            {
                emailContent = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MessageEmail/DepositPaymentNotification.html"));
            }
            else if (yeuCau.Maht == 2)
            {
                emailContent = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MessageEmail/MomoPaymentNotification.html"));
            }
            emailContent = emailContent.Replace("{{UserName}}", hoTenNguoiThue ?? "khách hàng");
            emailContent = emailContent.Replace("{{CarDetails}}", yeuCau.Biensoxe);
            _emailsender.SendMail(yeuCau.Nguoithue, "Yêu cầu đặt xe Devoiture", emailContent);
            var biensoxe = yeuCau.Biensoxe;
            var ngaynhanxe = yeuCau.Ngaynhanxe;
            var ngaytraxe = yeuCau.Ngaytraxe;
            var yeuCauKhac = _context.Yeucauthuexes
                .Where(y => y.Biensoxe == biensoxe && y.Matt == 1)
                .ToList();
            foreach (var yc in yeuCauKhac)
            {
                if ((yc.Ngaynhanxe >= ngaynhanxe && yc.Ngaynhanxe <= ngaytraxe) ||
                    (yc.Ngaytraxe >= ngaynhanxe && yc.Ngaytraxe <= ngaytraxe))
                {
                    yc.Matt = 3;
                    var rejectionHtmlContent = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MessageEmail/RejectionNotification.html"));
                    rejectionHtmlContent = rejectionHtmlContent.Replace("{{UserName}}", yc.Nguoithue);
                    rejectionHtmlContent = rejectionHtmlContent.Replace("{{CarDetails}}", yc.Biensoxe);

                    _emailsender.SendMail(yc.Nguoithue, "Yêu cầu thuê xe Devoiture bị từ chối", rejectionHtmlContent);
                }
            }
            _context.SaveChanges();
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY)?.Value;
            if (!string.IsNullOrEmpty(kh))
            {
                var soyc = _context.Yeucauthuexes
                    .Count(y => y.Chuxe == kh && y.Matt == 1);
                HttpContext.Session.SetInt32(MySettings.ACCOUNT_REQUEST, soyc);
            }
            return RedirectToAction("Danhsachyeucauthuexe");
        }
        public IActionResult Deny(int mayc)
        {
            var yeuCau = _context.Yeucauthuexes.FirstOrDefault(y => y.MaYc == mayc);
            if (yeuCau == null)
            {
                return NotFound();
            }
            yeuCau.Matt = 3;
            _context.SaveChanges();
            var kh = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySettings.ACCOUNT_KEY)?.Value;
            if (!string.IsNullOrEmpty(kh))
            {
                var soyc = _context.Yeucauthuexes
                    .Count(y => y.Chuxe == kh && y.Matt == 1);
                HttpContext.Session.SetInt32(MySettings.ACCOUNT_REQUEST, soyc);
            }
            var rejectionHtmlContent = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "MessageEmail/RejectionNotification.html"));
            rejectionHtmlContent = rejectionHtmlContent.Replace("{{UserName}}", yeuCau.Nguoithue);
            rejectionHtmlContent = rejectionHtmlContent.Replace("{{CarDetails}}", yeuCau.Biensoxe);
            _emailsender.SendMail(yeuCau.Nguoithue, "Yêu cầu thuê xe Devoiture bị từ chối", rejectionHtmlContent);
            return RedirectToAction("Danhsachyeucauthuexe");
        }
        public IActionResult DanhsachThuexeCanThanhToanTienCoc()
        {
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Dangnhap", "Khachhang");
            }
            var thuexes = (from yc in _context.Yeucauthuexes
                           join xe in _context.Xes on yc.Biensoxe equals xe.Biensoxe
                           join chuxe in _context.Taikhoans on yc.Chuxe equals chuxe.Email
                           join trangthai in _context.TrangthaiThuexes on yc.Matt equals trangthai.Matt
                           join ht in _context.Hinhthucthanhtoans on yc.Maht equals ht.MaHt
                           where (yc.Matt != 1 && yc.Matt != 3 && yc.Matt != 6) && yc.Nguoithue == userEmail
                           select new ThuexeThanhToanTienCoc_VM
                           {
                               MaYc = yc.MaYc,
                               MauXe = xe.MaMxNavigation.TenMx,
                               HinhXe = xe.Hinhanh,
                               HoTenChuXe = chuxe.HoTen,
                               NgayNhanXe = yc.Ngaynhanxe,
                               NgayTraXe = yc.Ngaytraxe,
                               TrangThaiThue = trangthai.Tentrangthai,
                               Maht = ht.MaHt
                           }).ToList();
            var chothuexes = (from yc in _context.Yeucauthuexes
                              join xe in _context.Xes on yc.Biensoxe equals xe.Biensoxe
                              join nguoithue in _context.Taikhoans on yc.Nguoithue equals nguoithue.Email
                              join trangthai in _context.TrangthaiThuexes on yc.Matt equals trangthai.Matt
                              join ht in _context.Hinhthucthanhtoans on yc.Maht equals ht.MaHt
                              where (yc.Matt != 1 && yc.Matt != 3 && yc.Matt != 6) && yc.Chuxe == userEmail
                              select new Chothuexe_VM
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
            var viewModel = new DanhsachThuexeViewModel
            {
                Thuexes = thuexes,
                ChoThuexes = chothuexes
            };
            return View(viewModel);
        }
        public IActionResult ChitietycThanhtoanTienCoc(int mayc)
        {
            var chitietyc = (from yc in _context.Yeucauthuexes
                             join xe in _context.Xes on yc.Biensoxe equals xe.Biensoxe
                             join chuxe in _context.Taikhoans on yc.Chuxe equals chuxe.Email
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
                                 Chuxe = chuxe.Email,
                                 DiaDiemNhanXe = yc.Diadiemnhanxe,
                                 BienSoXe = yc.Biensoxe,
                                 Trangthaiyc = trangthai.Tentrangthai,
                                 TongTienThue = yc.Tongtienthue,
                                 Sotiencantra = yc.Sotiencantra,
                                 Baohiemthuexe = yc.Baohiemthuexe,
                                 HoTen = chuxe.HoTen,
                                 Sdt = chuxe.Sdt,
                                 SoGplxB2 = chuxe.SoGplxB2,
                                 HinhGplxb2 = chuxe.HinhGplxb2,
                                 SoCccd = chuxe.SoCccd,
                                 HinhCccd = chuxe.HinhCccd,
                                 httt = ht.TenHt
                             }).FirstOrDefault();
            if (chitietyc == null)
            {
                return NotFound();
            }
            return View(chitietyc);
        }
        public IActionResult Chitietycdathuexe(int mayc)
        {
            var chitietyc = (from yc in _context.Yeucauthuexes
                             join xe in _context.Xes on yc.Biensoxe equals xe.Biensoxe
                             join chuxe in _context.Taikhoans on yc.Chuxe equals chuxe.Email
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
                                 Chuxe = chuxe.Email,
                                 DiaDiemNhanXe = yc.Diadiemnhanxe,
                                 BienSoXe = yc.Biensoxe,
                                 Trangthaiyc = trangthai.Tentrangthai,
                                 TongTienThue = yc.Tongtienthue,
                                 Sotiencantra = yc.Sotiencantra,
                                 Baohiemthuexe = yc.Baohiemthuexe,
                                 HoTen = chuxe.HoTen,
                                 Sdt = chuxe.Sdt,
                                 SoGplxB2 = chuxe.SoGplxB2,
                                 HinhGplxb2 = chuxe.HinhGplxb2,
                                 SoCccd = chuxe.SoCccd,
                                 HinhCccd = chuxe.HinhCccd,
                                 httt = ht.TenHt
                             }).FirstOrDefault();
            if (chitietyc == null)
            {
                return NotFound();
            }
            return View(chitietyc);
        }
        [HttpPost]
        public IActionResult Nhanxe(int mayc)
        {
            var yeuCau = _context.Yeucauthuexes.FirstOrDefault(y => y.MaYc == mayc);
            if (yeuCau == null)
            {
                return NotFound();
            }
            yeuCau.Matt = 5;
            _context.Update(yeuCau);
            _context.SaveChanges();
            TempData["success"] = "Đã nhận xe thành công";
            return RedirectToAction("DanhsachThuexeCanThanhToanTienCoc", "Thuexetulai");
        }
        public IActionResult Chitietycdanhanxe(int mayc)
        {
            var chitietyc = (from yc in _context.Yeucauthuexes
                             join xe in _context.Xes on yc.Biensoxe equals xe.Biensoxe
                             join chuxe in _context.Taikhoans on yc.Chuxe equals chuxe.Email
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
                                 Chuxe = chuxe.Email,
                                 DiaDiemNhanXe = yc.Diadiemnhanxe,
                                 BienSoXe = yc.Biensoxe,
                                 Trangthaiyc = trangthai.Tentrangthai,
                                 TongTienThue = yc.Tongtienthue,
                                 Sotiencantra = yc.Sotiencantra,
                                 Baohiemthuexe = yc.Baohiemthuexe,
                                 HoTen = chuxe.HoTen,
                                 Sdt = chuxe.Sdt,
                                 SoGplxB2 = chuxe.SoGplxB2,
                                 HinhGplxb2 = chuxe.HinhGplxb2,
                                 SoCccd = chuxe.SoCccd,
                                 HinhCccd = chuxe.HinhCccd,
                                 httt = ht.TenHt
                             }).FirstOrDefault();
            if (chitietyc == null)
            {
                return NotFound();
            }
            return View(chitietyc);
        }
        [HttpPost]
        public IActionResult Hoanthanhgiaodich(int mayc)
        {
            var yeuCau = _context.Yeucauthuexes.FirstOrDefault(y => y.MaYc == mayc);
            if (yeuCau == null)
            {
                return NotFound();
            }
            yeuCau.Matt = 6;
            yeuCau.Sotiencantra = 0;
            _context.Update(yeuCau);
            var hoaDonThueXe = _context.HoadonThuexes.FirstOrDefault(h => h.MaYc == mayc);
            if (hoaDonThueXe != null)
            {
                hoaDonThueXe.Sotiencantra = 0;
                _context.HoadonThuexes.Update(hoaDonThueXe);
            }
            var hoaDonchothuexe = _context.HoaDonChoThueXes.FirstOrDefault(h => h.MaYc == mayc);
            if (hoaDonchothuexe != null)
            {
                hoaDonchothuexe.Tongtiennhanduoc = yeuCau.Tongtienthue;
                _context.HoaDonChoThueXes.Update(hoaDonchothuexe);
            }
            _context.SaveChanges();
            TempData["success"] = "Hoàn thành giao dịch";
            return RedirectToAction("DanhsachThuexeCanThanhToanTienCoc", "Thuexetulai");
        }
    }
}